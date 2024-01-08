using System.Drawing;
using System.Drawing.Drawing2D;

using static SharpMoku.GomokuCellAttribute;

namespace SharpMoku.UI.LabelCustomPaint
{
	public class GoMokuPaintBuilder
	{
		private string whiteStoneImagePath = "";
		private string blackStoneImagePath = "";
		private Color whiteStoneBorderColor = Color.White;
		private Color blackStoneBorderColor = Color.Black;
		// private String boardBackgroundImagePath = "";
		// private Color boardBackColor = Color.Yellow;
		private Color whiteStoneBackColor = Color.White;
		private Color blackStoneBackColor = Color.Black;
		private Pen penTable = null;
		private Pen penBorder = null;
		public GoMokuPaintBuilder WhiteStoneImagePath(string path)
		{
			whiteStoneImagePath = path;
			return this;
		}
		public GoMokuPaintBuilder BlackStoneImagePath(string path)
		{
			blackStoneImagePath = path;
			return this;
		}
		/*
        public GoMokuPaintBuilder BoardBackgroundImagePath(String path)
        {
            this.boardBackgroundImagePath = path;
            return this;
        }
        */
		public GoMokuPaintBuilder WhiteStoneBorderColor(Color borderColor)
		{
			whiteStoneBorderColor = borderColor;
			return this;
		}

		public GoMokuPaintBuilder BlackStoneBorderColor(Color borderColor)
		{
			blackStoneBorderColor = borderColor;
			return this;
		}
		public GoMokuPaintBuilder WhiteStoneBackColor(Color backColor)
		{
			whiteStoneBackColor = backColor;
			return this;
		}

		public GoMokuPaintBuilder BlackStoneBackColor(Color backColor)
		{
			blackStoneBackColor = backColor;
			return this;
		}

