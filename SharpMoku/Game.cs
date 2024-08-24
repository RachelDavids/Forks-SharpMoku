using System;

using SharpMoku.AI;
using SharpMoku.Board;
using SharpMoku.Logging;
using SharpMoku.UI;

namespace SharpMoku
{
	public sealed class Game
	{
		public Board.Board Board { get; private set; }
		private IUI _ui;
		public WinStatus WinResult { get; private set; } = WinStatus.NotDecidedYet;
		//public Turn TheWinner { get; private set; }
		public ILog Logger { get; set; }
		public GameMode GameMode { get; private set; } = GameMode.PlayerVsBot;
		public GameState GameState { get; private set; } = GameState.NotBegin;
		/*
         * GameFinished event to tell the UI to display the result
         * BotThinking to tell the UI to change the cursor to hourglass
         * BotFinishedThinking to tell the UI that now UI is allowed to accept user input
         */
		public event EventHandler<WinStatusEventArgs> GameFinished;
		public event EventHandler BotThinking;
		public event EventHandler BotFinishedThinking;

		// For ObjectID is for helping to debug purpose only
		// We don't need it for other purpose
		// so I commented it out
		//public int ObjectID { get; set; } = 0;
		private void ExplicitConstructor(UI.IUI ui,
										 Board.Board board,
										 int boardSize,
										 IEvaluate pbot,
										 int botSearchDepth,
										 GameMode gameMode)
		{
			_ui = ui;
			GameMode = gameMode;
			BotSearchDepth = botSearchDepth;
			if (pbot != null)
			{
				_bot = pbot;
			}
			WinResult = WinStatus.NotDecidedYet;

			_ui.CellClicked -= UI_CellClicked;
			_ui.HasFinishedMoveCursor -= UI_HasFinishedMoveCursor;
			GameFinished -= _ui.OnGameFinished;
			BotThinking -= _ui.OnBotThinking;
			BotFinishedThinking -= _ui.OnBotFinishedThinking;

			_ui.CellClicked += UI_CellClicked;
			_ui.HasFinishedMoveCursor += UI_HasFinishedMoveCursor;

			Board = board ?? new Board.Board(boardSize);

			//Cannnot use this event (Game_GameFinished)
			//Has the problem with threading
			GameFinished += _ui.OnGameFinished;
			BotThinking += _ui.OnBotThinking;
			BotFinishedThinking += _ui.OnBotFinishedThinking;

		}
		public Game(UI.IUI ui,
					Board.Board board,
					IEvaluate pbot,
					int botSearchDepth,
					GameMode gameMode)
		{

			ExplicitConstructor(ui, board, 0, pbot, botSearchDepth, gameMode);
			/*
            this.UI = ui;
            this.GameMode = gameMode;
            this.BotSearchDepth = botSearchDepth;
            if(pbot != null)
            {
                bot = pbot;
            }
            this.UI.CellClicked += UI_CellClicked;
            this.UI.HasFinishedMoveCursor += UI_HasFinishedMoveCursor;
            WinResult = Board.WinStatus.NotDecidedYet;
            this.board = new Board(board);
            */
		}
		public Game(UI.IUI ui,
					int boardSize,
					IEvaluate pbot,
					int botSearchDepth,
					GameMode gameMode
			)
		{
			ExplicitConstructor(ui, null, boardSize, pbot, botSearchDepth, gameMode);

		}
		public bool CanUndo => Board != null && Board.CanUndo;

