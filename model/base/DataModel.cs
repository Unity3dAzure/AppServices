using System;
using UnityEngine;

namespace Unity3dAzure.AppServices
{
	[Serializable]
	public class DataModel : IDataModel
	{
		[SerializeField] 
		public string id;

		public string GetId ()
		{
			return id;
		}

		public void SetId (string id)
		{
			this.id = id;
		}

		// System Properties (read only)
		[SerializeField] 
		private string createdAt;
		private DateTime? _createdAt;

		public DateTime? CreatedAt ()
		{
			if (_createdAt == null) {
				_createdAt = Convert.ToDateTime (createdAt);
			}
			return _createdAt;
		}

		[SerializeField] 
		private string updatedAt;
		private DateTime? _updatedAt;

		public DateTime? UpdatedAt ()
		{
			if (_updatedAt == null) {
				_updatedAt = Convert.ToDateTime (updatedAt);
			}
			return _updatedAt;
		}

		[SerializeField] 
		private string version;

		public string Version ()
		{
			return version;
		}

		[SerializeField] 
		private bool deleted;

		public bool Deleted ()
		{
			return deleted;
		}

		[SerializeField]
		private string ROW_NUMBER;

		public uint RowNumber ()
		{
			return Convert.ToUInt32 (ROW_NUMBER, 10);
		}

		public override string ToString ()
		{
			return JsonUtility.ToJson (this);
		}

		public string ToJSON ()
		{
			return JsonHelper.ToJsonExcludingSystemProperties (this);
		}
	}
}