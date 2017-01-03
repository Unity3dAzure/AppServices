using System;

namespace Unity3dAzure.AppServices
{
	[Serializable]
	public class AccessToken
	{
		public string access_token;

		/// <summary>
		/// Facebook, Google, AAD access_token request
		/// </summary>
		public AccessToken (string accessTokenValue)
		{
			access_token = accessTokenValue;
		}

		// Must have a public parameterless contructor in order to use it as paramter 'T' in the generic type or method RestRequest.AddBody<T>
		//public AccessToken() {}
	}
}