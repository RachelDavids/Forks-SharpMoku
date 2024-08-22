using System.Drawing;

namespace SharpMoku.UI.LabelCustomPaint
{
	public class TicTacToe2 : IExtendLabelCustomPaint
	{

		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{

			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			//Color temp = pLabel.BackColor;
			Rectangle rec = pLabel.ClientRectangle;
			RectangleF borderRec = new(rec.X + 0.5f, rec.Y + 0.5f, rec.Width - 1, rec.Height - 1);

			graphics.DrawRectangle(ShareGraphicObject.Pen(Color.White, 1),
								   borderRec.X,
								   borderRec.Y,
								   borderRec.Width,
								   borderRec.Height);
			if (pLabel.CellDetail.CellValue == CellValue.White)
			{

				int offset = 12;
				float lineWidth = 3f;
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
					float characterWidth = 8;

					RectangleF RecCircle = new(12, 12, pLabel.Width - 24, pLabel.Height - 24);

					graphics.DrawEllipse(ShareGraphicObject.Pen(pLabel.theme.OColor, characterWidth / 2), RecCircle);

				}
			}
		}
	}
}

