using System;
using System.Collections;

namespace Unity3dAzure.AppServices
{
	public interface IAzureMobileServiceTable
	{
		/// <summary>
		/// Create a new item. 
		/// </summary>
		IEnumerator Insert<T> (T item, Action<IRestResponse<T>> callback = null) where T : new();

		/// <summary>
		/// Get a list of items. 
		/// </summary>
		IEnumerator Read<T> (Action<IRestResponse<T[]>> callback = null) where T : new();

		/// <summary>
		/// Update an item's data using id property. 
		/// </summary>
		IEnumerator Update<T> (T item, Action<IRestResponse<T>> callback = null) where T : new();

		/// <summary>
		/// Delete an item using id property. 
		/// </summary>
		IEnumerator Delete<T> (string id, Action<IRestResponse<T>> callback = null) where T : new();

		/// <summary>
		/// Get a list of results using a custom query. 
		/// More info about custom queries here: 
		/// https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
		/// </summary>
		IEnumerator Query<T> (CustomQuery query, Action<IRestResponse<T[]>> callback = null) where T : new();

        /// <summary>
        /// Returns a 'count' and nested list of 'results' (appends `$inlinecount=allpages` parameter to the query)
        /// </summary>
        IEnumerator Query<T>(CustomQuery query, Action<IRestResponse<NestedResults<T>>> callback = null) where T : new();

        /// <summary>
        /// Get an item's data using id property. 
        /// </summary>
        IEnumerator Lookup<T> (string id, Action<IRestResponse<T>> callback = null) where T : new();
	}
}