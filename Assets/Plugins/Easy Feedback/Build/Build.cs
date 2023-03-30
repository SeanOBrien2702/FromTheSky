using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyFeedback.Editor.Build
{
	internal static class Build
	{
		[MenuItem("Tools/Build AssetStore Project")]
		public static void StartBuild()
		{
			// load package.json
			var package = PackageFactory.LoadFrom(
				Util.GetEFRelativePath("package.json")
			);

			Log(package.name + ": v" + package.version);

			// TODO: update version with conventional-commit semver -ntr
			// TODO: set version text in prefabs -ntr

			var outputPath = CreateOutputProject(package);

			BuildArtifacts(outputPath, package);

			EditorUtility.RevealInFinder(Path.Combine(outputPath, "Assets"));
		}

		private static string CreateOutputProject(Package package)
		{
			// copy to output project
			var outputPath = Util.GetProjectRelativePath(
				Path.Combine("Build", package.name + "_" + package.version)
			);

			Util.CopyProjectTo(
				destination: outputPath,
				include: new Func<string, bool>[]
				{
					f => f.StartsWith(Util.GetProjectRelativePath("Assets")),
					f => f.StartsWith(Util.GetProjectRelativePath("Packages")),
					f => f.StartsWith(Util.GetProjectRelativePath("ProjectSettings"))
				},
				exclude: new Func<string, bool>[]
				{
					f => f.Contains("\\docfx_project~\\"),
					f => f.Contains("\\node_modules\\"),
					f => f.Contains("\\Build\\"),
					f => f.EndsWith(".gitignore"),
					f => f.EndsWith(".gitmodules"),
					f => f.EndsWith("README.md"),
					f => f.EndsWith("package.json"),
					f => f.EndsWith("package-lock.json")
				}
			);

			return outputPath;
		}

		private static void BuildArtifacts(string outputPath, Package package)
		{
			BuildManifest(outputPath, package);
		}

		private static string BuildManifest(string outputPath, Package package)
		{
			// setup output manifest
			var manifest = new Package(package);
			PopulateManifest(manifest);

			// write manifest
			var json = JsonUtility.ToJson(manifest, true);
			var path = Path.Combine(
				outputPath, 
				Util.GetEFRelativePath("package.json")
			);
			File.WriteAllText(path, json);

			return path;
		}

		private static void PopulateManifest(Package package)
		{
			package.name = "com.aesthetician.EasyFeedback";
			package.displayName = "Easy Feedback";
			package.description = "Easy Feedback streamlines bug reporting and player feedback in your Unity game by bringing it directly to you!\n\nDetailed feedback and bug reports are submitted from in-game with just the tap of a button. With Trello, these reports are easily accessible and organized to your liking! Easy Feedback is very easy to setup and deploy, saving you development time, and getting your players' feedback to you as soon as possible.";
			package.unity = "2019.4";
		}

		private static void Log(string message)
		{
			Debug.Log("[Builder] " + message);
		}
	}
}