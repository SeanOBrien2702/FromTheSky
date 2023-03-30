using System.IO;
using UnityEngine;

namespace EasyFeedback.Editor.Build
{
	internal static class PackageFactory
	{
		public static Package LoadFrom(string path)
		{
			var json = File.ReadAllText(path);
			return JsonUtility.FromJson<Package>(json);
		}
	}
}