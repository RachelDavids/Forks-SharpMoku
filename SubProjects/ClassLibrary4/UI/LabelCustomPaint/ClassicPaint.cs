using System.Drawing;

namespace SharpMoku.UI
{
	public class ClassicPaint : IExtendLabelCustomPaint
	{
		public void Paint(Graphics g, ExtendLabel pLabel)
		{
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			FontFamily family = new("Segoe UI");

			Font font = new(family, 24.25f, FontStyle.Bold);

			Color ForeColor = pLabel.theme.OColor;
			if (pLabel.CellAttribute.CellValue == CellValue.Black)
			{
				ForeColor = pLabel.theme.XColor;
			}
			StringFormat sf = new() {
				LineAlignment = StringAlignment.Center,
				Alignment = StringAlignment.Center
			};

			if (pLabel.CellAttribute.CellValue != CellValue.Empty)
			{
				int xOffset = 3;
				int yOffset = 3;
				Rectangle rectangleCircle = new(
				   pLabel.ClientRectangle.X + xOffset,
				   pLabel.ClientRectangle.Y + yOffset,
				   pLabel.ClientRectangle.Width - (xOffset * 2),
				   pLabel.ClientRectangle.Height - (yOffset * 2));

				g.FillEllipse(ShareGraphicObject.SolidBrush(ForeColor), rectangleCircle);
			}
			g.DrawRectangle(ShareGraphicObject.Pen(Color.Black, 2f), pLabel.ClientRectangle);

			// Don't use TextRendered, your text will not be in center alginment
			/*
            TextRenderer.DrawText(g, pLabel.CustomDrawValue , font, 
                                     pLabel.ClientRectangle, 
                                     ForeColor );
                                     
            */
			// DrawShadow(g, pLabel.ClientRectangle);
		}
	}
}
