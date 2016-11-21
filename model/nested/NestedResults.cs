using System;

namespace Unity3dAzure.AppServices
{
	/// <summary>
	/// Wrap your data model with this object to call the table Query with `$inlinecount=allpages` param.
	/// </summary>
	[Serializable]
	public class NestedResults<T> : INestedResults
	{
		public uint count;
		public T[] results;
	}
}