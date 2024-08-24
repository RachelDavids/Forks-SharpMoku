using System;
using System.Collections.Generic;
using System.Text;

using SharpMoku.Logging;

namespace SharpMoku.AI
{
	public class Minimax(Board board, IEvaluate evaluator, ILog logger)
	{
		private void Log(string message)
		{
			logger?.Log("MiniMaxNew::" + message);

		}

		public int NumberOfNodes = 0;
		private int evaluationCount = 0;

		private const int CONST_winScore = 10000000;
		public Position calculateNextMove(int depth)
		{
			int minAcceptDepth = 1;
			int maxAcceptDepth = 6;
			if (depth < minAcceptDepth || depth > maxAcceptDepth)
			{
				string message = $"depth value is {depth} which is invalid, program support the depth value between {minAcceptDepth} and {maxAcceptDepth}";
				throw new ArgumentOutOfRangeException(message);
			}

			MoveScore botMoveScore = new();

			if (board.IsEmpty)
			{
				botMoveScore.Row = Utility.Randomizer.GetRandomNumber(0, board.Matrix.GetUpperBound(0));
				botMoveScore.Col = Utility.Randomizer.GetRandomNumber(0, board.Matrix.GetUpperBound(1));
				//bestMove.Row =
				evaluationCount = 0;
				return botMoveScore.GetPosition();
			}

			MoveScore botWinningPosition = searchBotWinningPosition(board, _evaluator);

			if (botWinningPosition.Row != -1)
			{
				evaluationCount = 0;
				return botWinningPosition.GetPosition();
			}

			MoveScore OpponentWinningPosition = searchOpponentWinningPosition(board, _evaluator);

			if (OpponentWinningPosition.Row != -1)
			{
				evaluationCount = 0;
				return OpponentWinningPosition.GetPosition();

			}

			// If there is no such move, search the minimax tree with specified depth.
			//  Boolean IsMax = true;
			//  double Alpha = -1.0;
			NumberOfNodes = 0;
			MiniMaxParameter Para = new() {
				Depth = depth,
				Board = board.Clone(),
				IsMax = true,
				Alpha = -1.0,
				Beta = CONST_winScore
			};
			Log("Before search");
			_numberOfNodeInEachLevel = [];
			int i;
			for (i = 0; i <= depth; i++)
			{
				_numberOfNodeInEachLevel.Add(0);
			}

			FirstLevelDepth = Para.Depth;
			botMoveScore = minimaxSearchAlphaBeta(Para.Depth, Para.Board, Para.IsMax, Para.Alpha, Para.Beta);

			Log("  Depth::" + Para.Depth);
			Log("  Total No of Nodes::" + NumberOfNodes);

			if (botMoveScore.Row == -1 ||
				botMoveScore.Col == -1)
			{
				Log(" Cannot find any good postion using minimaxSerchAB");
				Log(" So we decide to use any position");

			}
			for (i = 0; i <= depth; i++)
			{
				Log("    Nodes[" + i + "] :" + _numberOfNodeInEachLevel[i]);
			}

			Log($"Score::{botMoveScore.Score}");
			Log($"Postion:: {botMoveScore.GetPosition().PositionString()}");
			return botMoveScore.GetPosition();
		}
		private int FirstLevelDepth = -1;

		/*
         * alpha : Best AI Move (Max)
         * beta : Best Player Move (Min)
         * returns: {score, move[0], move[1]}
         * */
		private sealed class MiniMaxParameter
		{
			public int Depth { get; set; }
			public Board Board { get; set; }
			public bool IsMax { get; set; }
			public double Alpha = 0;
			public double Beta = 0;
			//public MiniMaxParameter Clone()
			//{
			//	return new() {
			//		Depth = Depth,
			//		Board = Board.Clone(),
			//		IsMax = IsMax,
			//		Alpha = Alpha,
			//		Beta = Beta
			//	};
			//}
		}