		public GoMokuPaintBuilder PenTable(Pen pen)
		{
			penTable = pen;
			return this;
		}
		public GoMokuPaintBuilder PenBorder(Pen pen)
		{
			penBorder = pen;
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
				whiteStoneImagePath,
				blackStoneImagePath,
				whiteStoneBackColor,
				whiteStoneBorderColor,
				blackStoneBackColor,
				blackStoneBorderColor,
				penTable,
				penBorder);
			return gomokuPaint;
		}
	}
	public class GoMokuPaint : IExtendLabelCustomPaint
	{

		private string whiteStoneImagePath = "";
		private string blackStoneImagePath = "";
		// private String boardBackgroundImagePath = "";
		private Color whiteStoneBackColor = Color.White;
		private Color blackStoneBackColor = Color.Black;
		private Color whiteStoneBorderColor = Color.White;
		private Color blackStoneBorderColor = Color.Black;

		// private Color boardBackColor = Color.Yellow;
		private Pen penTable = new(Color.Black, 2f);
		private Pen penBorder = null;// new Pen(Color.Black, 4f);

		public GoMokuPaint(
			string whiteStoneImagePath,
			string blackStoneImagePath,
			Color whiteStoneBackColor,
			Color whiteStoneBorderColor,
			Color blackStoneBackColor,
			Color blackStoneBorderColor,
			Pen penTable,
			Pen penBorder)
		{
			//  this.boardBackgroundImagePath = boardBackgroundImagePath;
			this.whiteStoneImagePath = whiteStoneImagePath;
			this.blackStoneImagePath = blackStoneImagePath;
			//  this.boardBackColor = boardBackColor;
			this.whiteStoneBackColor = whiteStoneBackColor;
			this.blackStoneBackColor = blackStoneBackColor;
			this.whiteStoneBorderColor = whiteStoneBorderColor;
			this.blackStoneBorderColor = blackStoneBorderColor;
			if (penTable != null)
			{
				this.penTable = penTable;
			}
			if (penBorder != null)
			{
				this.penBorder = penBorder;
			}
		}

		private void SetLeftBorder(int beginWidth,
			int beginHeight,
			int endWidth,
			int endHeight,
			ref Point pfromPointBorderX,
			ref Point ptoPointBorderX,
			ref Point pfromPointBorderY,
			ref Point ptoPointBorderY)
		{
			pfromPointBorderX = new Point(beginWidth + 1, beginHeight);
			ptoPointBorderX = new Point(beginWidth + 1, beginHeight);

			pfromPointBorderY = new Point(beginWidth + 1, beginHeight);
			ptoPointBorderY = new Point(beginWidth + 1, endHeight);
		}

		private void SetRightBorder(int beginWidth,
	int beginHeight,
	int endWidth,
	int endHeight,
	ref Point pfromPointBorderX,
	ref Point ptoPointBorderX,
	ref Point pfromPointBorderY,
	ref Point ptoPointBorderY)
		{
			pfromPointBorderX = new Point(endWidth - 1, beginHeight);
			ptoPointBorderX = new Point(endWidth - 1, beginHeight);

			pfromPointBorderY = new Point(endWidth - 1, beginHeight);
			ptoPointBorderY = new Point(endWidth - 1, endHeight);
		}

		private void SetTopBorder(int beginWidth,
	int beginHeight,
	int endWidth,
	int endHeight,
	ref Point pfromPointBorderX,
	ref Point ptoPointBorderX,
	ref Point pfromPointBorderY,
	ref Point ptoPointBorderY)
		{
			int Space = 0;
			pfromPointBorderX = new Point(beginWidth + Space, beginHeight);
			ptoPointBorderX = new Point(endWidth - Space, beginHeight);

			pfromPointBorderY = new Point(beginWidth + Space, beginHeight);
			ptoPointBorderY = new Point(beginWidth + Space, beginHeight);
		}

		private void SetBottomBorder(int beginWidth,
	int beginHeight,
	int endWidth,
	int endHeight,
	ref Point pfromPointBorderX,
	ref Point ptoPointBorderX,
	ref Point pfromPointBorderY,
	ref Point ptoPointBorderY)
		{

			int Space = 0;
			pfromPointBorderX = new Point(beginWidth + Space, endHeight);
			ptoPointBorderX = new Point(endWidth - Space, endHeight);

			pfromPointBorderY = new Point(beginWidth + Space, endHeight);
			ptoPointBorderY = new Point(beginWidth + Space, endHeight);
		}
		private void PaintBorder(Graphics g, ExtendLabel pLabel)
		{
			Point fromPointY = Point.Empty;
			Point toPointY = Point.Empty;
			Point fromPointX = Point.Empty;
			Point toPointX = Point.Empty;

			Point fromPointBorderY = new(-1, -1);
			Point toPointBorderY = new(-1, -1);
			Point fromPointBorderX = new(-1, -1);
			Point toPointBorderX = new(-1, -1);

			Point fromPointBorderY2 = new(-1, -1);
			Point toPointBorderY2 = new(-1, -1);
			Point fromPointBorderX2 = new(-1, -1);
			Point toPointBorderX2 = new(-1, -1);

			int beginWidth = 0;
			int middleWidth = pLabel.Width / 2;
			int endWidth = pLabel.Width;
			int beginHeight = 0;
			int middleHeight = pLabel.Height / 2;
			int endHeight = pLabel.Height;

			switch (pLabel.CellAttribute.GoboardPosition)
			{
				case GoBoardPositionEnum.Center:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					break;
				case GoBoardPositionEnum.TopLeftCorner:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);

					SetLeftBorder(beginWidth, beginHeight, endWidth, endHeight,
						ref fromPointBorderX,
						ref toPointBorderX,
						ref fromPointBorderY,
						ref toPointBorderY);

					SetTopBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX2,
ref toPointBorderX2,
ref fromPointBorderY2,
ref toPointBorderY2);

					break;
				case GoBoardPositionEnum.TopRightCorner:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);

					SetRightBorder(beginWidth, beginHeight, endWidth, endHeight,
	ref fromPointBorderX,
	ref toPointBorderX,
	ref fromPointBorderY,
	ref toPointBorderY);

					SetTopBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX2,
ref toPointBorderX2,
ref fromPointBorderY2,
ref toPointBorderY2);
					break;
				case GoBoardPositionEnum.BottomLeftCorner:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, middleHeight);

					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);

					SetLeftBorder(beginWidth, beginHeight, endWidth, endHeight,
	ref fromPointBorderX,
	ref toPointBorderX,
	ref fromPointBorderY,
	ref toPointBorderY);

					SetBottomBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX2,
ref toPointBorderX2,
ref fromPointBorderY2,
ref toPointBorderY2);
					break;
				case GoBoardPositionEnum.BottomRightCorner:

					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, middleHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);

					SetRightBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX,
ref toPointBorderX,
ref fromPointBorderY,
ref toPointBorderY);
					SetBottomBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX2,
ref toPointBorderX2,
ref fromPointBorderY2,
ref toPointBorderY2);
					break;
				case GoBoardPositionEnum.TopBorder:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);

					SetTopBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX,
