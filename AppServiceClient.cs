// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System;
using System.Collections;
using UnityEngine;

namespace Azure.AppServices {
  public sealed class AppServiceClient : ZumoClient {

    public AppServiceClient(string url) : base(url) {
    }

    public static AppServiceClient Create(string account) {
      string url = AppUrl(account);
      return new AppServiceClient(url);
    }

    private const string URI_API = "api";

    public AppServiceTable<E> GetTable<E>(string tableName) where E : class {
      return new AppServiceTable<E>(tableName, this);
    }

    /// <summary>
		/// GET custom API
		/// </summary>
		public IEnumerator InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new() {
      return InvokeApi<T>(apiName, Method.GET, callback);
    }

    /// <summary>
    /// Invokes custom API for HTTP Methods: GET, POST, PUT, PATCH, DELETE
    /// </summary>
    public IEnumerator InvokeApi<T>(string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new() {
      string url = ApiUrl(apiName);
      Debug.Log(httpMethod.ToString() + " custom API Request Url: " + url);
      ZumoRequest request = new ZumoRequest(url, httpMethod, true, User);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    /// <summary>
    /// Invokes custom API with body (of type B) and returning response (of type T)
    /// </summary>
    public IEnumerator InvokeApi<B, T>(string apiName, Method httpMethod, B body, Action<IRestResponse<T>> callback = null) where T : new() {
      string url = ApiUrl(apiName);
      Debug.Log(httpMethod.ToString() + " custom API Request Url: " + url);
      ZumoRequest request = new ZumoRequest(url, httpMethod, true, User);
      request.AddBody<B>(body);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    private string ApiUrl(string apiName) {
      return string.Format("{0}/{1}/{2}", Url, URI_API, apiName);
    }

  }
}
