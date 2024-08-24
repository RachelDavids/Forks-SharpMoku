#nullable enable
using System;
using System.IO;

namespace SharpMoku.Utility
{
	public static class FileUtility
	{
		private static string? s_appInfoPath = null;
		private static string AppInfoPath {
			get {
				if (s_appInfoPath == null)
				{
					string exePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath;
					s_appInfoPath = Path.Combine(Path.GetDirectoryName(exePath)!, "AppInfo");
				}
				return s_appInfoPath;
			}
		}
		public static string SettingPath => Path.Combine(AppInfoPath, "Settings.json");
		public static string ResourcesPath => Path.Combine(AppInfoPath, "Resources");
		public static string BoardPath => Path.Combine(AppInfoPath, "Board");
		public static string LogFilePath => Path.Combine(AppInfoPath, "Log.txt");

		//public static bool IsFileExist(string fileName) => System.IO.File.Exists(fileName);

		//public static void CopyFileIfItIsDifferentPath(string original, string destination)
		//{
		//	bool needToCopy = !String.Equals(original.Trim().ToLower(),
		//									 destination.Trim().ToLower(),
		//									 StringComparison.OrdinalIgnoreCase);
		//	if (needToCopy)
		//	{
		//		System.IO.File.Copy(original, destination, true);
		//	}
		//}

		//public static Image ReadImageWithoutLockFile(string fileName)
		//{
		//	Image img;
		//	using (Bitmap bmpTemp = new(fileName))
		//	{
		//		img = new Bitmap(bmpTemp);
		//	}
		//	return img;
		//}
	}
}
