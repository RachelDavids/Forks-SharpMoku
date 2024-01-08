using System.Drawing;

namespace SharpMoku.UI.Theme
{
	public class TicTacToe2Theme : Theme
	{
		public TicTacToe2Theme()
		{

			NotationForeColor = Color.White;
			CellBackColor = Color.White;
			BoardColor = Color.FromArgb(53, 152, 219);
			XColor = Color.White;
			OColor = Color.White;
			CustomPaint = new LabelCustomPaint.TicTacToe2();

		}
	}
}
