using System;

namespace Unity3dAzure.AppServices
{
	[Serializable]
	public class MobileServiceUser
	{
		public string authenticationToken;
		public User user;
	}
}