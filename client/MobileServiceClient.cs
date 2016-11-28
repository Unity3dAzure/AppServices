using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;


#if !NETFX_CORE || UNITY_ANDROID
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
#endif

namespace Unity3dAzure.AppServices
{
	public class MobileServiceClient : IAzureMobileServiceClient
	{
		public string AppUrl { get; private set; }

		public MobileServiceUser User { get; set; }

		public const string URI_API = "api/";

		/// <summary>
		/// Creates a new RestClient using Azure App Service's Application Url
		/// </summary>
		public MobileServiceClient (string appUrl)
		{
			AppUrl = HttpsUri (appUrl);
			Debug.Log ("App Url: " + AppUrl);

			// required for running in Windows and Android
			#if !NETFX_CORE || UNITY_ANDROID
			Debug.Log ("ServerCertificateValidation");
			ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
			#endif
		}

		public override string ToString ()
		{
			return this.AppUrl;
		}

		public MobileServiceTable<E> GetTable<E> (string tableName) where E : class
		{
			return new MobileServiceTable<E> (tableName, this);
		}

		/// <summary>
		/// Client-directed single sign on (POST with access token)
		/// </summary>
		public IEnumerator Login (MobileServiceAuthenticationProvider provider, string token, Action<IRestResponse<MobileServiceUser>> callback = null)
		{
			string p = provider.ToString ().ToLower ();
			string url = string.Format ("{0}/.auth/login/{1}", AppUrl, p);
			Debug.Log ("Login Request Url: " + url + " access token: " + token);
			ZumoRequest request = new ZumoRequest (this, url, Method.POST);
			request.AddBodyAccessToken (token);
			yield return request.request.Send ();
			request.ParseJson<MobileServiceUser> (callback);
		}

		/// <summary>
		/// TODO: Service login (using GET via webview)
		/// </summary>
		/*
        public void Login(MobileServiceAuthenticationProvider provider)
        {
            Debug.Log("TODO");
        }
        //*/

		/// <summary>
		/// GET custom API
		/// </summary>
		public IEnumerator InvokeApi<T> (string apiName, Action<IRestResponse<T>> callback = null) where T : new()
		{
			return InvokeApi<T> (apiName, Method.GET, callback);
		}

		/// <summary>
		/// Invokes custom API for HTTP Methods: GET, POST, PUT, PATCH, DELETE
		/// </summary>
		public IEnumerator InvokeApi<T> (string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}", AppUrl, URI_API, apiName);
			Debug.Log (httpMethod.ToString () + " custom API Request Url: " + url);
			ZumoRequest request = new ZumoRequest (this, url, httpMethod);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

		/// <summary>
		/// Invokes custom API with body
		/// </summary>
		public IEnumerator InvokeApi<T> (string apiName, Method httpMethod, T body, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}", AppUrl, URI_API, apiName);
			Debug.Log (httpMethod.ToString () + " custom API Request Url: " + url);
			ZumoRequest request = new ZumoRequest (this, url, httpMethod);
			request.AddBody<T> (body);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

		/// <summary>
		/// When you copy the URL from the Azure Portal it is 'http' by default, but it needs to be 'https' for post
		/// </summary>
		private static string HttpsUri (string appUrl)
		{
			return Regex.Replace (appUrl, "(?si)^http://", "https://").TrimEnd ('/');
		}

		#if !NETFX_CORE || UNITY_ANDROID
		private bool RemoteCertificateValidationCallback (System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			//   Check the certificate to see if it was issued from Azure
			if (certificate.Subject.Contains ("azurewebsites.net")) {
				return true;
			} else {
				return false;
			}
		}
		#endif

	}
}
