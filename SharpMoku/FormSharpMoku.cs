using System;
using System.Drawing;
using System.Windows.Forms;

using SharpMoku.Logging;
using SharpMoku.UI;
using SharpMoku.UI.Theme;

namespace SharpMoku
{
	public partial class FormSharpMoku : Form, IUI
	{
		/* This class imeplement IUI
		 * It has UI.PictureBoxGomoku picGoMoku that responsible for rendering a board
		 * It has game object that use to bind between this class, game object and the board information
		 */
		public FormSharpMoku()
		{
			InitializeComponent();
		}

		private PictureBoxGomoku _picGoMoku = null;
		private Game _game = null;

		//AI.IEvaluate bot = new AI.EvaluateV2();
		private readonly AI.IEvaluate bot = new AI.EvaluateV3();

		public event EventHandler<PositionEventArgs> CellClicked;
		public event EventHandler HasFinishedMoveCursor;

		private void UpdateTheme(Theme theme)
		{

			_picGoMoku.Initial(theme);
			_picGoMoku.ReRender();
			ScaleBoard();
		}
		private void ScaleBoard()
		{
			//Scale the board to be bigger in case the board size is 9
			if (_game.Board.BoardSize == 9)
			{
				_picGoMoku.Visible = false;
				_picGoMoku.Scale(new SizeF(1.5f, 1.5f));
				_picGoMoku.Top = menuStrip1.Height + 5;
				_picGoMoku.Visible = true;
			}
		}
		// int iCountGameID = 0;

		private void NewGame(Game.GameModeEnum gameMode,
							 int boardSize,
							 int botSearchDepth,
							 ThemeFactory.ThemeEnum themeEnum) =>
				NewGame(gameMode, boardSize, botSearchDepth, themeEnum, null);

		private void CreateGame(Game.GameModeEnum gameMode, int boardSize, int botDepth, Board board)
		{
			_game?.ReleaseResource();

			_game = board != null
					   ? new Game(this, board, bot, botDepth, gameMode)
					   : new Game(this, boardSize, bot, botDepth, gameMode);

			_game.log = new SimpleLog(Utility.FileUtility.LogFilePath,
									 Common.CurrentSettings);

		}
		private void NewGame(Game.GameModeEnum gameMode,
							 int boardSize,
							 int botSearchDepth,
							 ThemeFactory.ThemeEnum themeEnum,
							 Board board)
		{

			Theme currentTheme = ThemeFactory.Create(themeEnum);
			CreateGame(gameMode, boardSize, botSearchDepth, board);

			const int cellWidth = 38;
			const int cellHeight = 38;

			// If it is the first time to loadboard or the boardSize is different
			// So we need to create a new picGoMoku
			if (_picGoMoku == null ||
					_picGoMoku.RowNumber != boardSize)
			{
				if (_picGoMoku != null)
				{
					_picGoMoku.CellClicked -= PicGoMoku_CellClicked;
					_picGoMoku.ReleaseResource();
					Controls.Remove(_picGoMoku);
				}
				_picGoMoku = new PictureBoxGomoku(_game.Board, cellWidth, cellHeight) {
					RowNumber = boardSize,
					ColumnNumber = boardSize,
					BorderStyle = BorderStyle.FixedSingle,

					Left = 0,
					Top = menuStrip1.Height + 5
				};
				_picGoMoku.Initial(currentTheme);
				_picGoMoku.CellClicked -= PicGoMoku_CellClicked;
				_picGoMoku.CellClicked += PicGoMoku_CellClicked;

			}
			else
			{
				//Using the old picGomoku
				//picGoMoku.Top = this.menuStrip1.Height + 5;
				_picGoMoku.SetBoard(_game.Board);
				_picGoMoku.Initial(currentTheme);
			}
			if (!Controls.Contains(_picGoMoku))
			{
				Controls.Add(_picGoMoku);
			}

			_game.NewGame();

			_picGoMoku.Visible = true;
			ScaleBoard();

			Height = _picGoMoku.Height + _picGoMoku.Top + menuStrip1.Height - 5;
			Width = _picGoMoku.Width + _picGoMoku.Left;

			RenderUI();
		}

		public void Game_BotFinishedThinking(object sender, EventArgs e)
		{

			IsBotThinking = false;
			Cursor = Cursors.Default;

		}

		private bool IsBotThinking = false;// If this value is true it will prevent user from click on the cell

		//private readonly Cursor tempCursor = Cursors.Default;
		public void Game_BotThinking(object sender, EventArgs e)
		{
			IsBotThinking = true;
			Cursor = Cursors.WaitCursor;
		}

		private void TShowPicGoMokue_Tick(object sender, EventArgs e)
		{

			_picGoMoku.Visible = true;
			// throw new NotImplementedException();
		}

