using System;
using System.Drawing;
using System.IO;

namespace SharpMoku.Utility
{
	public static class FileUtility
	{
		private static string AppInfoPath {
			get {
				string exePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
				return Path.GetDirectoryName(exePath) + @"\AppInfo";
			}
		}
		public static string SettingPath => $@"{AppInfoPath}\Setting.bin";
		public static string ResourcesPath => $@"{AppInfoPath}\Resources";
		public static string BoardPath => $@"{AppInfoPath}\Board\";
		public static string LogFilePath => $@"{AppInfoPath}\Log.txt";

		public static bool IsFileExist(string fileName) => System.IO.File.Exists(fileName);

		public static void CopyFileIfItIsDifferentPath(string original, string destination)
		{
			bool needToCopy = !String.Equals(original.Trim().ToLower(),
											 destination.Trim().ToLower(),
											 StringComparison.OrdinalIgnoreCase);
			if (needToCopy)
			{
				System.IO.File.Copy(original, destination, true);
			}
		}

		public static Image ReadImageWithoutLockFile(string fileName)
		{
			Image img;
			using (Bitmap bmpTemp = new(fileName))
			{
				img = new Bitmap(bmpTemp);
			}
			return img;
		}
	}
}
