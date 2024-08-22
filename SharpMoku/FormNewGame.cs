using System;
using System.Windows.Forms;

namespace SharpMoku
{
	public partial class FormNewGame
		: Form
	{
		public FormNewGame()
		{
			InitializeComponent();
		}
		private void InitialValue()
		{
			cboBotLevel.SelectedIndex = 0;
			cboBoardSize.SelectedIndex = 0;
			cboMode.SelectedIndex = Common.CurrentSettings.GameMode switch {
				Game.GameModeEnum.PlayerVsPlayer => 0,
				Game.GameModeEnum.PlayerVsBot => 1,
				Game.GameModeEnum.BotVsPlayer => 2,
				_ => 0
			};

			cboBoardSize.SelectedIndex = 0;
			if (Common.CurrentSettings.BoardSize == 15)
			{
				cboBoardSize.SelectedIndex = 1;
			}
			if (Common.CurrentSettings.BotDepth == 4)
			{
				cboBotLevel.SelectedIndex = 1;
			}
			BackColor = Common.BackColor;
			lblBoardSize.ForeColor = Common.ForeColor;
			lblBotLevel.ForeColor = Common.ForeColor;
			lblMode.ForeColor = Common.ForeColor;

		}
		private void FormNewGame_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;
			InitialValue();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Common.CurrentSettings.BoardSize = 9;
			if (cboBoardSize.SelectedIndex == 1)
			{
				Common.CurrentSettings.BoardSize = 15;
			}

			Common.CurrentSettings.GameMode = Game.GameModeEnum.PlayerVsPlayer;
			if (cboMode.SelectedIndex == 1)
			{
				Common.CurrentSettings.GameMode = Game.GameModeEnum.PlayerVsBot;
			}
			else if (cboMode.SelectedIndex == 2)
			{
				Common.CurrentSettings.GameMode = Game.GameModeEnum.BotVsPlayer;
			}

			Common.CurrentSettings.BotDepth = 2;
			if (cboBotLevel.SelectedIndex == 1)
			{

				Common.CurrentSettings.BotDepth = 4;
			}

			Common.SaveSettings();

			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
