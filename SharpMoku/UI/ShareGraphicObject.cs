using System.Collections.Generic;
using System.Drawing;

namespace SharpMoku.UI
{
	public static class ShareGraphicObject
	{
		// This class is a flyweight that contain all of the grahpic object

		private static string HexFromColor(Color color)
		{
			return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

		}

		private static Dictionary<Color, SolidBrush> Brushes = null;
		private static Dictionary<string, Bitmap> Bitmaps = null;
		private static Font _GoMokuBoardFont = null;
		public static Font GoMokuBoardFont {
			get {
				if (_GoMokuBoardFont == null)
				{
					FontFamily fontFamily = new("Segoe UI");
					_GoMokuBoardFont = new Font(fontFamily,
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
			Bitmaps ??= [];
			if (!Bitmaps.ContainsKey(filePath))
			{
				Bitmap bitmap = new(filePath);
				Bitmaps.Add(filePath, bitmap);
			}
			return Bitmaps[filePath];
		}
		public static SolidBrush SolidBrush(Color color)
		{
			Brushes ??= [];
			if (!Brushes.ContainsKey(color))
			{
				Brushes.Add(color, new SolidBrush(color));
			}

			return Brushes[color];
		}

		private static Dictionary<string, Pen> Pens = null;
		public static Pen Pen(Color color, float width)
		{
			Pens ??= [];
			string key = HexFromColor(color) + "_" + width;
			if (!Pens.ContainsKey(key))
			{
				Pen pen = new(color, width);
				Pens.Add(key, pen);

			}
			return Pens[key];

		}
	}
}
