using UnityEngine;
using System.Collections;
using RestSharp;
#if !NETFX_CORE
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
#endif

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
            // required for running in Windows
            #if !NETFX_CORE
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            #endif
        }

        public void AddBodyAccessToken(string token)
        {
            AccessToken accessToken = new AccessToken(token);
            this.AddBody(accessToken);
        }

        #if !NETFX_CORE
        private bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //   Check the certificate to see if it was issued from Azure
            if ( certificate.Subject.Contains("azurewebsites.net") )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endif

    }
}
