using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public abstract class RestRequest {

		public UnityWebRequest request { get; private set; }

		public RestRequest(string url, Method method) {
			request = new UnityWebRequest (url, method.ToString());
			request.downloadHandler = new DownloadHandlerBuffer();
		}

		public void AddHeader(string key, string value) {
			request.SetRequestHeader (key, value);
		}

		public void AddBody(byte[] bytes, string contentType) {
			if (request.uploadHandler != null) {
				Debug.LogWarning ("Request body can only be set once");
				return;
			}
			request.uploadHandler = new UploadHandlerRaw (bytes);
			request.uploadHandler.contentType = contentType;
		}

		public virtual void AddBody<T>(T data, string contentType="application/json; charset=utf-8")  {
			string jsonString = JsonUtility.ToJson (data);
			byte[] bytes = DataHelper.ToUTF8Bytes( jsonString );
			this.AddBody (bytes, contentType);
		}

		#region Response and json object parsing
		public void ParseData<T>(Action<IRestResponse<T>> callback = null)  {
			Debug.Log ("parse data response:" + request.url);
			int code = Convert.ToInt32 (request.responseCode);
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof (HttpStatusCode), request.responseCode.ToString());
			string text = request.downloadHandler.text;
			// TODO: add error handling
			Debug.Log ("HttpStatusCode:" + statusCode + " code:" + code + " request url:" + request.url);
			if (statusCode == HttpStatusCode.OK) {
				Debug.Log ("Status OK");
			}
			T data = JsonUtility.FromJson<T>(text);
			callback( new RestResponse<T> (request.url, statusCode, text, data) );
			request.Dispose ();
		}

		public void ParseDataArray<T>(Action<IRestResponse<T[]>> callback = null) {
			Debug.Log ("parse data array response:" + request.url);
			int code = Convert.ToInt32 (request.responseCode);
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof (HttpStatusCode), request.responseCode.ToString());
			string text = request.downloadHandler.text;
			// TODO: add error handling
			Debug.Log ("HttpStatusCode:" + statusCode + " code:" + code + " request url:" + request.url);
			if (statusCode == HttpStatusCode.OK) {
				Debug.Log ("Status OK");
			}
			T[] data = JsonHelper.GetJsonArray<T>(request.downloadHandler.text);
			callback ( new RestResponse<T[]> (request.url, statusCode, text, data) );
			request.Dispose ();
		}
		#endregion

	}
}
