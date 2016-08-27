namespace Unity3dAzure.AppServices
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

        /// Needed only for Serialization (Fixes error: AccessToken cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute)
        public AccessToken() {}
    }
}