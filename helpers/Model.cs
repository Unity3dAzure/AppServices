using System.Reflection;

namespace Unity3dAzure.AppServices
{
	/// <summary>
	/// Helper methods to check and get object properties
	/// </summary>
	public class Model {
		public static bool HasProperty(object obj, string propertyName)
		{
			return GetProperty(obj, propertyName) != null;
		}

		public static PropertyInfo GetProperty(object obj, string propertyName)
		{
			#if NETFX_CORE 
			return obj.GetType().GetTypeInfo().GetDeclaredProperty(propertyName); // workaround for GetProperty on Windows
			#else
			return obj.GetType().GetProperty(propertyName);
			#endif
		}
	}
}