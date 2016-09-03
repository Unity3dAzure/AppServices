using System;
using System.Collections.Generic;

namespace Unity3dAzure.AppServices
{
	/// <summary>
	/// Wrap your data model with this object to call the table Query with `$inlinecount=allpages` param.
	/// </summary>
	[CLSCompliant(false)]
	[Serializable]
	public class NestedResults<T> : INestedResults
	{
		public uint count { get; set; }
		public List<T> results { get; set; }
	}
}