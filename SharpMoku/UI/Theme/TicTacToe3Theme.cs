using System.Drawing;

namespace SharpMoku.UI.Theme
{
	public class TicTacToe3Theme : Theme
	{
		public TicTacToe3Theme()
		{

			NotationForeColor = Color.White;
			CellBackColor = Color.White;
			BoardColor = Color.White;
			XColor = Color.FromArgb(230, 107, 38);
			OColor = Color.FromArgb(20, 185, 150);
			CustomPaint = new LabelCustomPaint.TicTacToe3();

		}
	}
}
