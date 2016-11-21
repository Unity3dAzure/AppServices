#pragma warning disable 0649 // ignores warning: array "is never assigned to, and will always have its default value 'null'" 
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace Unity3dAzure.AppServices
{
	public class JsonHelper {
		/// <summary>
		/// Workaround for json array described on https://forum.unity3d.com/threads/how-to-load-an-array-with-jsonutility.375735/
		/// </summary>
		public static T[] GetJsonArray<T>(string json)
		{
			string newJson = "{\"array\":" + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
			return wrapper.array;
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T[] array;
		}

		/// <summary>
		/// Workaround to only exclude Data Model's read only system properties being returned as json object. Unfortunately there is no JsonUtil attribute to do this as [NonSerialized] will just ignore the properties completely (both in and out).
		/// </summary>
		public static string ToJsonExcludingSystemProperties(object obj)
		{
			string jsonString = JsonUtility.ToJson(obj);
			return Regex.Replace(jsonString, "(?i)(\\\"id\\\":\\\"\\\",)?(\\\"createdAt\\\":\\\"[0-9TZ:.-]*\\\",)?(\\\"updatedAt\\\":\\\"[0-9TZ:.-]*\\\",)?(\\\"version\\\":\\\"[A-Z0-9=]*\\\",)?(\\\"deleted\\\":(true|false),)?(\\\"ROW_NUMBER\\\":\\\"[0-9]*\\\",)?", "");
		}
	}

}