ref toPointBorderX,
ref fromPointBorderY,
ref toPointBorderY);
					break;
				case GoBoardPositionEnum.BottomBorder:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, beginHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);

					SetBottomBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX,
ref toPointBorderX,
ref fromPointBorderY,
ref toPointBorderY);
					break;
				case GoBoardPositionEnum.LeftBorder:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);

					SetLeftBorder(beginWidth, beginHeight, endWidth, endHeight,
						ref fromPointBorderX,
						ref toPointBorderX,
						ref fromPointBorderY,
						ref toPointBorderY);

					break;
				case GoBoardPositionEnum.RightBorder:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);

					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);

					SetRightBorder(beginWidth, beginHeight, endWidth, endHeight,
ref fromPointBorderX,
ref toPointBorderX,
ref fromPointBorderY,
ref toPointBorderY);

					break;
			}

			g.DrawLine(penTable, fromPointX, toPointX);
			g.DrawLine(penTable, fromPointY, toPointY);
			if (penBorder != null)
			{
				if (fromPointBorderX.X != -1)
				{
					g.DrawLine(penBorder, fromPointBorderX, toPointBorderX);
					g.DrawLine(penBorder, fromPointBorderY, toPointBorderY);

				}
				if (fromPointBorderX2.X != -1)
				{
					g.DrawLine(penBorder, fromPointBorderX2, toPointBorderX2);
					g.DrawLine(penBorder, fromPointBorderY2, toPointBorderY2);

				}
			}

			g.CompositingMode = CompositingMode.SourceOver;

			if (pLabel.CellAttribute.IsIntersection)
			{
				RectangleF RecCircleIntersecton = new(middleWidth - 4, middleHeight - 4, 8, 8);
				g.FillEllipse(ShareGraphicObject.SolidBrush(penTable.Color), RecCircleIntersecton);
			}
		}
		private RectangleF GetRectangleCircle(int labelWidth, int labelHeight)
		{
			int Space = 1;

			return new RectangleF(Space, Space, labelWidth - (Space * 2), labelHeight - (Space * 2));

		}
		public void PaintStone(Graphics g, ExtendLabel pLabel)
		{
			RectangleF RecCircle = GetRectangleCircle(pLabel.Width, pLabel.Height);
			RectangleF RecCircleImage = new(RecCircle.X,
	RecCircle.Y,
	RecCircle.Width - 0.5f,
	RecCircle.Height - 0.5f);

			if (pLabel.CellAttribute.CellValue == CellValue.White)
			{

				if (whiteStoneImagePath.Trim() != "")
				{

					g.DrawImage(ShareGraphicObject.BitmapFilePath(whiteStoneImagePath), RecCircleImage);
				}
				else
				{
					g.FillEllipse(ShareGraphicObject.SolidBrush(whiteStoneBackColor), RecCircle);
					g.DrawEllipse(ShareGraphicObject.Pen(whiteStoneBorderColor, 0.2f), RecCircle);

				}
			}
			if (pLabel.CellAttribute.CellValue == CellValue.Black)
			{

				if (blackStoneImagePath.Trim() != "")
				{
					g.DrawImage(ShareGraphicObject.BitmapFilePath(blackStoneImagePath), RecCircleImage);
				}
				else
				{

					g.FillEllipse(ShareGraphicObject.SolidBrush(blackStoneBackColor), RecCircle);
					g.DrawEllipse(ShareGraphicObject.Pen(blackStoneBorderColor, 0.2f), RecCircle);

				}
			}
		}
		public void PaintNeighbour(Graphics g, ExtendLabel pLabel)
		{

			RectangleF RecCircle = GetRectangleCircle(pLabel.Width, pLabel.Height);
			if (pLabel.CellAttribute.IsNeighborCell)
			{
				g.FillEllipse(ShareGraphicObject.SolidBrush(Color.Gray), RecCircle);

			}
		}
		public void Paint(Graphics g, ExtendLabel pLabel)
		{

			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.CompositingMode = CompositingMode.SourceCopy;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			PaintBorder(g, pLabel);
			PaintStone(g, pLabel);

			//We Actually don't use this for actual gaming
			//We paint NeighborCell to shows its position for explaining the postion for Tutorial and for debugging purpose only
			//Uncomment this code if you need to see Neighbor cell
			//[DEBUG:]
			//PaintNeighbour(g, pLabel);

		}
	}
}
