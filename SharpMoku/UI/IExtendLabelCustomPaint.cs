using System.Drawing;

using SharpMoku.Board;
using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI
{
	public class Connect5Paint
		: IExtendLabelCustomPaint
	{

		public void Paint(Graphics graphics, ExtendLabel pLabel)
		{

			// Pen P;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			int Radius = 8;
			RectangleF RecCircle = new(Radius, Radius, pLabel.Width - (Radius * 2), pLabel.Height - (Radius * 2));

			Color c = Color.White;
			switch (pLabel.CellDetail.CellValue)
			{
				case CellValue.Black:
					c = pLabel.Theme.XColor;
					break;
				case CellValue.White:
					c = pLabel.Theme.OColor;
					break;
				case CellValue.Empty:
					break;
				default:
					break;
			}

			graphics.FillEllipse(ShareGraphicObject.SolidBrush(c), RecCircle);
		}
	}

}
