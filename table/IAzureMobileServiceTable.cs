using System.Collections;
using System.Collections.Generic;
using System;
using RestSharp;

namespace Unity3dAzure.AppServices
{
    /// <remarks>
    /// NB: Interface uses callbacks as Unity does not support async methods right now. 
    /// An async method would mean we could just return result as success or failure and eliminate need for a callback. 
    /// <example>
    /// List<T> Read<T>() where T : new();
    /// </example>
    /// </remarks>
	[CLSCompliant(false)]
    public interface IAzureMobileServiceTable
    {
        /// <summary>
        /// Create a new item. 
        /// </summary>
        void Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
        
        /// <summary>
        /// Get a list of items. 
        /// </summary>
        void Read<T>(Action<IRestResponse<List<T>>> callback = null) where T : new();
        
        /// <summary>
        /// Update an item's data using id property. 
        /// </summary>
        void Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
        
        /// <summary>
        /// Delete an item using id property. 
        /// </summary>
        void Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
        
        /// <summary>
        /// Get a list of results using a custom query. 
        /// More info about custom queries here: 
        /// https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
        /// </summary>
        void Query<T>(CustomQuery query, Action<IRestResponse<List<T>>> callback = null) where T : new();
        
        /// <summary>
        /// Get an item's data using id property. 
        /// </summary>
        void Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
    }
}