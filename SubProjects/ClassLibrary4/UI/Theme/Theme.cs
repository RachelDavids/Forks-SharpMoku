using System.Drawing;
using System.Windows.Forms;

namespace SharpMoku.UI.Theme
{
	public abstract class Theme
	{

		public Color NotationForeColor { get; set; } = Color.Black;
		public int CellHeight;
		public int CellWidth;
		public int CellCornerRadius;
		public IExtendLabelCustomPaint CustomPaint;
		public int SpaceBetweenBorderSize = -1;
		public Color CellBackColor;
		public BorderStyle CellBorderStyle;
		public GomokuCellDetail CellDetail = new();
		public Color XColor { get; set; }
		public Color OColor { get; set; }
		public Color BoardColor;
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
			label.theme = this;
			label._BackColor = CellBackColor;

		}
		public string BoardImageFile = "";
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

		private Bitmap _boardImage = null;
		public void ApplyThemeToBoard(Panel p)
		{
			p.Paint += Panel1_Paint;
		}
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