		public void Undo()
		{

			if (GameMode == GameMode.PlayerVsPlayer)
			{
				Board.Undo();
				if (GameState == GameState.End)
				{
					Board.SwitchTurn();
				}
			}
			else
			{
				//If the gameMode is BotVsPlayer or PlayerVsBOT
				//We need to Undo 2 times
				//1 for bot another for player
				if (GameState == GameState.End)
				{
					bool isneedToDoubleUndo = false;
					if (GameMode == GameMode.BotVsPlayer)
					{
						if (WinResult == WinStatus.BlackWon)
						{
							isneedToDoubleUndo = true;

						}
					}
					else
					{
						if (WinResult == WinStatus.WhiteWon)
						{
							isneedToDoubleUndo = true;
						}
					}

					if (isneedToDoubleUndo)
					{
						Board.Undo();
						Board.Undo();
						Board.SwitchTurn();
					}
					else
					{
						Board.Undo();
						Board.SwitchTurn();
					}
				}
				else
				{
					Board.Undo();
					Board.Undo();
				}
			}
			WinResult = WinStatus.NotDecidedYet;
			GameState = GameState.Playing;
			_ui.RenderUI();

		}
		private void UI_HasFinishedMoveCursor(object sender, EventArgs e)
		{

			PutStone(_botMoveToPosition, (CellValue)Board.CurrentTurn);

		}
		public void PutStone(Position position)
		{
			PutStone(position, Board.CurrentTurnCellValue);
		}
		//public Boolean
		public void PutStone(Position position, CellValue turn)
		{

			Board.PutStone(position, turn);

			_ui.RenderUI();

			WinResult = Board.CheckWinStatus();

			if (WinResult == WinStatus.NotDecidedYet)
			{
				//[DEBUG:]
				Board.SwitchTurn();
				bool IsBotTurn = (GameMode == GameMode.PlayerVsBot && !IsPlayer1Turn) ||
								(GameMode == GameMode.BotVsPlayer && IsPlayer1Turn);
				if (IsBotTurn)
				{
					BotThinking?.Invoke(this, null);
					BotMove();
				}
				return;
			}

			GameState = GameState.End;
			WinStatusEventArgs statusEvent = new(WinResult);
			GameFinished?.Invoke(this, statusEvent);

		}

		//private System.Timers.Timer botTimer = new();

		public void NewGame()
		{
			GameState = GameState.Playing;

			if (GameMode == GameMode.BotVsPlayer)
			{
				// botTimer.Start();
				System.Threading.Thread.Sleep(20);
				BotMove();

			}
		}

		private bool IsPlayer1Turn => Board.CurrentTurn == Turn.Black;

		// This method being used by human only.
		private void UI_CellClicked(object o, PositionEventArgs positionClick)
		{
			bool isPlayerClickDespiteItisBotTurn = (GameMode == GameMode.PlayerVsBot && Board.CurrentTurn != Turn.Black) ||
													  (GameMode == GameMode.BotVsPlayer && Board.CurrentTurn != Turn.White);

			bool isClickedOnNonEmptyCell = Board.Matrix[positionClick.Value.Row, positionClick.Value.Col] != (int)CellValue.Empty;
			bool isClickedOInValidPosition = !Board.IsValidPosition(positionClick.Value);

			if (GameState != GameState.Playing
				|| isClickedOInValidPosition
				|| isPlayerClickDespiteItisBotTurn
				|| isClickedOnNonEmptyCell)
			{
				return;
			}

			/*
            if (!board.IsValidPosition(positionClick.Value))
            {
                return;
            }
               if (isPlayerClickDespiteItisBotTurn )
            {
                return;
            }

            if(board.Matrix[positionClick.Value.Row ,positionClick.Value.Col ] != (int)Board.CellValue.Empty)
            {
                return;
            }
            */

			PutStone(positionClick.Value, Board.CurrentTurnCellValue);

		}

		public void ReleaseResource()
		{
			if (_ui != null)
			{
				_ui.CellClicked -= UI_CellClicked;
				_ui.HasFinishedMoveCursor -= UI_HasFinishedMoveCursor;
				GameFinished -= _ui.OnGameFinished;
				BotThinking -= _ui.OnBotThinking;
				BotFinishedThinking -= _ui.OnBotFinishedThinking;
			}
			_ui = null;
		}
		public int BotSearchDepth { get; private set; } = 2;
		private Position _botMoveToPosition;
		private IEvaluate _bot = new EvaluateV3();
		private void BotMove()
		{
			Board.Board cloneBoard = new(Board);
			Minimax miniMax = new(cloneBoard, _bot, Logger);
			_botMoveToPosition = miniMax.CalculateNextMove(BotSearchDepth);
			BotFinishedThinking?.Invoke(this, EventArgs.Empty);
			_ui.MoveCursorTo(_botMoveToPosition);

		}
	}
}