		private readonly IEvaluate _evaluator = evaluator ?? new EvaluateV3();
		private List<int> _numberOfNodeInEachLevel = null;
		private static string GetTab(int moveDepth)
		{
			int maxDepth = 5;
			int numberOfTab = maxDepth - moveDepth;
			StringBuilder strB = new();
			int i;
			for (i = 0; i < numberOfTab; i++)
			{
				strB.Append("\t");
			}
			return strB.ToString();
		}
		private MoveScore minimaxSearchAlphaBeta(int depth, Board board, bool IsMax, double AlphaValue, double BetaValue)
		{
			NumberOfNodes++;
			_numberOfNodeInEachLevel[depth]++;
			// Last depth (terminal node), evaluate the current board score.
			string tabString = Minimax.GetTab(depth);
			Log($"{tabString}depth{depth}");

			if (depth == 0)
			{

				MoveScore moveScore = new(_evaluator.EvaluateBoard(board, !IsMax));
				Log($"{tabString}Evaluate happens here");
				Log($"{tabString}Score::{moveScore.Score}");

				return moveScore;
			}

			/*If it is first level, the radiusNeighbor can be 2
             * because it will not have too much node.
            */
			int radiusNeighbour = (depth == FirstLevelDepth)
				? 2
				: 1;
			List<Position> allNeighborPossibleMoves;// = null;
			if (radiusNeighbour == 2)
			{
				allNeighborPossibleMoves = board.generateNeighbourMoves(radiusNeighbour);
				if (allNeighborPossibleMoves.Count > 30)
				{
					allNeighborPossibleMoves = board.generateNeighbourMoves(1);
				}
			}
			else
			{
				allNeighborPossibleMoves = board.generateNeighbourMoves(1);
			}

			// If there is no possible move left, treat this node as a terminal node and return the score.
			bool isNothingLeftToSearch = allNeighborPossibleMoves.Count == 0;

			if (isNothingLeftToSearch)
			{
				MoveScore moveScore = new(_evaluator.EvaluateBoard(board, !IsMax));
				return moveScore;
			}

			/*If we reach this stage it means
             * There are valid moves
             */

			MoveScore bestMove = new();
			int depthChild = depth - 1;
			bool isMaxChild = !IsMax;
			bestMove.Row = allNeighborPossibleMoves[0].Row;
			bestMove.Col = allNeighborPossibleMoves[0].Col;
			bestMove.Score = IsMax
							? Int32.MinValue
							: Int32.MaxValue;
			int iCountMove = 0;
			Log($"{tabString}No of neighbor::{allNeighborPossibleMoves.Count}");
			foreach (Position move in allNeighborPossibleMoves)
			{

				iCountMove++;
				Log($"{tabString}{iCountMove}.   move::{move.PositionString()}");
				board.PutStoneAndSwitchTurn(move);
				MoveScore moveScore = minimaxSearchAlphaBeta(depthChild, board, isMaxChild, AlphaValue, BetaValue);
				moveScore.Row = move.Row;
				moveScore.Col = move.Col;

				Log($"{tabString}Score::{moveScore.Score}");
				//  board.Undo();

				if (board.IsFull)
				{
					Log("{tabString}board.IsFull");
					return moveScore;

				}
				board.Undo();

				if (IsMax)
				{
					AlphaValue = Math.Max(moveScore.Score, AlphaValue);
					if (moveScore.Score >= BetaValue)
					{
						Log($"{tabString}moveScoe >= Beta");
						return moveScore;
					}
					bestMove = MoveScore.Max(bestMove, moveScore);

				}
				else
				{
					BetaValue = Math.Min(moveScore.Score, BetaValue);
					if (moveScore.Score > AlphaValue)
					{
						Log($"{tabString}moveScore > Alpha");
						return moveScore;
					}
					bestMove = MoveScore.Min(bestMove, moveScore);

				}
			}
			return bestMove;

		}

		public MoveScore searchBotWinningPosition(Board board, IEvaluate bot)
		{

			List<Position> allPossibleMoves = board.generateNeighbourMoves();
			MoveScore winningPosition = new() {
				Score = -1,
				Row = -1,
				Col = -1
			};

			MoveScore maxMoveScore = new(Int32.MinValue, -1, -1);

			foreach (Position move in allPossibleMoves)
			{
				evaluationCount++;
				Board tempBoard = new(board);

				tempBoard.PutStone(move);
				int Score = bot.GetScore(tempBoard);

				if (Score > maxMoveScore.Score)
				{

					maxMoveScore = new MoveScore(Score, move.Row, move.Col);
				}
			}

			return maxMoveScore.Score >= CONST_winScore ? maxMoveScore : winningPosition;
		}

		public MoveScore searchOpponentWinningPosition(Board board, IEvaluate bot)
		{

			List<Position> allPossibleMoves = board.generateNeighbourMoves();
			MoveScore winningPosition = new() {
				Score = -1,
				Row = -1,
				Col = -1
			};

			MoveScore maxMoveScore = new(Int32.MinValue, -1, -1);

			foreach (Position move in allPossibleMoves)
			{
				evaluationCount++;

				Board tempBoard = new(board);

				tempBoard.SwitchTurn();
				tempBoard.PutStone(move);
				int Score = bot.GetScore(tempBoard);

				if (Score > maxMoveScore.Score)
				{
					maxMoveScore = new MoveScore(Score, move.Row, move.Col);
				}
			}
			if (evaluationCount > Int16.MaxValue)
			{
				// TODO: this is just a placeholder to see what we need to  (if anything)
			}
			return maxMoveScore.Score >= 10000 ? maxMoveScore : winningPosition;
		}
	}
}
