using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpMoku
{
	public partial class FormAbout
		: Form
	{
		public FormAbout()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormAbout_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;
			UpdateUIColor(Common.BackColor, Common.ForeColor);
		}
		private void UpdateUIColor(Color backColor, Color foreColor)
		{
			lblProgramName.ForeColor = foreColor;
			linkLabel1.LinkColor = foreColor;
			BackColor = backColor;
		}
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(linkLabel1.Text);
		}
	}
}