		public void Game_GameFinished(object sender, EventArgs e)
		{
			IsBotThinking = false;
			//    this.Cursor = Cursors.Default;
			if (_game.GameState == Game.GameStateEnum.End)
			{
				/*
				bool isPlaywon =
					   (game.WinResult == Board.WinStatus.BlackWon && game.GameMode == Game.GameModeEnum.PlayerVsBot)
					|| (game.WinResult == Board.WinStatus.WhiteWon && game.GameMode == Game.GameModeEnum.BotVsPlayer);
					*/
				Game_GameFinishedV2(_game.WinResult);
				/*
				if (isPlaywon)
				{
					Game_GameFinishedV2(game.WinResult);
				}
				*/
			}
			return;

		}
		private void Game_GameFinishedV2(WinStatus winStatus)
		{
			// Deley a little bit before show the result.
			System.Threading.Thread.Sleep(300);

			string whiteTurn = "White";
			string blackTurn = "Black";

			switch (Common.CurrentSettings.ThemeEnum)
			{
				case ThemeFactory.ThemeEnum.TicTacToe1:
				case ThemeFactory.ThemeEnum.TicTacToe2:
				case ThemeFactory.ThemeEnum.TicTacToe3:
					whiteTurn = "X";
					blackTurn = "O";
					break;
				case ThemeFactory.ThemeEnum.TableTennis:
					blackTurn = "Orange";
					break;
				case ThemeFactory.ThemeEnum.Gomoku1:
					break;
				case ThemeFactory.ThemeEnum.Gomoku2:
					break;
				case ThemeFactory.ThemeEnum.Gomoku3:
					break;
				case ThemeFactory.ThemeEnum.Gomoku4:
					break;
				case ThemeFactory.ThemeEnum.Gomoku5:
					break;
				default:
					break;
			}
			string message;// = whiteTurn + " Won.";

			if (winStatus == WinStatus.NotDecidedYet)
			{
				return;
			}

			message = winStatus switch {
				WinStatus.BlackWon => $"{blackTurn} Won.",
				WinStatus.WhiteWon => $"{whiteTurn} Won.",
				WinStatus.Draw => " Draw.",
				WinStatus.NotDecidedYet => throw new NotImplementedException(),
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
			NewGame(Common.CurrentSettings.GameMode,
					Common.CurrentSettings.BoardSize,
					Common.CurrentSettings.BotDepth,
					Common.CurrentSettings.ThemeEnum);

		}

		private void FormSharpMoku_Load(object sender, EventArgs e)
		{
			Icon = Resource1.SharpMokuIcon;

			NewGame(Common.CurrentSettings.GameMode,
					Common.CurrentSettings.BoardSize,
					Common.CurrentSettings.BotDepth,
					Common.CurrentSettings.ThemeEnum);
			undoToolStripMenuItem.Enabled = Common.CurrentSettings.IsAllowUndo;

		}

		private void PicGoMoku_CellClicked(PictureBoxGomoku pic, PictureBoxGomoku.PositionEventArgs positionClick)
		{
			if (IsBotThinking)
			{
				return;
			}

			if (_game.GameState != Game.GameStateEnum.Playing)
			{
				return;
			}

			CellClicked?.Invoke(this, new(positionClick.Value));

		}

		public void RenderUI()
		{

			_picGoMoku.ReRender();
			Application.DoEvents();
			/*
			 * Without Application.DoEvents
			 * When player click on the cell, it will not be render immidately it will wait
			 * until the bot has put the cell also. So instead of when player put black, the game render black first
			 * the game just make the bot calculate its position immediately then render both white and black
			 *
			 * 1.First solution I tried to use Timer to delay the bot action, it works but it has
			 * the problem with UI thread because the thread created by the Timer cannot access UI thread
			 *
			 * 2. Second solution, do not delay anything just put Application.DoEvents();
			 */
		}

		public void MoveCursorTo(Position position)
		{
			if (!Common.CurrentSettings.IsUseBotMouseMove)
			{
				MouseAction_HasFinishedMoved(this, new EventArgs());
				return;
			}

			if (position.Row < 0 ||
				position.Col < 0 ||
				position.Row >= _game.Board.BoardSize ||
				position.Col >= _game.Board.BoardSize)
			{
				return;
			}

			Point ToPoint = _picGoMoku.GetLowerRightPoint(position);

			MoveCursor(ToPoint);

		}
		private void MoveCursor(Point ToPoint)
		{
			MoveCursor(Cursor.Position, ToPoint);
		}

		private readonly Random RandomGenerator = new();
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

			NewGame(Common.CurrentSettings.GameMode,
				Common.CurrentSettings.BoardSize,
				Common.CurrentSettings.BotDepth,
				Common.CurrentSettings.ThemeEnum);

		}

		private void toolStripMenuItemOption_Click(object sender, EventArgs e)
		{
			FormOption f = new();
			// f.Parent = this;
			f.ThemeChanged -= FThemeChanged;
			f.ThemeChanged += FThemeChanged;
			f.StartPosition = FormStartPosition.CenterParent;
			f.ShowDialog(this);
			if (f.DialogResult != DialogResult.OK)
			{
				return;
			}

			UpdateTheme(ThemeFactory.Create(Common.CurrentSettings.ThemeEnum));
			undoToolStripMenuItem.Enabled = Common.CurrentSettings.IsAllowUndo;

		}

		private void FThemeChanged(object sender, EventArgs e)
		{
			ThemeChangedEventArgs eChange = (ThemeChangedEventArgs)e;
			UpdateTheme(eChange.Theme);

		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_game == null ||
			   !_game.CanUndo ||
			   IsBotThinking)
			{
				return;
			}

			_game.Undo();
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

			Utility.SerializeUtility.Serialize(_game.Board, fileName);
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

			Board board = Utility.SerializeUtility.Deserialize<Board>(fileName);

			NewGame(Common.CurrentSettings.GameMode,
					Common.CurrentSettings.BoardSize,
					Common.CurrentSettings.BotDepth,
					Common.CurrentSettings.ThemeEnum,
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

			Utility.SerializeUtility.Serialize(_game.Board, fileName);

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
			Board board = Utility.SerializeUtility.Deserialize<Board>(fileName);

			Common.CurrentSettings.BoardSize = board.BoardSize;
			NewGame(Common.CurrentSettings.GameMode,
				Common.CurrentSettings.BoardSize,
				Common.CurrentSettings.BotDepth,
				Common.CurrentSettings.ThemeEnum,
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
