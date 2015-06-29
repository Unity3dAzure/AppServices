using System.Collections;
using System.Collections.Generic;
using System;
using RestSharp;

namespace Unity3dAzure.MobileServices
{
    public interface IAzureMobileServiceClient
    {
        MobileServiceTable<E> GetTable<E>(string tableName) where E : class;
        
        /// <summary>
        /// Client-directed login (sdk) for single sign on
        /// https://msdn.microsoft.com/en-us/library/azure/jj710106.aspx
        /// </summary>
        void Login(MobileServiceAuthenticationProvider provider, string token, Action<IRestResponse<MobileServiceUser>> callback = null);
        
        /// <summary>
        /// TODO: Service-directed login (webview)
        /// </summary>
        //void Login(MobileServiceAuthenticationProvider provider);
        
        /// <summary>
        /// TODO: Custom API
        /// </summary>
        void InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new();
    }
}