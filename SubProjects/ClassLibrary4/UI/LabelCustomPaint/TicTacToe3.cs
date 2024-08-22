using System.Drawing;

using SharpMoku.Utility;

using PositionEnum = SharpMoku.GoBoardPosition;

namespace SharpMoku.UI.LabelCustomPaint
{
	internal class TicTacToe3 : IExtendLabelCustomPaint
	{

		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{

			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			Color labelBackColor = Color.White; // pLabel.BackColor;

			Rectangle rec = pLabel.ClientRectangle;
			RectangleF BorderRec = new(rec.X + 0.5f, rec.Y + 0.5f, rec.Width - 1, rec.Height - 1);

			graphics.DrawRectangle(ShareGraphicObject.Pen(Color.Black, 1), BorderRec.X, BorderRec.Y, BorderRec.Width, BorderRec.Height);

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
					new PointF(BorderRec.X, BorderRec.Y),
					new PointF(BorderRec.X + BorderRec.Width, BorderRec.Y));
			}

			if (isNeedToRemoveLeftBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
					new PointF(BorderRec.X, BorderRec.Y),
					new PointF(BorderRec.X, BorderRec.Y + BorderRec.Height));
			}
			if (isNeedToRemoveRightBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
					new PointF(BorderRec.X + BorderRec.Width, BorderRec.Y),
					new PointF(BorderRec.X + BorderRec.Width, BorderRec.Y + BorderRec.Height));
			}
			if (isNeedToRemoveBottomBorder)
			{
				graphics.DrawLine(ShareGraphicObject.Pen(labelBackColor, 2),
					new PointF(BorderRec.X, BorderRec.Y + BorderRec.Height),
					new PointF(BorderRec.X + BorderRec.Width, BorderRec.Y + BorderRec.Height));
			}

			if (pLabel.CellDetail.CellValue == CellValue.White)
			{

				int offset = 8;
				float lineWidth = 3.3f;
				graphics.DrawLine(ShareGraphicObject.Pen(pLabel.theme.XColor, lineWidth),
									 offset,
									 offset,
									 pLabel.Width - offset,
									 pLabel.Height - offset);

				graphics.DrawLine(ShareGraphicObject.Pen(pLabel.theme.XColor, lineWidth),
									 offset,
									 pLabel.Width - offset,
									 pLabel.Height - offset,
									 offset);
			}
			else
			{
				if (pLabel.CellDetail.CellValue == CellValue.Black)
				{

					RectangleF RecCircle = new(8, 8, pLabel.Width - 16, pLabel.Height - 16);

					graphics.DrawEllipse(ShareGraphicObject.Pen(pLabel.theme.OColor, 3), RecCircle);

				}
			}
		}
	}
}
