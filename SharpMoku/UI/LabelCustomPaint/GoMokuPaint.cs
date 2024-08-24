using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpMoku.UI.LabelCustomPaint
{
	public class GoMokuPaint
		: IExtendLabelCustomPaint
	{
		private readonly string _whiteStoneImagePath = "";
		private readonly string _blackStoneImagePath = "";
		// private String boardBackgroundImagePath = "";
		private readonly Color _whiteStoneBackColor = Color.White;
		private readonly Color _blackStoneBackColor = Color.Black;
		private readonly Color _whiteStoneBorderColor = Color.White;
		private readonly Color _blackStoneBorderColor = Color.Black;
		// private Color boardBackColor = Color.Yellow;
		private readonly Pen _penTable = new(Color.Black, 2f);
		private readonly Pen _penBorder = null;// new Pen(Color.Black, 4f);
		public GoMokuPaint(string whiteStoneImagePath,
						   string blackStoneImagePath,
						   Color whiteStoneBackColor,
						   Color whiteStoneBorderColor,
						   Color blackStoneBackColor,
						   Color blackStoneBorderColor,
						   Pen penTable,
						   Pen penBorder)
		{
			//  this.boardBackgroundImagePath = boardBackgroundImagePath;
			_whiteStoneImagePath = whiteStoneImagePath;
			_blackStoneImagePath = blackStoneImagePath;
			//  this.boardBackColor = boardBackColor;
			_whiteStoneBackColor = whiteStoneBackColor;
			_blackStoneBackColor = blackStoneBackColor;
			_whiteStoneBorderColor = whiteStoneBorderColor;
			_blackStoneBorderColor = blackStoneBorderColor;
			if (penTable != null)
			{
				_penTable = penTable;
			}
			if (penBorder != null)
			{
				_penBorder = penBorder;
			}
		}
#pragma warning disable IDE0060 // Remove unused parameter
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
			int space = 0;
			pfromPointBorderX = new Point(beginWidth + space, beginHeight);
			ptoPointBorderX = new Point(endWidth - space, beginHeight);
			pfromPointBorderY = new Point(beginWidth + space, beginHeight);
			ptoPointBorderY = new Point(beginWidth + space, beginHeight);
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
			int space = 0;
			pfromPointBorderX = new Point(beginWidth + space, endHeight);
			ptoPointBorderX = new Point(endWidth - space, endHeight);
			pfromPointBorderY = new Point(beginWidth + space, endHeight);
			ptoPointBorderY = new Point(beginWidth + space, endHeight);
		}
#pragma warning restore IDE0060 // Remove unused parameter

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
			switch (pLabel.CellDetail.GoBoardPosition)
			{
				case GoBoardPosition.Center:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					break;
				case GoBoardPosition.TopLeftCorner:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					SetLeftBorder(beginWidth,
								  beginHeight,
								  endWidth,
								  endHeight,
								  ref fromPointBorderX,
								  ref toPointBorderX,
								  ref fromPointBorderY,
								  ref toPointBorderY);
					SetTopBorder(beginWidth,
								 beginHeight,
								 endWidth,
								 endHeight,
								 ref fromPointBorderX2,
								 ref toPointBorderX2,
								 ref fromPointBorderY2,
								 ref toPointBorderY2);
					break;
				case GoBoardPosition.TopRightCorner:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);
					SetRightBorder(beginWidth,
								   beginHeight,
								   endWidth,
								   endHeight,
								   ref fromPointBorderX,
								   ref toPointBorderX,
								   ref fromPointBorderY,
								   ref toPointBorderY);
					SetTopBorder(beginWidth,
								 beginHeight,
								 endWidth,
								 endHeight,
								 ref fromPointBorderX2,
								 ref toPointBorderX2,
								 ref fromPointBorderY2,
								 ref toPointBorderY2);
					break;
				case GoBoardPosition.BottomLeftCorner:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, middleHeight);
					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					SetLeftBorder(beginWidth,
								  beginHeight,
								  endWidth,
								  endHeight,
								  ref fromPointBorderX,
								  ref toPointBorderX,
								  ref fromPointBorderY,
								  ref toPointBorderY);
					SetBottomBorder(beginWidth,
									beginHeight,
									endWidth,
									endHeight,
									ref fromPointBorderX2,
									ref toPointBorderX2,
									ref fromPointBorderY2,
									ref toPointBorderY2);
					break;
				case GoBoardPosition.BottomRightCorner:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, middleHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);
					SetRightBorder(beginWidth,
								   beginHeight,
								   endWidth,
								   endHeight,
								   ref fromPointBorderX,
								   ref toPointBorderX,
								   ref fromPointBorderY,
								   ref toPointBorderY);
					SetBottomBorder(beginWidth,
									beginHeight,
									endWidth,
									endHeight,
									ref fromPointBorderX2,
									ref toPointBorderX2,
									ref fromPointBorderY2,
									ref toPointBorderY2);
					break;
				case GoBoardPosition.TopBorder:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					SetTopBorder(beginWidth,
								 beginHeight,
								 endWidth,
								 endHeight,
								 ref fromPointBorderX,
								 ref toPointBorderX,
								 ref fromPointBorderY,
								 ref toPointBorderY);
					break;
				case GoBoardPosition.BottomBorder:
					fromPointX = new Point(middleWidth, middleHeight);
					toPointX = new Point(middleWidth, beginHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					SetBottomBorder(beginWidth,
									beginHeight,
									endWidth,
									endHeight,
									ref fromPointBorderX,
									ref toPointBorderX,
									ref fromPointBorderY,
									ref toPointBorderY);
					break;
				case GoBoardPosition.LeftBorder:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(middleWidth, middleHeight);
					toPointY = new Point(endWidth, middleHeight);
					SetLeftBorder(beginWidth,
								  beginHeight,
								  endWidth,
								  endHeight,
								  ref fromPointBorderX,
								  ref toPointBorderX,
								  ref fromPointBorderY,
								  ref toPointBorderY);
					break;
				case GoBoardPosition.RightBorder:
					fromPointX = new Point(middleWidth, beginHeight);
					toPointX = new Point(middleWidth, endHeight);
					fromPointY = new Point(beginWidth, middleHeight);
					toPointY = new Point(middleWidth, middleHeight);
					SetRightBorder(beginWidth,
								   beginHeight,
								   endWidth,
								   endHeight,
								   ref fromPointBorderX,
								   ref toPointBorderX,
								   ref fromPointBorderY,
								   ref toPointBorderY);
					break;
				case GoBoardPosition.LeftNotation:
					break;
				case GoBoardPosition.RightNotation:
					break;
				case GoBoardPosition.TopNotation:
					break;
				case GoBoardPosition.BottomNotation:
					break;
				default:
					break;
			}
			g.DrawLine(_penTable, fromPointX, toPointX);
			g.DrawLine(_penTable, fromPointY, toPointY);
			if (_penBorder != null)
			{
				if (fromPointBorderX.X != -1)
				{
					g.DrawLine(_penBorder, fromPointBorderX, toPointBorderX);
					g.DrawLine(_penBorder, fromPointBorderY, toPointBorderY);
				}
				if (fromPointBorderX2.X != -1)
				{
					g.DrawLine(_penBorder, fromPointBorderX2, toPointBorderX2);
					g.DrawLine(_penBorder, fromPointBorderY2, toPointBorderY2);
				}
			}
			g.CompositingMode = CompositingMode.SourceOver;
			if (pLabel.CellDetail.IsIntersection)
			{
				RectangleF circleIntersection = new(middleWidth - 4, middleHeight - 4, 8, 8);
				g.FillEllipse(ShareGraphicObject.SolidBrush(_penTable.Color), circleIntersection);
			}
		}
		private RectangleF GetRectangleCircle(int labelWidth, int labelHeight)
		{
			int space = 1;
			int doubleSpace = space * 2;
			return new(space, space, labelWidth - doubleSpace, labelHeight - doubleSpace);
		}
		public void PaintStone(Graphics g, ExtendLabel pLabel)
		{
			RectangleF circle = GetRectangleCircle(pLabel.Width, pLabel.Height);
			RectangleF circleImage = circle with {
				Width = circle.Width - 0.5f,
				Height = circle.Height - 0.5f
			};
			if (pLabel.CellDetail.CellValue == CellValue.White)
			{
				if (_whiteStoneImagePath.Trim() != "")
				{
					g.DrawImage(ShareGraphicObject.BitmapFilePath(_whiteStoneImagePath), circleImage);
				}
				else
				{
					g.FillEllipse(ShareGraphicObject.SolidBrush(_whiteStoneBackColor), circle);
					g.DrawEllipse(ShareGraphicObject.Pen(_whiteStoneBorderColor, 0.2f), circle);
				}
			}
			if (pLabel.CellDetail.CellValue == CellValue.Black)
			{
				if (_blackStoneImagePath.Trim() != "")
				{
					g.DrawImage(ShareGraphicObject.BitmapFilePath(_blackStoneImagePath), circleImage);
				}
				else
				{
					g.FillEllipse(ShareGraphicObject.SolidBrush(_blackStoneBackColor), circle);
					g.DrawEllipse(ShareGraphicObject.Pen(_blackStoneBorderColor, 0.2f), circle);
				}
			}
		}
		public void PaintNeighbour(Graphics g, ExtendLabel pLabel)
		{
			RectangleF circle = GetRectangleCircle(pLabel.Width, pLabel.Height);
			if (pLabel.CellDetail.IsNeighborCell)
			{
				g.FillEllipse(ShareGraphicObject.SolidBrush(Color.Gray), circle);
			}
		}
		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			PaintBorder(graphics, pLabel);
			PaintStone(graphics, pLabel);
			//We Actually don't use this for actual gaming
			//We paint NeighborCell to shows its position for explaining the postion for Tutorial and for debugging purpose only
			//Uncomment this code if you need to see Neighbor cell
			//[DEBUG:]
			//PaintNeighbour(g, pLabel);
		}
	}
}
