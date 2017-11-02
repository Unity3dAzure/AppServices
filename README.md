# Azure App Services for Unity
For Unity developers looking to use Azure App Services (previously Mobile Services) in their Unity game / app.

## What's new
Please be aware this latest update of the library brings a number of changes to the namespace, some class names and method calls have been updated as Mobile Services is replaced by App Services. There is also a dependency on a shared [RESTClient for Unity](https://github.com/Unity3dAzure/RESTClient) so that multiple Unity libraries for Azure can be supported without adding duplicate scripts.

## External dependencies
**First download the shared [REST Client library for Unity](https://github.com/Unity3dAzure/RESTClient) and extract the contents into your Unity project "Assets" folder.**
* [RESTClient](https://github.com/Unity3dAzure/RESTClient)

## Requirements
Requires Unity v5.3 or greater as [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) and [JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) features are used. Unity will be extending platform support for UnityWebRequest so keep Unity up to date if you need to support these additional platforms.

## How to setup App Services with a new Unity project
1. [Download AppServices](https://github.com/Unity3dAzure/AppServices/archive/master.zip) and [REST Client](https://github.com/Unity3dAzure/RESTClient/archive/master.zip) for Unity.
	* Copy 'AppServices' and 'RESTClient' into project `Assets` folder.
2. Create an Azure App Service [Mobile App](https://portal.azure.com)
	* Create a Table (using Easy Tables) for app data.

## Azure App Services Demos for Unity 5
Try the [Azure App Services Demos](https://github.com/Unity3dAzure/AppServicesDemo) project for Unity v5.4.x on Mac / Windows. (The demo project has got everything already bundled in and does not require any additional assets to work. Just wire it up with your [Azure App Service](https://portal.azure.com) and run it right inside the Unity Editor.)
For detailed instructions read my developer blog on [how to setup Azure App Services and Unity demo project](http://www.deadlyfingers.net/azure/azure-app-services-for-unity3d/).

## Supported Features
### AppServiceClient API
API | Description
--- | -----------
LoginWithFacebook | Client-directed login using user access token.
LoginWithTwitter | Client-directed login using access token and access token secret.
LoginWithGoogle | Client-directed login using access token and id token.
LoginWithMicrosoftAccount | Client-directed login using access token.
LoginWithAAD | Client-directed login using access token.
Logout | Logout current user
InvokeApi | Invoke custom API (Easy API) using GET, POST, PUT, PATCH or DELETE

### AppServiceTable API
API | Description
--- | -----------
Insert | Create a new item.
Read | Get a list of items.
Update | Update an item’s data using id property.
Delete | Delete an item using id property.
Query | Get a list of results using a custom query.
Lookup | Get an item’s data using id property.

### [AppServiceClient Interface](https://github.com/Unity3dAzure/AppServices/blob/master/IAppServiceClient.cs)
	LoginWithFacebook(string accessToken, Action<IRestResponse<AuthenticatedUser>> callback = null);
	LoginWithTwitter(string accessToken, string accessTokenSecret,	Action<IRestResponse<AuthenticatedUser>> callback = null);
	LoginWithGoogle(string accessToken, string idToken,Action<IRestResponse<AuthenticatedUser>> callback = null);
	LoginWithMicrosoftAccount(string accessToken,Action<IRestResponse<AuthenticatedUser>> callback = null);
	LoginWithAAD(string accessToken, Action<IRestResponse<AuthenticatedUser>> callback = null);
	Logout(Action<IRestResponse<string>> callback = null);

### [AppServiceClient (Easy APIs) Interface](https://github.com/Unity3dAzure/AppServices/blob/master/IAppServiceClient.cs)
	InvokeApi<T> (string apiName, Action<IRestResponse<T>> callback = null) where T : new();
	InvokeApi<T> (string apiName, Method httpMethod, Action<IRestResponse<T>> callback = null) where T : new();
	InvokeApi<B,T> (string apiName, Method httpMethod, B body, Action<IRestResponse<T>> callback = null) where T : new();

### [AppServiceTable (Easy Tables) Interface](https://github.com/Unity3dAzure/AppServices/blob/master/table/IAppServiceTable.cs)
	Insert<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	Read<T>(Action<IRestResponse<T[]>> callback = null) where T : new();
	Update<T>(T item, Action<IRestResponse<T>> callback = null) where T : new();
	Delete<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();
	Query<T>(TableQuery query, Action<IRestResponse<T[]>> callback = null) where T : new();
	Query<T>(TableQuery query, Action<IRestResponse<T>> callback = null) where T : INestedResults, new();
	Lookup<T>(string id, Action<IRestResponse<T>> callback = null) where T : new();

## Sample usage
 - Data model for **TodoItem.cs**
```
using Azure.AppServices;
[System.Serializable]
public class TodoItem : DataModel {
  public string text;
  public bool complete;
}
```

 - Unity script
```
using System.Collections
using System.Collections.Generic;
using UnityEngine;
using System;
using RESTClient;
using Azure.AppServices;
```

```
private AppServiceClient _client;
private AppServiceTable<TodoItem> _table;
```

```
void Start () {
  _client = AppServiceClient.Create("unityapp"); // <- add your App Service account here!
  _table = _client.GetTable<TodoItem>("TodoItem");
  ReadItems();
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
    Debug.Log ( "Todo items count: " +     items.Length);
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
Intended to work on all the platforms [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) supports including:
* Unity Editor (Mac/PC) and Standalone players
* iOS
* Android
* Windows

Questions or tweet [@deadlyfingers](https://twitter.com/deadlyfingers)
