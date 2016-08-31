using UnityEngine;
using System.Collections;
using RestSharp;
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
	[CLSCompliant(false)]
    public class MobileServiceClient : RestClient, IAzureMobileServiceClient
    {
        public string AppUrl { get; private set; }
        public string AppKey { get; private set; }

        public MobileServiceUser User { get; set; }

        public const string URI_API = "api/";

        /// <summary>
        /// Creates a new RestClient using Azure Mobile Service's Application Url and App Key
        /// NB: Mobile Services should be migrated to use Azure App Service with constructor below.
        /// </summary>
        public MobileServiceClient(string appUrl, string appKey) : base(appUrl)
        {
            AppUrl = appUrl;
            AppKey = appKey;

            // required for running in Windows and Android
            #if !NETFX_CORE || UNITY_ANDROID
            Debug.Log("ServerCertificateValidation");
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            #endif
        }

    	/// <summary>
    	/// Creates a new RestClient using Azure App Service's Application Url
    	/// </summary>
    	public MobileServiceClient(string appUrl) : base(appUrl)
    	{
			AppUrl = appUrl;
			Debug.Log("AppUrl: " + AppUrl);

            // required for running in Windows and Android
            #if !NETFX_CORE || UNITY_ANDROID
            Debug.Log("ServerCertificateValidation");
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            #endif
        }

		/// <summary>
		/// Using factory method forces app url to be changed from 'http' to 'https' url
		/// </summary>
		public static MobileServiceClient Create(string appUrl)
		{
			return new MobileServiceClient (ForceHttps (appUrl));
		}	

        public override string ToString()
        {
            return this.BaseUrl;
        }

        public MobileServiceTable<E> GetTable<E>(string tableName) where E : class
        {
            return new MobileServiceTable<E>(tableName, this);
        }

        /// <summary>
        /// Client-directed single sign on (POST with access token)
        /// </summary>
        public void Login(MobileServiceAuthenticationProvider provider, string token, Action<IRestResponse<MobileServiceUser>> callback = null)
        {
            string p = provider.ToString().ToLower();
            string uri = IsAppService() ? ".auth/login/" + p : "login/" + p;
            ZumoRequest request = new ZumoRequest(this, uri, Method.POST);
            Debug.Log("Login Request Uri: " + uri + " access token: " + token);
            request.AddBodyAccessToken(token);
            this.ExecuteAsync(request, callback);
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
        /// TODO: Implement custom API (using GET request)
        /// </summary>
        public void InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new()
        {
            string uri = URI_API + apiName;
            ZumoRequest request = new ZumoRequest(this, uri, Method.GET);
            Debug.Log( "Custom API Request Uri: " + uri );
            this.ExecuteAsync(request, callback);
        }

		/// <summary>
		/// When you copy the URL is is 'http' by default, but its preferable to use 'https'
		/// </summary>
		private static string ForceHttps(string appUrl) 
		{
			return Regex.Replace(appUrl, "(?m)http://", "https://");
		}

        /// <summary>
        /// Mobile Service uses an AppKey, but App Service does not.
        /// </summary>
        public bool IsAppService()
        {
          return String.IsNullOrEmpty(AppKey);
        }

        #if !NETFX_CORE || UNITY_ANDROID
        private bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //   Check the certificate to see if it was issued from Azure
            if (certificate.Subject.Contains("azurewebsites.net"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endif

    }
}
