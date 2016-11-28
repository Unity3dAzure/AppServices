using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public abstract class RestRequest : IDisposable
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

        private RestResult<T> GetRestResult<T>()
        {
            HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), request.responseCode.ToString());
            RestResult<T> result = new RestResult<T>(statusCode);

            if (result.IsError)
            {
                result.ErrorMessage = "Response failed with status: " + statusCode.ToString();
                return result;
            }

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                result.IsError = true;
                result.ErrorMessage = "Response has empty body";
                return result;
            }

            return result;
        }

		/// <summary>
		/// Shared method to return response result whether an object or array of objects
		/// </summary>
		private RestResult<T> TryParseJsonArray<T> ()
		{
            RestResult<T> result = GetRestResult<T>();
            // try parse an array of objects
            try
            {
                result.AnArrayOfObjects = JsonHelper.FromJsonArray<T>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrorMessage = "Failed to parse an array of objects of type: " + typeof(T).ToString() + " Exception message: " + e.Message;
            }
			return result;
		}

        private RestResult<T> TryParseJson<T>()
        {
            RestResult<T> result = GetRestResult<T>();
            // try parse an object
            try
            {
                result.AnObject = JsonUtility.FromJson<T>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrorMessage = "Failed to parse object of type: " + typeof(T).ToString() + " Exception message: " + e.Message;
            }
            return result;
        }

        /// <summary>
        /// Parses object with T data = JsonUtil.FromJson<T>, then callback RestResponse<T>
        /// </summary>
        public void ParseJson<T> (Action<IRestResponse<T>> callback = null)
		{
			RestResult<T> result = TryParseJson<T> ();

			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse<T> (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse<T> (result.StatusCode, request.url, request.downloadHandler.text, result.AnObject));
			}
            this.Dispose();
		}

		/// <summary>
		/// Parses array of objects with T[] data = JsonHelper.GetJsonArray<T>, then callback RestResponse<T[]>
		/// </summary>
		public void ParseJsonArray<T> (Action<IRestResponse<T[]>> callback = null)
		{
			RestResult<T> result = TryParseJsonArray<T> ();

			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse<T[]> (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse<T[]> (result.StatusCode, request.url, request.downloadHandler.text, result.AnArrayOfObjects));
			}
            this.Dispose();
        }

        // *WSA
        private RestResult<N> TryParseJsonNestedArray<T,N>(string namedArray) where N : INestedResults<T>, new()
        {
            RestResult<N> result = GetRestResult<N>();
            // try parse an object
            try
            {
                result.AnObject = JsonHelper.FromJsonNestedArray<T,N>(request.downloadHandler.text, namedArray); //JsonUtility.FromJson<N>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrorMessage = "Failed to parse object of type: " + typeof(N).ToString() + " Exception message: " + e.Message;
            }
            return result;
        }

        // Work-around for nested array
        public void ParseJsonNestedArray<T,N>(string namedArray, Action<IRestResponse<N>> callback = null) where N : INestedResults<T>, new()
        {
            RestResult<N> result = TryParseJsonNestedArray<T,N>(namedArray);

            if (result.IsError)
            {
                Debug.LogWarning("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
                callback(new RestResponse<N>(result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
            }
            else
            {
                callback(new RestResponse<N>(result.StatusCode, request.url, request.downloadHandler.text, result.AnObject));
            }
            this.Dispose();
        }

        public void Dispose()
        {
            request.Dispose(); // request completed, clean-up resources
        }

        #endregion

    }
}
