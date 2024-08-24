using System;
using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.Theme;

namespace SharpMoku
{
	public partial class FormOption : Form
	{
		public FormOption()
		{
			InitializeComponent();
		}
		public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

		private void InitialValue()
		{

			chkAllowUndo.Checked = Common.CurrentSettings.IsAllowUndo;
			chkWriteLogFile.Checked = Common.CurrentSettings.IsWriteLog;

			chkBotMouseMove.Checked = Common.CurrentSettings.IsUseBotMouseMove;
			cboTheme.SelectedIndex = (int)Common.CurrentSettings.KnownTheme;

			cboTheme.SelectedIndexChanged += CboTheme_SelectedIndexChanged;
			UpdateUIColor(Common.BackColor, Common.ForeColor);

		}
		private void UpdateUIColor(Color backColor, Color foreColor)
		{
			lblTheme.ForeColor = foreColor;
			chkBotMouseMove.ForeColor = foreColor;
			chkWriteLogFile.ForeColor = foreColor;
			chkAllowUndo.ForeColor = foreColor;

			BackColor = backColor;
		}
		private void CboTheme_SelectedIndexChanged(object sender, EventArgs e)
		{

			KnownTheme themeEnum = (KnownTheme)cboTheme.SelectedIndex;

			ThemeChangedEventArgs eventArgs = new(ThemeFactory.Create(themeEnum));
			ThemeChanged?.Invoke(this, eventArgs);

			ThemeFactory.BackColor(themeEnum);
			UpdateUIColor(ThemeFactory.BackColor(themeEnum),
			ThemeFactory.ForeColor(themeEnum));

		}

		private void FormOption_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;

			InitialValue();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{

			Common.CurrentSettings.IsUseBotMouseMove = chkBotMouseMove.Checked;

			Common.CurrentSettings.IsAllowUndo = chkAllowUndo.Checked;
			Common.CurrentSettings.IsWriteLog = chkWriteLogFile.Checked;

			Common.CurrentSettings.KnownTheme = (KnownTheme)cboTheme.SelectedIndex;

			Common.SaveSettings();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void cboTheme_SelectedIndexChanged_1(object sender, EventArgs e)
		{

		}
	}
}
