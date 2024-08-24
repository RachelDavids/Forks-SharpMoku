using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI.Theme
{
	public abstract class Theme
	{
		public Color NotationForeColor { get; set; } = Color.Black;
		public int CellHeight { get; set; }
		public int CellWidth { get; set; }
		public int CellCornerRadius { get; set; }
		public IExtendLabelCustomPaint CustomPaint { get; set; }
		public int SpaceBetweenBorderSize { get; set; } = -1;
		public Color CellBackColor { get; set; }
		public BorderStyle CellBorderStyle { get; set; }
		//public GomokuCellDetail CellDetail { get; set; } = new();
		public Color XColor { get; set; }
		public Color OColor { get; set; }
		public Color BoardColor { get; set; }
		//public int BoardSize;
		public Bitmap BoardImage {
			get {
				if (_boardImage == null)
				{
					LoadBoardImage();
				}
				return _boardImage;
			}
		}

		public void ApplyThemeToCell(ExtendLabel label)
		{
			label.Width = CellWidth;
			label.Height = CellHeight;
			label.CornerRadius = CellCornerRadius;
			label.CustomPaint = CustomPaint;
			label.BorderStyle = CellBorderStyle;
			label.Theme = this;
			label.BackColor = CellBackColor;

		}
		public string BoardImageFile { get; set; } = "";
		public bool HasImage => BoardImageFile.Trim() != "";
		private void LoadBoardImage()
		{

			if (BoardImageFile == "")
			{
				return;
			}

			Bitmap B = (Bitmap)Image.FromFile(BoardImageFile);
			B.SetResolution(96, 96);

			_boardImage = B;
		}

		private Bitmap _boardImage;
		//public void ApplyThemeToBoard(Panel p)
		//{
		//	p.Paint += Panel1_Paint;
		//}
		public void ApplyThemeToBoard(PictureBox p)
		{
			p.BackColor = BoardColor;
			p.Paint += Panel1_Paint;
		}
		private void Panel1_Paint(object sender, PaintEventArgs e)
		{

			//int LastIndex = BoardSize - 1;

		}
	}
}
