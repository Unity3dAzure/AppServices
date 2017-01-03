using System;
using System.Text;

namespace Unity3dAzure.AppServices
{
	public class DataHelper
	{

		public static byte[] ToUTF8Bytes (string utf8String)
		{
			return Encoding.UTF8.GetBytes (utf8String);
		}

		public static string ToUTF8String (byte[] bytes)
		{
			return Encoding.UTF8.GetString (bytes);
		}

	}
}