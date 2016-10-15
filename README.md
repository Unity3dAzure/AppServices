# Azure App Services for Unity3d
For game developers looking to use Azure App Services (previously Mobile Services) in their Unity project.

## How to setup App Services with a new Unity project
1. [Download UnityRestClient](https://github.com/ProjectStratus/UnityRestClient/archive/master.zip)
 	* Copy JsonFx 'plugins' and the UnityRestClient 'Source' into project `Assets` folder
2. [Download AppServices](https://github.com/Unity3dAzure/AppServices/archive/master.zip)  
	* Copy 'AppServices' into project `Assets` folder.
3. Create an Azure App Service [Mobile App](https://portal.azure.com)
	* Create a Table (using Easy Tables) for app data.

## Azure App Services Demos for Unity 5
Try the [Azure App Services Demos](https://github.com/Unity3dAzure/AppServicesDemo) project for Unity 5 on Mac / Windows. (The demo project has got everything already bundled in and does not require any additional assets to work. Just wire it up with your [Azure App Service](https://portal.azure.com) and run it right inside the Unity Editor.)
For detailed instructions read my developer blog on [how to setup Azure App Services and Unity demo project](http://www.deadlyfingers.net/azure/azure-app-services-for-unity3d/).

## Supported Features
### MobileServiceClient
API | Description
--- | -----------
Login | Client-directed login using access token.
InvokeApi | Invoke custom API (Easy API) using GET, POST, PUT, PATCH or DELETE

### MobileServiceTable
API | Description
--- | -----------
Insert | Create a new item.
Read | Get a list of items.
Update | Update an item’s data using id property.
Delete | Delete an item using id property.  
Query | Get a list of results using a custom query.
Lookup | Get an item’s data using id property.

### MobileServiceClient Interface
	void Login(MobileServiceAuthenticationProvider provider, string token, Action<IRestResponse<MobileServiceUser>> callback = null);
	void InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new();

### MobileServiceTable Interface
	void Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	void Read<T>(Action<IRestResponse<List<T>>> callback = null) where T : new();
	void Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	void Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
	void Query<T>(CustomQuery query, Action<IRestResponse<List<T>>> callback = null) where T : new();
	void Query<T>(CustomQuery query, Action<IRestResponse<T>> callback = null) where T : INestedResults, new();
	void Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();

## Sample usage
```
using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using Pathfinding.Serialization.JsonFx;
using Unity3dAzure.AppServices;

```

```
private MobileServiceClient _client;
private MobileServiceTable<TodoItem> _table;
```

```
void Start () {
	_client = new MobileServiceClient(appUrl); // <- add your app url here.
	_table = _client.GetTable<TodoItem>("TodoItem");
}
```
```
private void ReadItems() {
	_table.Read<TodoItem>(OnReadItemsCompleted);
}

private void OnReadItemsCompleted(IRestResponse<List<TodoItem>> response) {
	if ( response.StatusCode == HttpStatusCode.OK) {
		Debug.Log("OnReadItemsCompleted data: " + response.Content);
		List<TodoItem> items = response.Data;
		Debug.Log( "Todo items count: " + items.Count);
	} else {
		ResponseError err = JsonReader.Deserialize<ResponseError>(response.Content);
		Debug.Log("Error " + err.code.ToString() + " " + err.error + " Uri: " + response.ResponseUri);
	}
}
```

## Dependencies
The REST service implements [UnityRestClient](https://github.com/ProjectStratus/UnityRestClient) which uses [JsonFx](https://bitbucket.org/TowerOfBricks/jsonfx-for-unity3d-git/) to parse JSON data.

## Supports
* iOS
* Android
* Windows

Questions or tweet #Azure #GameDev [@deadlyfingers](https://twitter.com/deadlyfingers)
