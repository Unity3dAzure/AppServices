using UnityEngine;
using System.Collections;
using System;

namespace Unity3dAzure.AppServices
{
	public class MobileServiceTable<E> : IAzureMobileServiceTable
	{
		private MobileServiceClient _client;
		private string _name;
        
		public const string URI_TABLES = "tables/";

		public MobileServiceTable (string tableName, MobileServiceClient client)
		{
			_client = client;
			_name = tableName; // NB: The table name could be infered from the Table's DataModel using typeof(E).Name; but passing Table name as a string allows for the case when the Classname is not the same as the Table name.
		}

		public override string ToString ()
		{
			return _name;
		}

		public IEnumerator Insert<T> (T item, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}", _client.AppUrl, URI_TABLES, _name);
			ZumoRequest request = new ZumoRequest (_client, url, Method.POST);
			Debug.Log ("Insert Request: " + url);
			request.AddBody (item);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

		public IEnumerator Read<T> (Action<IRestResponse<T[]>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}", _client.AppUrl, URI_TABLES, _name);
			ZumoRequest request = new ZumoRequest (_client, url, Method.GET);
			Debug.Log ("Read Request: " + url);
			yield return request.request.Send ();
			request.ParseJsonArray<T> (callback);
		}

		public IEnumerator Query<T> (CustomQuery query, Action<IRestResponse<T[]>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}{3}", _client.AppUrl, URI_TABLES, _name, query);
			ZumoRequest request = new ZumoRequest (_client, url, Method.GET);
			Debug.Log ("Query Request: " + url + " Query:" + query);
			yield return request.request.Send ();
			request.ParseJsonArray<T> (callback);
		}
        /*
		public IEnumerator Query<T,N> (CustomQuery query, Action<IRestResponse<N>> callback = null) where N : INestedResults<T> where T : new()
        {
			string queryString = query.ToString ();
			string q = queryString.Length > 0 ? "&" : "?";
			queryString += string.Format ("{0}$inlinecount=allpages", q);
			string url = string.Format ("{0}/{1}{2}{3}", _client.AppUrl, URI_TABLES, _name, queryString);
			Debug.Log ("Query Request: " + url + " Paginated Query:" + query);
			ZumoRequest request = new ZumoRequest (_client, url, Method.GET);
			yield return request.request.Send ();
			request.TryParseJsonNestedArray<T,N> ("results", callback);
		}
        */
        public IEnumerator Query<T>(CustomQuery query, Action<IRestResponse<NestedResults<T>>> callback = null) where T : new()
        {
            string queryString = query.ToString();
            string q = queryString.Length > 0 ? "&" : "?";
            queryString += string.Format("{0}$inlinecount=allpages", q);
            string url = string.Format("{0}/{1}{2}{3}", _client.AppUrl, URI_TABLES, _name, queryString);
            Debug.Log("Query Request: " + url + " Paginated Query:" + query);
            ZumoRequest request = new ZumoRequest(_client, url, Method.GET);
            yield return request.request.Send();
            request.ParseJsonNestedArray<T, NestedResults<T>>("results", callback);
        }

        public IEnumerator Update<T> (T item, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string id = null;
			// Check if model uses the 'IDataModel' Interface to get id property, otherwise try Refelection (using 'Model' helper).
			IDataModel model = item as IDataModel;
			if (model != null) {
				id = model.GetId ();
			} else if (Model.HasField (item, "id")) {
				var x = Model.GetField (item, "id");
				id = x.GetValue (item) as string;
			} else {
				Debug.LogError ("Unable to get 'id' data model property");
			}
			if (string.IsNullOrEmpty (id)) {
				Debug.LogError ("Error 'id' value is missing");
				yield return null;
			}
			string url = string.Format ("{0}/{1}{2}/{3}", _client.AppUrl, URI_TABLES, _name, id);
			ZumoRequest request = new ZumoRequest (_client, url, Method.PATCH);
			request.AddBody (item);
			Debug.Log ("Update Request Url: " + url + " patch:" + item);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

		public IEnumerator Delete<T> (string id, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}/{3}", _client.AppUrl, URI_TABLES, _name, id);
			ZumoRequest request = new ZumoRequest (_client, url, Method.DELETE);
			Debug.Log ("Delete Request Url: " + url);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

		public IEnumerator Lookup<T> (string id, Action<IRestResponse<T>> callback = null) where T : new()
		{
			string url = string.Format ("{0}/{1}{2}/{3}", _client.AppUrl, URI_TABLES, _name, id);
			ZumoRequest request = new ZumoRequest (_client, url, Method.GET);
			Debug.Log ("Lookup Request Url: " + url);
			yield return request.request.Send ();
			request.ParseJson<T> (callback);
		}

	}
}