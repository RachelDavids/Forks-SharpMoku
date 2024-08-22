using System.Drawing;

namespace SharpMoku.UI
{
	public interface IExtendLabelCustomPaint
	{
		void Paint(Graphics graphics, ExtendLabel pLabel);
	}
}
