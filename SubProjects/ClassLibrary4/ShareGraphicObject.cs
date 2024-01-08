using System.Collections.Generic;
using System.Drawing;
namespace SharpMoku
{
	public static class ShareGraphicObject
	{
		// This class is a flyweight that contain all of the grahpic object

		private static string GetHexfromColor(Color color)
		{
			return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

		}

		private static Dictionary<Color, SolidBrush> solidBrushDictionary = null;
		private static Dictionary<string, Bitmap> bitmapFilePathDictionary = null;
		private static Font _GoMokuBoardFont = null;
		public static Font GoMokuBoardFont {
			get {
				if (_GoMokuBoardFont == null)
				{
					FontFamily fontFamily = new("Segoe UI");
					_GoMokuBoardFont = new Font(
					fontFamily,
					20,
					FontStyle.Bold,
					GraphicsUnit.Pixel);
				}
				return _GoMokuBoardFont;
			}
			set => _GoMokuBoardFont = value;
		}
		public static Bitmap BitmapFilePath(string filePath)
		{
			bitmapFilePathDictionary ??= [];
			if (!bitmapFilePathDictionary.ContainsKey(filePath))
			{
				Bitmap bitmap = new(filePath);
				bitmapFilePathDictionary.Add(filePath, bitmap);
			}
			return bitmapFilePathDictionary[filePath];
		}
		public static SolidBrush SolidBrush(Color color)
		{
			solidBrushDictionary ??= [];
			if (!solidBrushDictionary.ContainsKey(color))
			{
				solidBrushDictionary.Add(color, new SolidBrush(color));
			}

			return solidBrushDictionary[color];
		}

		private static Dictionary<string, Pen> penDictionary = null;
		public static Pen Pen(Color color, float width)
		{
			penDictionary ??= [];
			string key = GetHexfromColor(color) + "_" + width.ToString();
			if (!penDictionary.ContainsKey(key))
			{
				Pen pen = new(color, width);
				penDictionary.Add(key, pen);

			}
			return penDictionary[key];

		}
	}
}
