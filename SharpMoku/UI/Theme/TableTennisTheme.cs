using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;

namespace SharpMoku.UI.Theme
{
	public class TableTennisTheme
		: Theme
	{
		public TableTennisTheme()
		{
			CellCornerRadius = 0;
			CellBorderStyle = BorderStyle.FixedSingle;
			BoardColor = Color.FromArgb(30, 143, 213);
			XColor = Color.Orange; // Color.Teal;
			OColor = Color.White;
			NotationForeColor = Color.White;
			CustomPaint = new ClassicPaint();
		}
	}
}
