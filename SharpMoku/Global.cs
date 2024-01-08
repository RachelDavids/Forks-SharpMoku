using System.Drawing;

using SharpMoku.UI.Theme;
using SharpMoku.Utility;

namespace SharpMoku
{
	public static class Global
	{
		private static SharpMokuSettings _currentSettings = null;
		public static SharpMokuSettings CurrentSettings {
			get {

				if (_currentSettings == null)
				{
					if (!System.IO.File.Exists(FileUtility.SettingPath))
					{
						SerializeUtility.Serialize(new SharpMokuSettings(), FileUtility.SettingPath);
					}

					_currentSettings = SerializeUtility.Deserialize<SharpMokuSettings>(FileUtility.SettingPath);

				}
				return _currentSettings;
			}
		}
		public static void SaveSettings()
		{

			SerializeUtility.Serialize(_currentSettings, FileUtility.SettingPath);

			_currentSettings = null;
		}

		public static Color BackColor => ThemeFactory.BackColor(CurrentSettings.ThemeEnum);
		public static Color ForeColor => ThemeFactory.ForeColor(CurrentSettings.ThemeEnum);

		public static void CreateNewSettings(string filename)
		{
			SerializeUtility.Serialize(new SharpMokuSettings(), filename);
		}

		public static void SerializeSettings(SharpMokuSettings setting, string filename)
		{
			SerializeUtility.Serialize(setting, filename);
		}

		public static SharpMokuSettings DeserializeSettings(string filename)
		{
			return SerializeUtility.Deserialize<SharpMokuSettings>(filename);
		}
	}
}
