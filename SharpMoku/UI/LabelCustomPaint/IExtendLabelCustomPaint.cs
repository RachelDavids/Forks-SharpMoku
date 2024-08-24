using System.Drawing;

namespace SharpMoku.UI.LabelCustomPaint
{
	public interface IExtendLabelCustomPaint
	{
		void Paint(Graphics graphics, ExtendLabel pLabel);
	}
}
