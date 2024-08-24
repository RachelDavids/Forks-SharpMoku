using System.Drawing;
using System.IO;

using SharpMoku.UI.Theme;
using SharpMoku.Utility;
namespace SharpMoku
{
	public static class Common
	{
		private static SharpMokuSettings s_currentSettings;
		public static SharpMokuSettings CurrentSettings {
			get {
				if (s_currentSettings == null)
				{
					if (!File.Exists(FileUtility.SettingPath))
					{
						CreateNewSettings(FileUtility.SettingPath);
					}
					s_currentSettings = SerializeUtility.Deserialize<SharpMokuSettings>(FileUtility.SettingPath);
				}
				return s_currentSettings;
			}
		}
		public static void SaveSettings()
		{
			SerializeUtility.Serialize(s_currentSettings, FileUtility.SettingPath);
			s_currentSettings = null;
		}
		public static Color BackColor => ThemeFactory.BackColor(CurrentSettings.KnownTheme);
		public static Color ForeColor => ThemeFactory.ForeColor(CurrentSettings.KnownTheme);
		public static void CreateNewSettings(string filename)
		{
			SerializeUtility.Serialize(new SharpMokuSettings(), filename);
		}
		//public static void SerializeSettings(SharpMokuSettings setting, string filename)
		//{
		//	SerializeUtility.Serialize(setting, filename);
		//}
		//public static SharpMokuSettings DeserializeSettings(string filename)
		//{
		//	return SerializeUtility.Deserialize<SharpMokuSettings>(filename);
		//}
	}
}
