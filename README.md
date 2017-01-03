# Azure App Services for Unity3d
For game developers looking to use Azure App Services (previously Mobile Services) in their Unity project. This App Services REST API library for Unity is quite similar in structure to the Azure Mobile Services SDK so if you've used that before you should feel relatively at home.

## Prerequisites
Requires Unity v5.3 or greater as [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) and [JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) features are used. Unity will be extending platform support for UnityWebRequest so keep Unity up to date if you need to support these additional platforms.

## How to setup App Services with a new Unity project
1. [Download AppServices](https://github.com/Unity3dAzure/AppServices/archive/master.zip)  
	* Copy 'AppServices' into project `Assets` folder.
2. Create an Azure App Service [Mobile App](https://portal.azure.com)
	* Create a Table (using Easy Tables) for app data.

## Azure App Services Demos for Unity 5
Try the [Azure App Services Demos](https://github.com/Unity3dAzure/AppServicesDemo) project for Unity v5.4.x on Mac / Windows. (The demo project has got everything already bundled in and does not require any additional assets to work. Just wire it up with your [Azure App Service](https://portal.azure.com) and run it right inside the Unity Editor.)
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

### [MobileServiceClient Interface](https://github.com/Unity3dAzure/AppServices/blob/master/client/IAzureMobileServiceClient.cs)
	Login(MobileServiceAuthenticationProvider provider, string token,	Action<IRestResponse<MobileServiceUser>> callback = null);
	InvokeApi<T>(string apiName, Action<IRestResponse<T>> callback = null) where T : new();
	InvokeApi<T>(string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new();
	InvokeApi<T>(string apiName, Method httpMethod, T body, Action<IRestResponse<T>> callback = null) where T : new();

### [MobileServiceTable Interface](https://github.com/Unity3dAzure/AppServices/blob/master/table/IAzureMobileServiceTable.cs)
	Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	Read<T>(Action<IRestResponse<T[]>> callback = null) where T : new();
	Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
	Query<T>(CustomQuery query, Action<IRestResponse<T[]>> callback = null) where T : new();
	Query<T>(CustomQuery query, Action<IRestResponse<T>> callback = null) where T : INestedResults, new();
	Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();

## Sample usage
```
using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;
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
	StartCoroutine( _table.Read<TodoItem>(OnReadItemsCompleted) );
}

private void OnReadItemsCompleted(IRestResponse<TodoItem[]> response) {
	if (!response.IsError) {
		Debug.Log ("OnReadCompleted: " + response.Url + " data: " + response.Content);
		TodoItem[] items = response.Data;
		Debug.Log ( "Todo items count: " + items.Count);
	} else {
		Debug.LogWarning ("Read Error Status:" + response.StatusCode + " Url: " + response.Url);
	}
}
```

## Known issues
* There is an issue with [PATCH on Android using UnityWebRequest with Azure App Services](http://answers.unity3d.com/questions/1230067/trying-to-use-patch-on-a-unitywebrequest-on-androi.html). Android won't recognize the "PATCH" http method currently required to update an item in App Services. One workaround is to enable the `X-HTTP-Method-Override` header. Here's the quick fix for App Services running node backend:
    1. Install the "method-override" package.  
        ```
        npm install method-override --save
        ```  
    2. In 'app.js' file insert:  
        ```
        var methodOverride = require('method-override');  
        // after the line "var app = express();" add  
        app.use(methodOverride('X-HTTP-Method-Override'));
        ```

This will enable PATCH requests to be sent on Android.

## Supported platforms
Will work on all the platforms [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) supports including:
* Unity Editor and Standalone players
* iOS
* Android
* Windows

Questions or tweet #Azure #GameDev [@deadlyfingers](https://twitter.com/deadlyfingers)
