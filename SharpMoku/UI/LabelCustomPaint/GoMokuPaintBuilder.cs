using System.Drawing;

namespace SharpMoku.UI.LabelCustomPaint
{
	public class GoMokuPaintBuilder
	{
		private string _whiteStoneImagePath = "";
		private string _blackStoneImagePath = "";
		private Color _whiteStoneBorderColor = Color.White;
		private Color _blackStoneBorderColor = Color.Black;
		// private String boardBackgroundImagePath = "";
		// private Color boardBackColor = Color.Yellow;
		private Color _whiteStoneBackColor = Color.White;
		private Color _blackStoneBackColor = Color.Black;
		private Pen _penTable = null;
		private Pen _penBorder = null;
		public GoMokuPaintBuilder WhiteStoneImagePath(string path)
		{
			_whiteStoneImagePath = path;
			return this;
		}
		public GoMokuPaintBuilder BlackStoneImagePath(string path)
		{
			_blackStoneImagePath = path;
			return this;
		}
		public GoMokuPaintBuilder WhiteStoneBorderColor(Color borderColor)
		{
			_whiteStoneBorderColor = borderColor;
			return this;
		}
		public GoMokuPaintBuilder BlackStoneBorderColor(Color borderColor)
		{
			_blackStoneBorderColor = borderColor;
			return this;
		}
		public GoMokuPaintBuilder WhiteStoneBackColor(Color backColor)
		{
			_whiteStoneBackColor = backColor;
			return this;
		}
		public GoMokuPaintBuilder BlackStoneBackColor(Color backColor)
		{
			_blackStoneBackColor = backColor;
			return this;
		}
		public GoMokuPaintBuilder PenTable(Pen pen)
		{
			_penTable = pen;
			return this;
		}
		public GoMokuPaintBuilder PenBorder(Pen pen)
		{
			_penBorder = pen;
			return this;
		}
		/*
        public GoMokuPaintBuilder BoardBackColor(Color backColor)
        {
            this.boardBackColor = backColor;
            return this;
        }
        */
		public GoMokuPaint Build()
		{
			/*
            if(!IsValidConfigured)
            {
                throw new Exception ("The configured value is not valid ")
            }
            */
			GoMokuPaint gomokuPaint = new(
				_whiteStoneImagePath,
				_blackStoneImagePath,
				_whiteStoneBackColor,
				_whiteStoneBorderColor,
				_blackStoneBackColor,
				_blackStoneBorderColor,
				_penTable,
				_penBorder);
			return gomokuPaint;
		}
	}
}
