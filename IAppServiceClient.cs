// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System;
using System.Collections;

namespace Azure.AppServices {
  public interface IAppServiceClient : IZumoClient {
    AppServiceTable<E> GetTable<E>(string tableName) where E : class;

    /// <summary>
    /// Invokes a custom API using GET HTTP method
    /// </summary>
    IEnumerator InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new();

    /// <summary>
    /// Invokes a custom API for HTTP Methods: GET, POST, PUT, PATCH, DELETE
    /// </summary>
    IEnumerator InvokeApi<T>(string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new();

    /// <summary>
    /// Invokes a custom API with body object
    /// </summary>
    IEnumerator InvokeApi<B, T>(string apiName, Method httpMethod, B body, Action<IRestResponse<T>> callback = null) where T : new();
  }
}
