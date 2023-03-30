using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EasyFeedback.Editor.Build
{
	internal static class Util
	{
		public static string ProjectPath
		{
			get
			{
				// Application.dataPath returns ProjectPath/Assets, so we go up 1 dir with ../
				return Path.GetFullPath(
					Path.Combine(Application.dataPath, "../")
				);
			}
		}

		public static string GetProjectRelativePath(string path)
		{
			return Path.Combine(ProjectPath, path);
		}

		public static string GetEFRelativePath(string path)
		{
			const string efPath = "Assets/Plugins/Easy Feedback/";
			return Path.Combine(efPath, path);
		}

		public static void CopyProjectTo(
			string destination, 
			Func<string, bool>[] include, 
			Func<string, bool>[] exclude,
			bool dryRun = false
		)
		{
			// get files to copy to output project
			var files = Directory.GetFiles(ProjectPath, "*.*", SearchOption.AllDirectories)
				// includes
				.Where(f =>
				{
					for (int i = 0; i < include.Length; i++)
					{
						if (include[i](f)) return true;
					}
					return false;
				})
				// excludes
				.Where(f =>
				{
					for (int i = 0; i < exclude.Length; i++)
					{
						if (exclude[i](f)) return false;
					}
					return true;
				});


			// copy files to output directory
			foreach (var file in files)
			{
				var dest = Path.Combine(
					destination, file.Replace(ProjectPath, "")
				);

				if (dryRun) // only log path if dry run
				{
					Debug.Log("[Builder] cp" + file + " => " + dest);
					continue;
				}

				// create output directory if it doesn't already exist
				Directory.CreateDirectory(Path.GetDirectoryName(dest));

				// copy file
				File.Copy(file, dest, true);
			}
		}
	}
}