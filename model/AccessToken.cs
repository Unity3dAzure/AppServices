namespace Unity3dAzure.MobileServices
{
    public class AccessToken
    {
		public string access_token { get; set; }
		
		/// <summary>
		/// Facebook, Google, AAD access_token request
		/// </summary>
		public AccessToken(string accessTokenValue)
		{
			access_token = accessTokenValue;
		}
    }
}