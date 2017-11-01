// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System;
using System.Collections;

namespace Azure.AppServices {
  public interface IAppServiceTable {
    /// <summary>
    /// Create a new item.
    /// </summary>
    IEnumerator Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();

    /// <summary>
    /// Get a list of items.
    /// </summary>
    IEnumerator Read<T>(Action<IRestResponse<T[]>> callback = null) where T : new();

    /// <summary>
    /// Update an item's data using id property.
    /// </summary>
    IEnumerator Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();

    /// <summary>
    /// Delete an item using id property.
    /// </summary>
    IEnumerator Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();

    /// <summary>
    /// Get a list of results using an App Service table query.
    /// More info about queries here:
    /// https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
    /// </summary>
    IEnumerator Query<T>(TableQuery query, Action<IRestResponse<T[]>> callback = null) where T : new();

    /// <summary>
    /// Returns a 'count' and nested list of 'results' (appends `$inlinecount=allpages` parameter to the query)
    /// </summary>
    IEnumerator Query<T>(TableQuery query, Action<IRestResponse<NestedResults<T>>> callback = null) where T : new();

    /// <summary>
    /// Get an item's data using id property.
    /// </summary>
    IEnumerator Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
  }
}
