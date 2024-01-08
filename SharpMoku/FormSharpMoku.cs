using System;
using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI;

namespace SharpMoku
{
	public partial class FormSharpMoku : Form, IUI
	{
		/* This class imeplement IUI
		 * It has UI.PictureBoxGoMoKu picGoMoku that responsible for rendering a board
		 * It has game object that use to bind between this class, game object and the board information
		 */
		public FormSharpMoku()
		{
			InitializeComponent();
		}

		private UI.PictureBoxGoMoKu picGoMoku = null;
		private Game game = null;

		//AI.IEvaluate bot = new AI.EvaluateV2();
		private AI.IEvaluate bot = new AI.EvaluateV3();

		public event Board.CellClickHandler CellClicked;
		public event EventHandler HasFinishedMoveCursor;

		private void UpdateTheme(UI.ThemeSpace.Theme theme)
		{

			picGoMoku.Initial(theme);
			picGoMoku.ReRender();
			ScaleBoard();
		}
		private void ScaleBoard()
		{
			//Scale the board to be bigger in case the board size is 9
			if (game.board.BoardSize == 9)
			{
				picGoMoku.Visible = false;
				picGoMoku.Scale(new SizeF(1.5f, 1.5f));
				picGoMoku.Top = menuStrip1.Height + 5;
				picGoMoku.Visible = true;
			}
		}
		// int iCountGameID = 0;

		private void NewGame(Game.GameModeEnum gameMode,
							 int boardSize,
							 int botSearchDepth,
							 UI.ThemeSpace.ThemeFactory.ThemeEnum themeEnum) =>
				NewGame(gameMode, boardSize, botSearchDepth, themeEnum, null);

		private void CreateGame(Game.GameModeEnum gameMode, int boardSize, int botDepth, Board board)
		{
			game?.ReleaseResource();

			game = board != null
					   ? new Game(this, board, bot, botDepth, gameMode)
					   : new Game(this, boardSize, bot, botDepth, gameMode);

			game.log = new SimpleLog(Utility.FileUtility.LogFilePath);

		}
		private void NewGame(Game.GameModeEnum gameMode,
			int boardSize,
			int botSearchDepth,
			UI.ThemeSpace.ThemeFactory.ThemeEnum themeEnum,
			Board board)
		{

			UI.ThemeSpace.Theme currentTheme = UI.ThemeSpace.ThemeFactory.Create(themeEnum);
			CreateGame(gameMode, boardSize, botSearchDepth, board);

			const int cellWidth = 38;
			const int cellHeight = 38;

			// If it is the first time to loadboard or the boardSize is different
			// So we need to create a new picGoMoku
			if (picGoMoku == null ||
					picGoMoku.NoofRow != boardSize)
			{
				if (picGoMoku != null)
				{
					picGoMoku.CellClicked -= PicGoMoku_CellClicked;
					picGoMoku.ReleaseResource();
					Controls.Remove(picGoMoku);
				}
				picGoMoku = new PictureBoxGoMoKu(game.board, cellWidth, cellHeight) {
					NoofRow = boardSize,
					NoofColumn = boardSize,
					BorderStyle = BorderStyle.FixedSingle,

					Left = 0,
					Top = menuStrip1.Height + 5
				};
				picGoMoku.Initial(currentTheme);
				picGoMoku.CellClicked -= PicGoMoku_CellClicked;
				picGoMoku.CellClicked += PicGoMoku_CellClicked;

			}
			else
			{
				//Using the old picGomoku
				//picGoMoku.Top = this.menuStrip1.Height + 5;
				picGoMoku.SetBoad(game.board);
				picGoMoku.Initial(currentTheme);
			}
			if (!Controls.Contains(picGoMoku))
			{
				Controls.Add(picGoMoku);
			}

			game.NewGame();

			picGoMoku.Visible = true;
			ScaleBoard();

			Height = picGoMoku.Height + picGoMoku.Top + menuStrip1.Height - 5;
			Width = picGoMoku.Width + picGoMoku.Left;

			RenderUI();
		}

		public void Game_BotFinishedThinking(object sender, EventArgs e)
		{

			IsBotThinking = false;
			Cursor = Cursors.Default;

		}

		private bool IsBotThinking = false;// If this value is true it will prevent user from click on the cell

		private Cursor tempCursor = Cursors.Default;
		public void Game_BotThinking(object sender, EventArgs e)
		{
			IsBotThinking = true;
			Cursor = Cursors.WaitCursor;
		}

		private void TShowPicGoMokue_Tick(object sender, EventArgs e)
		{

			picGoMoku.Visible = true;
			// throw new NotImplementedException();
		}

