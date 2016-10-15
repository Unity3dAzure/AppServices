using System.Collections;
using System.Collections.Generic;
using System;
using RestSharp;

namespace Unity3dAzure.AppServices
{
	[CLSCompliant(false)]
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
        /// Invokes a custom API using GET HTTP method
        /// </summary>
        void InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new();

		/// <summary>
		/// Invokes a custom API for HTTP Methods: GET, POST, PUT, PATCH, DELETE
		/// </summary>
		void InvokeApi<T>(string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new();

		/// <summary>
		/// Invokes a custom API with body object
		/// </summary>
		void InvokeApi<T> (string apiName, Method httpMethod, T body, Action<IRestResponse<T>> callback = null) where T : new();
    }
}