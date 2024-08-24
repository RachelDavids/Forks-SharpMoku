using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI.Theme
{
	/*
     *Credit:https://codepen.io/mudrenok/pen/gpMXgg
     */
	public class SleekTheme
		: Theme
	{
		public SleekTheme()
		{
			NotationForeColor = Color.White;
			CellBackColor = Color.FromArgb(52, 73, 94);
			BoardColor = Color.FromArgb(40, 50, 70);
			CellCornerRadius = 10;
			CellBorderStyle = BorderStyle.FixedSingle;
			XColor = Color.FromArgb(46, 204, 113);
			OColor = Color.OrangeRed;
			CustomPaint = new TicTacToe1();

		}
	}
}