		public void Game_GameFinished(object sender, EventArgs e)
		{
			IsBotThinking = false;
			//    this.Cursor = Cursors.Default;
			if (game.GameState == Game.GameStateEnum.End)
			{
				/*
				bool isPlaywon =
					   (game.WinResult == Board.WinStatus.BlackWon && game.GameMode == Game.GameModeEnum.PlayerVsBot)
					|| (game.WinResult == Board.WinStatus.WhiteWon && game.GameMode == Game.GameModeEnum.BotVsPlayer);
					*/
				Game_GameFinishedV2(game.WinResult);
				/*
				if (isPlaywon)
				{
					Game_GameFinishedV2(game.WinResult);
				}
				*/
			}
			return;

		}
		private void Game_GameFinishedV2(Board.WinStatus winStatus)
		{
			// Deley a little bit before show the result.
			System.Threading.Thread.Sleep(300);

			string whiteTurn = "White";
			string blackTurn = "Black";

			switch (Global.CurrentSettings.ThemeEnum)
			{
				case UI.ThemeSpace.ThemeFactory.ThemeEnum.TicTacToe1:
				case UI.ThemeSpace.ThemeFactory.ThemeEnum.TicTacToe2:
				case UI.ThemeSpace.ThemeFactory.ThemeEnum.TicTacToe3:
					whiteTurn = "X";
					blackTurn = "O";
					break;
				case UI.ThemeSpace.ThemeFactory.ThemeEnum.TableTennis:
					blackTurn = "Orange";
					break;
			}
			string message;// = whiteTurn + " Won.";

			if (winStatus == Board.WinStatus.NotDecidedYet)
			{
				return;
			}

			message = winStatus switch {
				Board.WinStatus.BlackWon => $"{blackTurn} Won.",
				Board.WinStatus.WhiteWon => $"{whiteTurn} Won.",
				Board.WinStatus.Draw => " Draw.",
				_ => throw new InvalidOperationException($"winStatus is not correct {winStatus}"),
			};
			message += "\n Click ok if you want a rematch.";
			FormCustomMessageBox formCustomMessageBox = new() {
				Message = message,
				ShowCancel = true,
				StartPosition = FormStartPosition.Manual,
				parentForm = this
			};
			formCustomMessageBox.ShowDialogAtCenter();

			if (formCustomMessageBox.DialogResult == DialogResult.Cancel)
			{
				return;
			}
			// Rematch();
			NewGame(Global.CurrentSettings.GameMode,
					Global.CurrentSettings.BoardSize,
					Global.CurrentSettings.BotDepth,
					Global.CurrentSettings.ThemeEnum);

		}

		private void FormSharpMoku_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;

			NewGame(Global.CurrentSettings.GameMode,
					Global.CurrentSettings.BoardSize,
					Global.CurrentSettings.BotDepth,
					Global.CurrentSettings.ThemeEnum);
			undoToolStripMenuItem.Enabled = Global.CurrentSettings.IsAllowUndo;

		}

		private void PicGoMoku_CellClicked(PictureBoxGoMoKu pic, PictureBoxGoMoKu.PositionEventArgs positionClick)
		{
			if (IsBotThinking)
			{
				return;
			}

			if (game.GameState != Game.GameStateEnum.Playing)
			{
				return;
			}

			CellClicked?.Invoke(this, new(positionClick.Value));

		}

		public void RenderUI()
		{

			picGoMoku.ReRender();
			Application.DoEvents();
			/*
			 * Without Application.DoEvents
			 * When player click on the cell, it will not be render immidately it will wait
			 * until the bot has put the cell also. So instead of when player put black, the game render black first
			 * the game just make the bot caluculate its position immidately then render both white and black
			 *
			 * 1.First solution I tried to use Timer to delay the bot action, it works but it has
			 * the problem with UI thread becasue the thread created by the Timer cannot access UI thread
			 *
			 * 2. Second solution, do not delay anything just put Application.DoEvents();
			 */
		}

		public void MoveCursorTo(Position position)
		{
			if (!Global.CurrentSettings.IsUseBotMouseMove)
			{
				MouseAction_HasFinishedMoved(this, new EventArgs());
				return;
			}

			if (position.Row < 0 ||
				position.Col < 0 ||
				position.Row >= game.board.BoardSize ||
				position.Col >= game.board.BoardSize)
			{
				return;
			}

			Point ToPoint = picGoMoku.GetLowerRightPoint(position);

			MoveCursor(ToPoint);

		}
		private void MoveCursor(Point ToPoint)
		{
			MoveCursor(Cursor.Position, ToPoint);
		}

		private Random RandomGenerator = new();
		private void MoveCursor(Point FromPoint, Point ToPoint)
		{
			MouseAction.HasFinishedMoved -= MouseAction_HasFinishedMoved;
			MouseAction.HasFinishedMoved += MouseAction_HasFinishedMoved;

			int xDifferent = Math.Abs(FromPoint.X - ToPoint.X);
			int yDifferent = Math.Abs(FromPoint.Y - ToPoint.Y);
			int xyDifferent = xDifferent + yDifferent;

			/*
			 * Make a numberofSteps a little bit random
			 * but it cannot be more than 35 and cannot be less than 20;
			 * More number of steps, more number of time it takes
			 */
			int numberOfSteps = RandomGenerator.Next(10, 40);// 100 - xyDifferent +  RandomGenerator.Next(10, 40);

			int maximumSteps = 35;
			int minimumSteps = 20;
			numberOfSteps = Math.Max(Math.Min(numberOfSteps, maximumSteps), minimumSteps);

			MouseAction.LinearSmoothMove(MouseAction.convertDrawingPointToStructPoint(ToPoint), numberOfSteps);

		}

