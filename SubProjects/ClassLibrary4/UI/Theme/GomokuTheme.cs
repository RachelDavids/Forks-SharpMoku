using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI.Theme
{
	public class GomokuThemeBuilder
	{
		private string _boardImageFile = "";
		private string _whiteStoneImagePath = "";
		private string _blackStoneImagePath = "";
		private Color _boardBackColor = Color.Yellow;
		private Color _whiteStoneBorderColor = Color.White;
		private Color _blackStoneBorderColor = Color.Black;
		private Color _whiteStoneBackColor = Color.White;
		private Color _blackStoneBackColor = Color.Black;
		private Color _notationForeColor = Color.Black;
		private Pen _penTable = null;
		private Pen _penBorder = null;

		public GomokuThemeBuilder WhiteStoneImagePath(string path)
		{
			_whiteStoneImagePath = path;
			return this;
		}
		public GomokuThemeBuilder BlackStoneImagePath(string path)
		{
			_blackStoneImagePath = path;
			return this;
		}

		public GomokuThemeBuilder WhiteStoneBorderColor(Color borderColor)
		{
			_whiteStoneBorderColor = borderColor;
			return this;
		}
		public GomokuThemeBuilder WhiteStoneBackColor(Color backColor)
		{
			_whiteStoneBackColor = backColor;
			return this;
		}
		public GomokuThemeBuilder BlackStoneBorderColor(Color borderColor)
		{
			_blackStoneBorderColor = borderColor;
			return this;
		}
		public GomokuThemeBuilder BlackStoneBackColor(Color backColor)
		{
			_blackStoneBackColor = backColor;
			return this;
		}

		public GomokuThemeBuilder BoardBackColor(Color backColor)
		{
			_boardBackColor = backColor;
			return this;
		}

		public GomokuThemeBuilder NotationForeColor(Color foreColor)
		{
			_notationForeColor = foreColor;
			return this;
		}
		public GomokuThemeBuilder PenTable(Pen pen)
		{
			_penTable = pen;
			return this;
		}
		public GomokuThemeBuilder PenBorder(Pen pen)
		{
			_penBorder = pen;
			return this;
		}
		public GomokuTheme Build()
		{

			GomokuTheme theme = new(_boardImageFile,
									_whiteStoneImagePath,
									_blackStoneImagePath,
									_boardBackColor,
									_whiteStoneBackColor,
									_whiteStoneBorderColor,
									_blackStoneBackColor,
									_blackStoneBorderColor,
									_notationForeColor,
									_penTable,
									_penBorder);
			return theme;
		}
		public GomokuThemeBuilder BoardImageFile(string path)
		{

			_boardImageFile = path;
			return this;
		}
	}
	public class GomokuTheme
		: Theme
	{
		public GomokuTheme(string boardImageFile,
						   string whiteStoneImagePath,
						   string blackStoneImagePath,
						   Color boardBackcolor,
						   Color whiteStoneBackColor,
						   Color whiteStoneBorderColor,
						   Color blackStoneBackColor,
						   Color blackStoneBorderColor,
						   Color notationForeColor,
						   Pen penTable,
						   Pen penBorder)
		{
			SpaceBetweenBorderSize = 0;
			CellBorderStyle = BorderStyle.None;
			CellBackColor = Color.Transparent;
			CellCornerRadius = 0;

			CustomPaint = new GoMokuPaint(whiteStoneImagePath,
										  blackStoneImagePath,
										  whiteStoneBackColor,
										  whiteStoneBorderColor,
										  blackStoneBackColor,
										  blackStoneBorderColor,
										  penTable,
										  penBorder);

			NotationForeColor = notationForeColor;

			BoardImageFile = boardImageFile;
			BoardColor = boardBackcolor;

		}
	}
}
