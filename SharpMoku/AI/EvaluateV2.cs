using System;
using System.Collections.Generic;
using System.Linq;

using SharpMoku.Board;

namespace SharpMoku.AI
{
	/*
     * https://blog.theofekfoundation.org/artificial-intelligence/2015/12/11/minimax-for-gomoku-connect-five/
     *
     */
	/* This class is not used anymore
     * We just keep it for studying a bread and butter of a 5 in a row evaluate function.
     */
	public sealed class EvaluateV2
		: IEvaluate
	{

		public double EvaluateBoard(Board.Board board, bool isMyTurn)
		{
			return CalculateBoardScoreForOpponent(board, isMyTurn);
		}

		public enum BlockType
		{
			ZeroBlock = 0,
			OneSideBlock = 1,
			BothSideBlock = 2
		}
		public readonly struct StonePattern(int numberOfconsecutiveStone, BlockType blockType)
		{
			public int NumberOfConsecutiveStones { get; } = numberOfconsecutiveStone;
			public BlockType BlockType { get; } = blockType;
		}
		public double CalculateBoardScoreForOpponent(Board.Board board, bool isMyTurn)
		{

			bool isItForMe = true;
			double myScore = GetScore(board, isItForMe, isMyTurn);
			double opponentScore = GetScore(board, !isItForMe, isMyTurn);

			if (myScore == 0)
			{
				myScore = 1.0;
			}
			return opponentScore / myScore;

		}

		public int GetScore(Board.Board board, bool isForBlackStone, bool isBlackTurn)
		{

			int[,] boardMatrix = board.Matrix;
			// Calculate score for each of the 3 directions

			int horizontalScore = CalculateScoreForHorizontal(boardMatrix, isForBlackStone, isBlackTurn);
			int verticalScore = CalculateScoreForVertical(boardMatrix, isForBlackStone, isBlackTurn);
			int diagonalScore = CalculateScoreForDiagonal(boardMatrix, isForBlackStone, isBlackTurn);

			return horizontalScore + verticalScore + diagonalScore;
		}

		public int GetScore(Board.Board board)
		{
			_ = board.LastPositionPut;

			CellValue cellValue = board.CurrentTurnCellValue;
			_ = (CellValue)
								   (-(int)cellValue);

			bool isBlackTurn = cellValue == CellValue.Black;

			int blackStoneValue = GetScore(board, isForBlackStone: true, isBlackTurn: isBlackTurn);
			int whiteStonevalue = GetScore(board, isForBlackStone: false, isBlackTurn: isBlackTurn);

			return cellValue == CellValue.Black ? blackStoneValue - whiteStonevalue : whiteStonevalue - blackStoneValue;

		}
		private readonly int leastStoneForConsecutive = 2;
		public List<StonePattern> GetStonePatternForHorizontal(int[,] boardMatrix, bool isForBlackStone)
		{
			List<StonePattern> listPattern = [];
			int numberOfconsecutiveStone = 0;

			BlockType blockType = BlockType.BothSideBlock;

			int targetcellValue = isForBlackStone
				? blackStoneCellvalue
				: whiteStoneCellvalue;

			// Row loop
			for (int indexRow = 0; indexRow < boardMatrix.GetLength(0); indexRow++)
			{
				// Column loop
				for (int indexColumn = 0; indexColumn < boardMatrix.GetLength(1); indexColumn++)
				{

					int currentCellValue = boardMatrix[indexRow, indexColumn];
					if (currentCellValue == targetcellValue)
					{

						numberOfconsecutiveStone++;
						continue;
					}

					if (currentCellValue == emptyCellvalue)
					{

						if (numberOfconsecutiveStone >= leastStoneForConsecutive)
						{

							blockType--;
							listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
							numberOfconsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					// Cell is occupied by opponent
					if (numberOfconsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
						numberOfconsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					// Current cell is occupied by opponent, next consecutive set may have 2 blocked sides
					blockType = BlockType.BothSideBlock;

				}
				// End of row, check if there were any consecutive stones before we reached right border
				if (numberOfconsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
					//score += getConsecutiveSetScore(numberOfconsecutiveStone, numberOfBlocks, isForBlackStone == isBlackTurn);
				}
				// Reset consecutive stone and blocks count
				numberOfconsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;
			}

			return listPattern;
		}

		public int CalculateScoreForHorizontal(int[,] boardMatrix, bool isForBlackstone, bool isBlackTurn)
			 => GetStonePatternForHorizontal(boardMatrix, isForBlackstone)
				.Sum(x => EvaluateV2.CalculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public int CalculateScoreForVertical(int[,] boardMatrix, bool isForBlackstone, bool isBlackTurn)
			=> GetStonePatternForVertical(boardMatrix, isForBlackstone)
				.Sum(x => EvaluateV2.CalculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public int CalculateScoreForDiagonal(int[,] boardMatrix, bool isForBlackStone, bool isBlackTurn)
			=> GetStonePatternForDiagonal(boardMatrix, isForBlackStone)
				.Sum(x => EvaluateV2.CalculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public List<StonePattern> GetStonePatternForVertical(int[,] boardMatrix, bool isForBlackstone)
		{

			int numberOfconsecutiveStone = 0;
			BlockType blockType = BlockType.BothSideBlock;
			int targetcellValue = isForBlackstone
	? blackStoneCellvalue
	: whiteStoneCellvalue;
			List<StonePattern> listPattern = [];
			for (int indexColumn = 0; indexColumn < boardMatrix.GetLength(1); indexColumn++)
			{
				for (int indexRow = 0; indexRow < boardMatrix.GetLength(0); indexRow++)
				{
					int currentCellValue = boardMatrix[indexRow, indexColumn];
					if (currentCellValue == targetcellValue)
					{
						numberOfconsecutiveStone++;
						continue;
					}
					if (currentCellValue == emptyCellvalue)
					{
						if (numberOfconsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
							numberOfconsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					if (numberOfconsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
						numberOfconsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}
				if (numberOfconsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));

				}
				numberOfconsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;

			}
			return listPattern;
		}
		private const int emptyCellvalue = 0;
		private const int blackStoneCellvalue = -1;
		private const int whiteStoneCellvalue = 1;

		public List<StonePattern> GetStonePatternForDiagonal(int[,] boardMatrix, bool isForBlackStone)
		{
			List<StonePattern> listPattern = [];
			int numberOfconsecutiveStone = 0;
			BlockType blockType = BlockType.BothSideBlock;

			int targetcellValue = isForBlackStone
? blackStoneCellvalue
: whiteStoneCellvalue;
			int opponentcellValue = isForBlackStone
				? whiteStoneCellvalue
				: blackStoneCellvalue;

			// From bottom-left to top-right diagonally
			// https://stackoverflow.com/questions/20420065/loop-diagonally-through-two-dimensional-array
			int lastIndex = 2 * (boardMatrix.GetLength(0) - 1);
			for (int indexSWtoNE = 0; indexSWtoNE <= lastIndex; indexSWtoNE++)
			{
				int iStart = Math.Max(0, indexSWtoNE - boardMatrix.GetLength(0) + 1);
				int iEnd = Math.Min(boardMatrix.GetLength(0) - 1, indexSWtoNE);
				for (int i = iStart; i <= iEnd; ++i)
				{
					int j = indexSWtoNE - i;

					if (boardMatrix[i, j] == targetcellValue)
					{
						numberOfconsecutiveStone++;
						continue;
					}

					if (boardMatrix[i, j] == emptyCellvalue)
					{
						if (numberOfconsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
							numberOfconsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					if (boardMatrix[i, j] != opponentcellValue)
					{
						throw new InvalidOperationException($"The cell value in position {i},{j} is {boardMatrix[i, j]} which is invalid, the valid value must be {blackStoneCellvalue} For blackStone, {whiteStoneCellvalue} for whiteStone, {blackStoneCellvalue} for blankcell");
					}

					if (numberOfconsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
						numberOfconsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}

				if (numberOfconsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));

				}
				numberOfconsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;
			}

			// From top-left to bottom-right diagonally
			lastIndex = boardMatrix.GetLength(0) - 1;
			blockType = BlockType.BothSideBlock;
			for (int indexNWtoSE = 1 - boardMatrix.GetLength(0); indexNWtoSE <= lastIndex; indexNWtoSE++)
			{
				int iStart = Math.Max(0, indexNWtoSE);
				int iEnd = Math.Min(boardMatrix.GetLength(0) + indexNWtoSE - 1, boardMatrix.GetLength(0) - 1);
				for (int i = iStart; i <= iEnd; ++i)
				{
					int j = i - indexNWtoSE;

					if (boardMatrix[i, j] == targetcellValue)
					{
						numberOfconsecutiveStone++;
						continue;
					}
					if (boardMatrix[i, j] == emptyCellvalue)
					{
						if (numberOfconsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
							numberOfconsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					if (numberOfconsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));
						numberOfconsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}
				if (numberOfconsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfconsecutiveStone, blockType));

				}
				numberOfconsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;
			}
			return listPattern;
		}

		//These readonly field are public because test cases need to access them

		public static readonly int wonScore = 50000000;
		public static readonly int confirmWinScore = 1000000;
#pragma warning disable CA1707 // Identifiers should not contain underscores
		public static readonly int ConsecutiveStone4_Not_MyTurn_0Block = 200000;
		public static readonly int ConsecutiveStone4_Not_MyTurn_MoreThan0Block = 300;

		public static readonly int ConsecutiveStone3_MyTurn_0Block = 50000;
		public static readonly int ConsecutiveStone3_MyTurn_MoreThan0Block = 20;

		public static readonly int ConsecutiveStone3_Not_MyTurn_0Block = 100;
		public static readonly int ConsecutiveStone3_Not_MyTurn_MoreThan0Block = 5;

		public static readonly int ConsecutiveStone2_MyTurn_0Block = 7;
		public static readonly int ConsecutiveStone2_MoreThan0Block = 3;
		public static readonly int ConsecutiveStone2_Not_MyTurn_0Block = 5;
		public static readonly int ConsecutiveStone1 = 1;
#pragma warning restore CA1707 // Identifiers should not contain underscores
		public static int CalculateConsecutiveStoneSequenceScore(StonePattern stonePattern, bool isMyTurn)
		{
			return CalculateConsecutiveStoneSequenceScore(stonePattern.NumberOfConsecutiveStones,
														  stonePattern.BlockType,
														  isMyTurn);
		}

		public static int CalculateConsecutiveStoneSequenceScore(int numberOfConsecutiveStones,
																 BlockType blockType,
																 bool isMyTurn)
		{

			return numberOfConsecutiveStones >= 5
				? wonScore
				: blockType == BlockType.BothSideBlock
				&& numberOfConsecutiveStones < 5
				? 0
				: numberOfConsecutiveStones switch {
					4 => isMyTurn
						? confirmWinScore
						: blockType == BlockType.ZeroBlock
							? ConsecutiveStone4_Not_MyTurn_0Block
							: ConsecutiveStone4_Not_MyTurn_MoreThan0Block,
					3 => blockType == BlockType.ZeroBlock
						? isMyTurn
							? ConsecutiveStone3_MyTurn_0Block
							: ConsecutiveStone3_Not_MyTurn_0Block
						: isMyTurn
							? ConsecutiveStone3_MyTurn_MoreThan0Block
							: ConsecutiveStone3_Not_MyTurn_MoreThan0Block,
					2 => blockType > BlockType.ZeroBlock
						? ConsecutiveStone2_MoreThan0Block
						: isMyTurn
							? ConsecutiveStone2_MyTurn_0Block
							: ConsecutiveStone2_Not_MyTurn_0Block,
					1 => ConsecutiveStone1,
					_ => 0,
				};
		}
	}
}
