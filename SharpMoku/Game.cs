using System;

using SharpMoku.AI;

namespace SharpMoku
{
	public class Game
	{
		public Board board = null;
		private UI.IUI UI = null;
		public enum GameStateEnum
		{
			NotBegin,
			Playing,
			End
		}
		public enum GameModeEnum
		{
			PlayerVsBot = 0,
			BotVsPlayer = 1,
			PlayerVsPlayer = 2,

		}
		public WinStatus WinResult { get; private set; } = WinStatus.NotDecidedYet;
		public Turn TheWinner { get; private set; }
		public ILog log = null;
		public GameModeEnum GameMode { get; private set; } = GameModeEnum.PlayerVsBot;
		public GameStateEnum GameState { get; private set; } = GameStateEnum.NotBegin;
		/*
         * GameFinished event to tell the UI to display the result
         * BotThinking to tell the UI to change the cursor to hourglass
         * BotFinishedThinking to tell the UI that now UI is allowed to accept user input
         */
		public event EventHandler GameFinished;
		public event EventHandler BotThinking;
		public event EventHandler BotFinishedThinking;

		public class WinStatusEventArgs : EventArgs
		{
			public WinStatus Winstatus { get; set; }
			public WinStatusEventArgs(WinStatus winStatus)
			{
				Winstatus = winStatus;
			}
		}
		// For ObjectID is for helping to debug purpose only
		// We don't need it for other purpose
		// so I commented it out
		//public int ObjectID { get; set; } = 0;
		private void ExplicitConstructor(UI.IUI ui,
										 Board board,
										 int boardSize,
										 IEvaluate pbot,
										 int botSearchDepth,
										 GameModeEnum gameMode)
		{
			UI = ui;
			GameMode = gameMode;
			BotSearchDepth = botSearchDepth;
			if (pbot != null)
			{
				bot = pbot;
			}
			WinResult = WinStatus.NotDecidedYet;

			UI.CellClicked -= UI_CellClicked;
			UI.HasFinishedMoveCursor -= UI_HasFinishedMoveCursor;
			GameFinished -= UI.Game_GameFinished;
			BotThinking -= UI.Game_BotThinking;
			BotFinishedThinking -= UI.Game_BotFinishedThinking;

			UI.CellClicked += UI_CellClicked;
			UI.HasFinishedMoveCursor += UI_HasFinishedMoveCursor;

			this.board = board ?? new Board(boardSize);

			//Cannnot use this event (Game_GameFinished)
			//Has the problem with threading
			GameFinished += UI.Game_GameFinished;
			BotThinking += UI.Game_BotThinking;
			BotFinishedThinking += UI.Game_BotFinishedThinking;

		}
		public Game(UI.IUI ui,
					Board board,
					IEvaluate pbot,
					int botSearchDepth,
					GameModeEnum gameMode)
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
					GameModeEnum gameMode
			)
		{
			ExplicitConstructor(ui, null, boardSize, pbot, botSearchDepth, gameMode);

		}
		public bool CanUndo => board != null && board.CanUndo;

		public void Undo()
		{

			if (GameMode == GameModeEnum.PlayerVsPlayer)
			{
				board.Undo();
				if (GameState == GameStateEnum.End)
				{
					board.SwitchTurn();
				}
			}
			else
			{
				//If the gameMode is BotVsPlayer or PlayerVsBOT
				//We need to Undo 2 times
				//1 for bot another for player
				if (GameState == GameStateEnum.End)
				{
					bool isneedToDoubleUndo = false;
					if (GameMode == GameModeEnum.BotVsPlayer)
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
						board.Undo();
						board.Undo();
						board.SwitchTurn();
					}
					else
					{
						board.Undo();
						board.SwitchTurn();
					}
				}
				else
				{
					board.Undo();
					board.Undo();
				}
			}
			WinResult = WinStatus.NotDecidedYet;
			GameState = GameStateEnum.Playing;
			UI.RenderUI();

		}
		private void UI_HasFinishedMoveCursor(object sender, EventArgs e)
		{

			PutStone(botMoveToPostion, (CellValue)board.CurrentTurn);

		}
		public void PutStone(Position position)
		{
			PutStone(position, board.CurrentTurnCellValue);
		}
		//public Boolean
		public void PutStone(Position position, CellValue turn)
		{

			board.PutStone(position, turn);

			UI.RenderUI();

			WinResult = board.CheckWinStatus();

			if (WinResult == WinStatus.NotDecidedYet)
			{
				//[DEBUG:]
				board.SwitchTurn();
				bool IsBotTurn = (GameMode == GameModeEnum.PlayerVsBot && !IsPlayer1Turn) ||
								(GameMode == GameModeEnum.BotVsPlayer && IsPlayer1Turn);
				if (IsBotTurn)
				{
					BotThinking?.Invoke(this, null);
					BotMove();
				}
				return;
			}

			GameState = GameStateEnum.End;
			WinStatusEventArgs statusEvent = new(WinResult);
			GameFinished?.Invoke(this, statusEvent);

		}

		private System.Timers.Timer botTimer = new();

		public void NewGame()
		{
			GameState = GameStateEnum.Playing;

			if (GameMode == GameModeEnum.BotVsPlayer)
			{
				// botTimer.Start();
				System.Threading.Thread.Sleep(20);
				BotMove();

			}
		}

		private bool IsPlayer1Turn => board.CurrentTurn == Turn.Black;

		// This method being used by human only.
		private void UI_CellClicked(object o, PositionEventArgs positionClick)
		{
			bool isPlayerClickDespiteItisBotTurn = (GameMode == GameModeEnum.PlayerVsBot && board.CurrentTurn != Turn.Black) ||
													  (GameMode == GameModeEnum.BotVsPlayer && board.CurrentTurn != Turn.White);

			bool isClickedOnNonEmptyCell = board.Matrix[positionClick.Value.Row, positionClick.Value.Col] != (int)CellValue.Empty;
			bool isClickedOInValidPosition = !board.IsValidPosition(positionClick.Value);

			if (GameState != GameStateEnum.Playing
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

			PutStone(positionClick.Value, board.CurrentTurnCellValue);

		}

		public void ReleaseResource()
		{
			if (UI != null)
			{
				UI.CellClicked -= UI_CellClicked;
				UI.HasFinishedMoveCursor -= UI_HasFinishedMoveCursor;
				GameFinished -= UI.Game_GameFinished;
				BotThinking -= UI.Game_BotThinking;
				BotFinishedThinking -= UI.Game_BotFinishedThinking;
			}
			UI = null;
		}
		public int BotSearchDepth { get; private set; } = 2;
		private Position botMoveToPostion;
		private IEvaluate bot = new EvaluateV3();
		private void BotMove()
		{

			SharpMoku.Board cloneBoard = new(board);

			Minimax miniMax = new(cloneBoard, bot, log);

			botMoveToPostion = miniMax.calculateNextMove(BotSearchDepth);
			BotFinishedThinking?.Invoke(this, null);
			UI.MoveCursorTo(botMoveToPostion);

		}
	}
}
