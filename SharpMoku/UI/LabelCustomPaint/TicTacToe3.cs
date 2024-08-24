using System.Drawing;

using SharpMoku.Board;
using SharpMoku.Utility;

using PositionEnum = SharpMoku.UI.GoBoardPosition;

namespace SharpMoku.UI.LabelCustomPaint
{
	internal sealed class TicTacToe3
		: IExtendLabelCustomPaint
	{

		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{

			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			Color labelBackColor = Color.White; // pLabel.BackColor;

			Rectangle rec = pLabel.ClientRectangle;
			RectangleF borderRec = new(rec.X + 0.5f, rec.Y + 0.5f, rec.Width - 1, rec.Height - 1);

			graphics.DrawRectangle(ShareGraphicObject.Pen(Color.Black, 1),
								   borderRec.X,
								   borderRec.Y,
								   borderRec.Width,
								   borderRec.Height);

			PositionEnum boardPosition = pLabel.CellDetail.GoBoardPosition;
			bool isNeedToRemoveTopBorder = boardPosition.In(PositionEnum.TopLeftCorner,
															PositionEnum.TopBorder,
															PositionEnum.TopRightCorner);

			bool isNeedToRemoveLeftBorder = boardPosition.In(PositionEnum.TopLeftCorner,
															 PositionEnum.LeftBorder,
															 PositionEnum.BottomLeftCorner);

			bool isNeedToRemoveRightBorder = boardPosition.In(PositionEnum.TopRightCorner,
															  PositionEnum.RightBorder,
															  PositionEnum.BottomRightCorner);

			bool isNeedToRemoveBottomBorder = boardPosition.In(PositionEnum.BottomLeftCorner,
															   PositionEnum.BottomBorder,
															   PositionEnum.BottomRightCorner);

			if (isNeedToRemoveTopBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
								  new(borderRec.X, borderRec.Y),
								  new PointF(borderRec.X + borderRec.Width, borderRec.Y));
			}

			if (isNeedToRemoveLeftBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
								  new(borderRec.X, borderRec.Y),
								  new PointF(borderRec.X, borderRec.Y + borderRec.Height));
			}
			if (isNeedToRemoveRightBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
								  new(borderRec.X + borderRec.Width, borderRec.Y),
								  new PointF(borderRec.X + borderRec.Width, borderRec.Y + borderRec.Height));
			}
			if (isNeedToRemoveBottomBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
								  new(borderRec.X, borderRec.Y + borderRec.Height),
								  new PointF(borderRec.X + borderRec.Width, borderRec.Y + borderRec.Height));
			}

			if (pLabel.CellDetail.CellValue == CellValue.White)
			{
				int offset = 8;
				float lineWidth = 3.3f;
				graphics.DrawLine(ShareGraphicObject.Pen(pLabel.Theme.XColor, lineWidth),
								  offset,
								  offset,
								  pLabel.Width - offset,
								  pLabel.Height - offset);

				graphics.DrawLine(ShareGraphicObject.Pen(pLabel.Theme.XColor, lineWidth),
								  offset,
								  pLabel.Width - offset,
								  pLabel.Height - offset,
								  offset);
			}
			else
			{
				if (pLabel.CellDetail.CellValue == CellValue.Black)
				{

					RectangleF recCircle = new(8, 8, pLabel.Width - 16, pLabel.Height - 16);
					graphics.DrawEllipse(ShareGraphicObject.Pen(pLabel.Theme.OColor, 3),
										 recCircle);

				}
			}
		}
	}
}
