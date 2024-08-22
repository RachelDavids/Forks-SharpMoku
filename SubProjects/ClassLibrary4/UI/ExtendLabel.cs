using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SharpMoku.UI
{
	/*
     Credit Code for rounding
     https://stackoverflow.com/questions/42627293/label-with-smooth-rounded-corners
     */

	public class ExtendLabel
		: Label
	{
		public GomokuCellDetail CellDetail = new();
		public Theme.Theme theme;
		[Browsable(true)]
		public Color _BackColor {
			get => _backColor;
			set {
				_backColor = value;
				Invalidate();
			}
		}
		private Color _backColor;

		public ExtendLabel()
		{
			base.DoubleBuffered = true;
			base.BackColor = Color.Transparent;

		}
		public void MakeRound()
		{
			_ = new GraphicsPath();
			GraphicsPath path = _getRoundRectangle(ClientRectangle);
			Region = new Region(path);
		}

		public IExtendLabelCustomPaint CustomPaint { get; set; } = null;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (CornerRadius > 0)
			{
				using (GraphicsPath graphicsPath = _getRoundRectangle(ClientRectangle))
				{

					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
					using (SolidBrush brush = new(_BackColor))
					{
						e.Graphics.FillPath(brush, graphicsPath);
					}

					TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor);
				}
			}
			CustomPaint?.Paint(e.Graphics, this);

		}

		public int CornerRadius { get; set; } = 7;

		public GraphicsPath _getRoundRectangle(Rectangle rectangle)
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
