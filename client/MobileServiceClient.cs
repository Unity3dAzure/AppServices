using UnityEngine;
using System.Collections;
using RestSharp;
using System.Collections.Generic;
using System;

namespace Unity3dAzure.MobileServices
{
    public class MobileServiceClient : RestClient, IAzureMobileServiceClient
    {        
        public string AppUrl { get; private set; }
        public string AppKey { get; private set; }
        
        public MobileServiceUser User { get; set; }
        
        public const string URI_API = "api/";
        
        /// <summary>
        /// Creates a new RestClient using Azure Mobile Service's Application Url
        /// </summary>
        public MobileServiceClient(string appUrl, string appKey) : base(appUrl)
        {
            AppUrl = appUrl;
            AppKey = appKey;
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
            string uri = "login/" + provider.ToString().ToLower();
            ZumoRequest request = new ZumoRequest(this, uri, Method.POST);
            Debug.Log( "Login Request Uri: " + uri );
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
        // TODO: Implement custom API (using GET request)
        /// </summary>
        public void InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new()
        {
            string uri = URI_API + apiName;
            ZumoRequest request = new ZumoRequest(this, uri, Method.GET);
            Debug.Log( "Custom API Request Uri: " + uri );
            this.ExecuteAsync(request, callback);
        }
    }
}