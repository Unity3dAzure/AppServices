using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public abstract class RestRequest
	{
		public UnityWebRequest request { get; private set; }

		public RestRequest (string url, Method method)
		{
			request = new UnityWebRequest (url, method.ToString ());
			request.downloadHandler = new DownloadHandlerBuffer ();
		}

		public void AddHeader (string key, string value)
		{
			request.SetRequestHeader (key, value);
		}

		public void AddBody (byte[] bytes, string contentType)
		{
			if (request.uploadHandler != null) {
				Debug.LogWarning ("Request body can only be set once");
				return;
			}
			request.uploadHandler = new UploadHandlerRaw (bytes);
			request.uploadHandler.contentType = contentType;
		}

		public virtual void AddBody<T> (T data, string contentType = "application/json; charset=utf-8")
		{
			string jsonString = JsonUtility.ToJson (data);
			byte[] bytes = DataHelper.ToUTF8Bytes (jsonString);
			this.AddBody (bytes, contentType);
		}

		#region Response and json object parsing

		/// <summary>
		/// Shared method to return response result whether an object or array of objects
		/// </summary>
		private RestResult<T> ParseResultAsAnArray<T> (bool isArray)
		{
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse (typeof(HttpStatusCode), request.responseCode.ToString ());
			RestResult<T> result = new RestResult<T> (statusCode);

			if (result.IsError) {
				result.ErrorMessage = "Response failed with status: " + statusCode.ToString ();
				return result;
			}

			if (string.IsNullOrEmpty (request.downloadHandler.text)) {
				result.IsError = true;
				result.ErrorMessage = "Response has empty body";
				return result;
			}

			if (isArray) {
				// try parse an array of objects
				result.AnArrayOfObjects = JsonHelper.GetJsonArray<T> (request.downloadHandler.text);
				if (result.AnArrayOfObjects == null) {
					result.IsError = true;
					result.ErrorMessage = "Failed to parse an array of objects of type: " + typeof(T).ToString ();
				}
			} else {
				// try parse an object
				result.AnObject = JsonUtility.FromJson<T> (request.downloadHandler.text);
				if (result.AnObject == null) {
					result.IsError = true;
					result.ErrorMessage = "Failed to parse object of type: " + typeof(T).ToString ();
				}
			}
			return result;
		}

		/// <summary>
		/// Parses object with T data = JsonUtil.FromJson<T>, then callback RestResponse<T>
		/// </summary>
		public void ParseData<T> (Action<IRestResponse<T>> callback = null)
		{
			RestResult<T> result = ParseResultAsAnArray<T> (false);

			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse<T> (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse<T> (result.StatusCode, request.url, request.downloadHandler.text, result.AnObject));
			}

			// all done, clean-up
			request.Dispose ();
		}

		/// <summary>
		/// Parses array of objects with T[] data = JsonHelper.GetJsonArray<T>, then callback RestResponse<T[]>
		/// </summary>
		public void ParseDataArray<T> (Action<IRestResponse<T[]>> callback = null)
		{
			RestResult<T> result = ParseResultAsAnArray<T> (true);

			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse<T[]> (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse<T[]> (result.StatusCode, request.url, request.downloadHandler.text, result.AnArrayOfObjects));
			}

			// all done, clean-up
			request.Dispose ();
		}

		#endregion

	}
}
