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

		private static Dictionary<Color, SolidBrush> s_brushes;
		private static Dictionary<string, Bitmap> s_bitmaps;
		private static Font s_goMokuBoardFont;
		public static Font GoMokuBoardFont {
			get {
				if (s_goMokuBoardFont == null)
				{
					FontFamily fontFamily = new("Segoe UI");
					s_goMokuBoardFont = new(fontFamily,
											20,
											FontStyle.Bold,
											GraphicsUnit.Pixel);
				}
				return s_goMokuBoardFont;
			}
			set => s_goMokuBoardFont = value;
		}
		public static Bitmap BitmapFilePath(string filePath)
		{
			s_bitmaps ??= [];
			if (!s_bitmaps.TryGetValue(filePath, out Bitmap value))
			{
				Bitmap bitmap = new(filePath);
				value = bitmap;
				s_bitmaps.Add(filePath, value);
			}
			return value;
		}
		public static SolidBrush SolidBrush(Color color)
		{
			s_brushes ??= [];
			if (!s_brushes.TryGetValue(color, out SolidBrush value))
			{
				value = new(color);
				s_brushes.Add(color, value);
			}

			return value;
		}

		private static Dictionary<string, Pen> s_pens;
		public static Pen Pen(Color color, float width)
		{
			s_pens ??= [];
			string key = HexFromColor(color) + "_" + width;
			if (!s_pens.TryGetValue(key, out Pen value))
			{
				Pen pen = new(color, width);
				value = pen;
				s_pens.Add(key, value);
			}
			return value;

		}
	}
}
