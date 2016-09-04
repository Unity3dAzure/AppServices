using System;
using System.ComponentModel;
using Pathfinding.Serialization.JsonFx;

namespace Unity3dAzure.AppServices
{
	[Serializable]
	[CLSCompliant(false)]
	public class DataModel : IDataModel
    {
		public string id { get; set; }

		// system properties
		[JsonIgnore] public DateTime createdAt { get; private set; }
		[JsonIgnore] public DateTime updatedAt { get; private set; }
		[JsonIgnore] public string version { get; private set; }
		[JsonIgnore] public bool deleted { get; private set; }

		// `$inlinecount=allpages` property
		[EditorBrowsable(EditorBrowsableState.Never)]
		[JsonIgnore] public uint ROW_NUMBER { get; private set; }
		public uint GetIndex()
		{
			return ROW_NUMBER;
		}

        public string GetId()
        {
            return id;
        }

		public override string ToString ()
		{
			return string.Format ("id:{0}, createdAt:{1}, updatedAt:{2}, version:{3}, deleted:{4}, index:{5}", id, createdAt, updatedAt, version, deleted, GetIndex());
		}
    }
}