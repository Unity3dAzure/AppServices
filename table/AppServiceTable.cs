// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using UnityEngine;
using System.Collections;
using System;

namespace Azure.AppServices {
  public class AppServiceTable<E> : IAppServiceTable {
    private AppServiceClient _client;
    private string _name;

    private const string URI_TABLES = "tables";

    public AppServiceTable(string tableName, AppServiceClient client) {
      _client = client;
      _name = tableName; // NB: The table name could be infered from the Table's DataModel using typeof(E).Name; but passing Table name as a string allows for the case when the Classname is not the same as the Table name.
    }

    public override string ToString() {
      return _name;
    }

    public IEnumerator Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new() {
      string url = TableUrl(_name);
      ZumoRequest request = new ZumoRequest(url, Method.POST, true, _client.User);
      Debug.Log("Insert Request: " + url);
      request.AddBody(item);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    public IEnumerator Read<T>(Action<IRestResponse<T[]>> callback = null) where T : new() {
      string url = TableUrl(_name);
      ZumoRequest request = new ZumoRequest(url, Method.GET, true, _client.User);
      Debug.Log("Read Request: " + url);
      yield return request.Request.Send();
      request.ParseJsonArray<T>(callback);
    }

    public IEnumerator Query<T>(TableQuery query, Action<IRestResponse<T[]>> callback = null) where T : new() {
      string url = TableQueryUrl(_name, query.ToString());
      ZumoRequest request = new ZumoRequest(url, Method.GET, true, _client.User);
      Debug.Log("Query Request: " + url + " Query:" + query);
      yield return request.Request.Send();
      request.ParseJsonArray<T>(callback);
    }

    public IEnumerator Query<T>(TableQuery query, Action<IRestResponse<NestedResults<T>>> callback = null) where T : new() {
      string q = query.ToString();
      string nestedQueryString = string.Format("{0}{1}$inlinecount=allpages", q, (q.Length > 1) ? "&" : "?");
      string url = TableQueryUrl(_name, nestedQueryString);
      Debug.Log("Query Request: " + url + " Paginated Query:" + query);
      ZumoRequest request = new ZumoRequest(url, Method.GET, true, _client.User);
      yield return request.Request.Send();
      request.ParseJsonNestedArray<T, NestedResults<T>>("results", callback);
    }

    public IEnumerator Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new() {
      string id = null;
      // Check if item uses the 'IDataModel' Interface to get 'id' property, otherwise try Refelection (using Reflection helper).
      IDataModel model = item as IDataModel;
      if (model != null) {
        id = model.GetId();
      } else if (ReflectionHelper.HasField(item, "id")) {
        var x = ReflectionHelper.GetField(item, "id");
        id = x.GetValue(item) as string;
      } else {
        Debug.LogError("Unable to get 'id' data model property");
      }
      if (string.IsNullOrEmpty(id)) {
        Debug.LogError("Error 'id' value is missing");
        yield return null;
      }
      string url = TableUrl(_name, id);
      ZumoRequest request = new ZumoRequest(url, Method.PATCH, true, _client.User);
      request.AddBody(item);
      Debug.Log("Update Request Url: " + url + " patch:" + item);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    public IEnumerator Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new() {
      string url = TableUrl(_name, id);
      ZumoRequest request = new ZumoRequest(url, Method.DELETE, true, _client.User);
      Debug.Log("Delete Request Url: " + url);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    public IEnumerator Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new() {
      string url = TableUrl(_name, id);
      ZumoRequest request = new ZumoRequest(url, Method.GET, true, _client.User);
      Debug.Log("Lookup Request Url: " + url);
      yield return request.Request.Send();
      request.ParseJson<T>(callback);
    }

    private string TableUrl(string tableName, string path = "") {
      string resourcePath = string.IsNullOrEmpty(path) ? tableName : tableName + "/" + path;
      return string.Format("{0}/{1}/{2}", _client.Url, URI_TABLES, resourcePath);
    }

    private string TableQueryUrl(string tableName, string query) {
      return string.Format("{0}/{1}/{2}{3}", _client.Url, URI_TABLES, tableName, query);
    }

  }
}
