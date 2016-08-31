using UnityEngine;
using System.Collections;
using RestSharp;
using System;

namespace Unity3dAzure.AppServices
{
	[CLSCompliant(false)]
    public class ZumoRequest : RestRequest
    {
        public ZumoRequest(MobileServiceClient client, string uri, Method httpMethod) : base(uri, httpMethod)
        {
            if (client.IsAppService())
            {
              // App Service headers
              this.AddHeader("ZUMO-API-VERSION", "2.0.0");
            } else {
              // Mobile Service headers
              this.AddHeader("X-ZUMO-APPLICATION", client.AppKey);
            }

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