		private void MouseAction_HasFinishedMoved(object sender, EventArgs e)
		{

			HasFinishedMoveCursor?.Invoke(this, new EventArgs());
		}

		private void toolStripMenuItemNewGame_Click(object sender, EventArgs e)
		{
			FormNewGame formNewGame = new() {
				StartPosition = FormStartPosition.CenterParent
			};
			formNewGame.ShowDialog(this);
			if (formNewGame.DialogResult != DialogResult.OK)
			{
				return;
			}

			NewGame(Global.CurrentSettings.GameMode,
				Global.CurrentSettings.BoardSize,
				Global.CurrentSettings.BotDepth,
				Global.CurrentSettings.ThemeEnum);

		}

		private void toolStripMenuItemOption_Click(object sender, EventArgs e)
		{
			FormOption f = new();
			// f.Parent = this;
			f.Themed_Changed -= F_Themed_Changed;
			f.Themed_Changed += F_Themed_Changed;
			f.StartPosition = FormStartPosition.CenterParent;
			f.ShowDialog(this);
			if (f.DialogResult != DialogResult.OK)
			{
				return;
			}

			UpdateTheme(UI.ThemeSpace.ThemeFactory.Create(Global.CurrentSettings.ThemeEnum));
			undoToolStripMenuItem.Enabled = Global.CurrentSettings.IsAllowUndo;

		}

		private void F_Themed_Changed(object sender, EventArgs e)
		{
			FormOption.ThemChangedEventArgs eChange = (FormOption.ThemChangedEventArgs)e;
			UpdateTheme(eChange.Theme);

		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (game == null ||
			   !game.CanUndo ||
			   IsBotThinking)
			{
				return;
			}

			game.Undo();
		}

		private void copyBoardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog spf = new() {
				InitialDirectory = Utility.FileUtility.BoardPath,
				Filter = "Board |*.bin"
			};

			DialogResult dialogResult = spf.ShowDialog();

			if (dialogResult != DialogResult.OK)
			{
				return;
			}
			string fileName = spf.FileName;

			Utility.SerializeUtility.SerializeBoard(game.board, fileName);
		}

		private void loadBoardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new() {
				InitialDirectory = Utility.FileUtility.BoardPath,
				Filter = "Board |*.bin"
			};

			DialogResult dialogResult = opf.ShowDialog();

			if (dialogResult != DialogResult.OK)
			{
				return;
			}
			string fileName = opf.FileName;

			Board board = Utility.SerializeUtility.DeserializeBoard(fileName);

			NewGame(Global.CurrentSettings.GameMode,
				Global.CurrentSettings.BoardSize,
				Global.CurrentSettings.BotDepth,
				Global.CurrentSettings.ThemeEnum,
				board);

		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			SaveFileDialog spf = new() {
				InitialDirectory = Utility.FileUtility.BoardPath,
				Filter = "Board |*.brd"
			};

			DialogResult dialogResult = spf.ShowDialog();

			if (dialogResult != DialogResult.OK)
			{
				return;
			}
			string fileName = spf.FileName;

			Utility.SerializeUtility.SerializeBoard(game.board, fileName);

		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new() {
				InitialDirectory = Utility.FileUtility.BoardPath,
				Filter = "Board |*.brd"
			};

			DialogResult dialogResult = opf.ShowDialog();

			if (dialogResult != DialogResult.OK)
			{
				return;
			}
			string fileName = opf.FileName;
			Board board = Utility.SerializeUtility.DeserializeBoard(fileName);

			Global.CurrentSettings.BoardSize = board.BoardSize;
			NewGame(Global.CurrentSettings.GameMode,
				Global.CurrentSettings.BoardSize,
				Global.CurrentSettings.BotDepth,
				Global.CurrentSettings.ThemeEnum,
				board);
		}

		private void toolStripMenuItemOptionExit_Click(object sender, EventArgs e)
		{
			FormCustomMessageBox f2 = new() {
				Message = "Do you want to exit ?",
				ShowCancel = true,
				StartPosition = FormStartPosition.Manual,
				parentForm = this
			};
			f2.ShowDialogAtCenter();

			if (f2.DialogResult == DialogResult.Cancel)
			{
				return;
			}
			Application.Exit();

		}

		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormAbout f = new() {
				StartPosition = FormStartPosition.CenterParent
			};
			f.ShowDialog(this);

		}
	}
}
