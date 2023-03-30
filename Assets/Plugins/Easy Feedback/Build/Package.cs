namespace EasyFeedback.Editor.Build
{
	internal class Package
	{
		// see: https://docs.unity3d.com/Manual/upm-manifestPkg.html
		public string name = null;
		public string version = null;

		public string displayName = null;
		public string description = null;
		public string unity = null;

		public Package(Package source)
		{
			name = source.name;
			version = source.version;
			displayName = source.displayName;
			description = source.description;
			unity = source.unity;
		}
	}
}