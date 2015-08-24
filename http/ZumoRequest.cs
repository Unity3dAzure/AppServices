using UnityEngine;
using System.Collections;
using RestSharp;

namespace Unity3dAzure.MobileServices
{
    public class ZumoRequest : RestRequest
    {
        public ZumoRequest(MobileServiceClient client, string uri, Method httpMethod) : base(uri, httpMethod)
        {
            this.AddHeader("X-ZUMO-APPLICATION", client.AppKey);
            //this.AddHeader("Content-Type", "application/json; charset=UTF-8"); // this line caused winrt app to not connect
            this.AddHeader("Accept", "application/json");
            this.RequestFormat = DataFormat.Json;
            if (client.User != null && !string.IsNullOrEmpty(client.User.authenticationToken))
            {
                this.AddHeader("X-ZUMO-AUTH", client.User.authenticationToken);
                Debug.Log("Auth UserId:" + client.User.user.userId);
            }
        }

        public void AddBodyAccessToken(string token)
        {
            AccessToken accessToken = new AccessToken(token);
            this.AddBody(accessToken);
        }

    }
}
