using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI
{
	/*
     Credit Code for rounding
     https://stackoverflow.com/questions/42627293/label-with-smooth-rounded-corners
     */

	public class ExtendLabel
		: Label
	{
		public GomokuCellDetail CellDetail { get; } = new();
		public Theme.Theme Theme { get; set; }
		[Browsable(true)]
		public override Color BackColor {
			get => base.BackColor;
			set {
				base.BackColor = value;
				Invalidate();
			}
		}

		public ExtendLabel()
		{
			base.DoubleBuffered = true;
			base.BackColor = Color.Transparent;

		}
		//public void MakeRound()
		//{
		//	_ = new GraphicsPath();
		//	GraphicsPath path = _getRoundRectangle(ClientRectangle);
		//	Region = new Region(path);
		//}

		public IExtendLabelCustomPaint CustomPaint { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (CornerRadius > 0)
			{
				using (GraphicsPath graphicsPath = GetRoundRectangle(ClientRectangle))
				{

					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
					using (SolidBrush brush = new(BackColor))
					{
						e.Graphics.FillPath(brush, graphicsPath);
					}

					TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor);
				}
			}
			CustomPaint?.Paint(e.Graphics, this);

		}

		public int CornerRadius { get; set; } = 7;

		public GraphicsPath GetRoundRectangle(Rectangle rectangle)
		{
			int diminisher = 1;
			GraphicsPath path = new();
			path.AddArc(rectangle.X, rectangle.Y, CornerRadius, CornerRadius, 180, 90);
			path.AddArc(rectangle.X + rectangle.Width - CornerRadius - diminisher, rectangle.Y, CornerRadius, CornerRadius, 270, 90);
			path.AddArc(rectangle.X + rectangle.Width - CornerRadius - diminisher, rectangle.Y + rectangle.Height - CornerRadius - diminisher, CornerRadius, CornerRadius, 0, 90);
			path.AddArc(rectangle.X, rectangle.Y + rectangle.Height - CornerRadius - diminisher, CornerRadius, CornerRadius, 90, 90);
			path.CloseAllFigures();
			return path;
		}
	}
}
