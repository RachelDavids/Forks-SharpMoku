using System.Drawing;

namespace SharpMoku.UI.LabelCustomPaint
{
	public class TicTacToe1
		: IExtendLabelCustomPaint
	{

		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{

			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//Color temp = pLabel.BackColor;
			if (pLabel.CellDetail.CellValue == CellValue.White)
			{

				int offset = 8;
				float lineWidth = 4.9f;
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
					RectangleF recCircle = new(7, 7, pLabel.Width - 14, pLabel.Height - 14);
					graphics.DrawEllipse(ShareGraphicObject.Pen(pLabel.theme.OColor, characterWidth / 2),
										 recCircle);
				}
			}
		}
	}
}
