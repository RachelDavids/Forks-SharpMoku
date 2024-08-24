using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpMoku
{
	public partial class FormCustomMessageBox : Form
	{
		public FormCustomMessageBox()
		{
			InitializeComponent();
		}

		public string Caption {
			get => Text;
			set => Text = value;

		}
		public string Message {
			get => txtMessage.Text;
			set => txtMessage.Text = value;
		}

		private bool _ShowCancel;
		public bool ShowCancel {
			get => _ShowCancel;
			set {
				_ShowCancel = value;
				SetCancelbuttonVisible();
			}
		}
		private void SetCancelbuttonVisible()
		{
			btnCancel.Visible = true;
			btnOK.Left = 247;
			if (!_ShowCancel)
			{
				btnCancel.Visible = false;
				btnOK.Left = btnCancel.Left;
			}
		}
		private void UpdateUIColor(Color backColor, Color foreColor)
		{

			txtMessage.BackColor = backColor;
			txtMessage.ForeColor = foreColor;

			BackColor = backColor;
		}
		private static void SetTheme()
		{
			/*
            this.txtMessage.BackColor = Global.CurrentTheme.FormBackColor;
            this.txtMessage.ForeColor = Global.CurrentTheme.LabelForeColor;
            Utility.UI.MakeFormCaptionToBeDarkMode(this, Global.CurrentTheme.IsFormCaptionDarkMode);
            Utility.ThemeUtility themeUtil = new Utility.ThemeUtility(Global.CurrentTheme);
            themeUtil
                .SetButtonColor(this.btnOK, this.btnCancel)
                .SetForm(this);

            */
		}
		private void FormCustomMessageBox_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;

			pictureBox1.Image = SystemIcons.Information.ToBitmap();
			//this.SetTheme();
			txtMessage.GotFocus += (_, _) => btnOK.Focus();

			UpdateUIColor(Common.BackColor, Common.ForeColor);
		}

		private delegate void DisplayDialogCallback();
		//public Form ParentForm { get; set; }
		public void ShowDialogAtCenter()
		{
			Left = ParentForm!.Left + ((ParentForm.Width - Width) / 2);
			Top = ParentForm.Top + ((ParentForm.Height - Height) / 2);

			ShowDialog();
		}
		public void DisplayDialog()
		{
			if (InvokeRequired)
			{
				Invoke(new DisplayDialogCallback(DisplayDialog));
				return;
			}

			if (IsHandleCreated)
			{
				if (ParentForm != null)
				{
					Left = ParentForm.Left + ((ParentForm.Width - Width) / 2);
					Top = ParentForm.Top + ((ParentForm.Height - Height) / 2);
				}
				ShowDialog();

				if (CanFocus)
				{
					Focus();
				}
			}
			else
			{
				// Handle the error
			}

		}

		private void btnOK_Click_1(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click_1(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
