using System.Drawing;

using SharpMoku.Board;

namespace SharpMoku.UI.LabelCustomPaint
{
	public sealed class ClassicPaint
		: IExtendLabelCustomPaint
	{
		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//FontFamily family = new("Segoe UI");

			//Font font; = new(family, 24.25f, FontStyle.Bold);

			Color foreColor = pLabel.Theme.OColor;
			if (pLabel.CellDetail.CellValue == CellValue.Black)
			{
				foreColor = pLabel.Theme.XColor;
			}
			//StringFormat sf = new() {
			//	LineAlignment = StringAlignment.Center,
			//	Alignment = StringAlignment.Center
			//};

			if (pLabel.CellDetail.CellValue != CellValue.Empty)
			{
				int xOffset = 3;
				int yOffset = 3;
				Rectangle rectangleCircle = new(
				   pLabel.ClientRectangle.X + xOffset,
				   pLabel.ClientRectangle.Y + yOffset,
				   pLabel.ClientRectangle.Width - (xOffset * 2),
				   pLabel.ClientRectangle.Height - (yOffset * 2));

				graphics.FillEllipse(ShareGraphicObject.SolidBrush(foreColor), rectangleCircle);
			}
			graphics.DrawRectangle(ShareGraphicObject.Pen(Color.Black, 2f), pLabel.ClientRectangle);

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
