using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMoku.AI
{
	/*
     * https://blog.theofekfoundation.org/artificial-intelligence/2015/12/11/minimax-for-gomoku-connect-five/
     *
     */
	/* This class is not used anymore
     * We just keep it for studying a brade and butter of a 5 in a row evaluate function.
     */
	public sealed class EvaluateV2
		: IEvaluate
	{

		public double EvaluateBoard(Board board, bool isMyTurn)
		{
			return CalculateBoardScoreForOpponent(board, isMyTurn);
		}

		public enum BlockType
		{
			ZeroBlock = 0,
			OneSideBlock = 1,
			BothSideBlock = 2
		}
		public readonly struct StonePattern(int numberOfConsecutiveStone, BlockType blockType)
		{
			public readonly int NumberOfConsecutiveStone = numberOfConsecutiveStone;
			public readonly BlockType BlockType = blockType;
		}
		public double CalculateBoardScoreForOpponent(Board board, bool isMyTurn)
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

		public int GetScore(Board board, bool isForBlackStone, bool isBlackTurn)
		{

			int[,] boardMatrix = board.Matrix;
			// Calculate score for each of the 3 directions

			int horizontalScore = CalculateScoreForHorizontal(boardMatrix, isForBlackStone, isBlackTurn);
			int verticalScore = CalculateScoreForVertical(boardMatrix, isForBlackStone, isBlackTurn);
			int diagonalScore = CalculateScoreForDiagonal(boardMatrix, isForBlackStone, isBlackTurn);

			return horizontalScore + verticalScore + diagonalScore;
		}

		public int GetScore(Board board)
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
			int numberOfConsecutiveStone = 0;

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

						numberOfConsecutiveStone++;
						continue;
					}

					if (currentCellValue == emptyCellvalue)
					{

						if (numberOfConsecutiveStone >= leastStoneForConsecutive)
						{

							blockType--;
							listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
							numberOfConsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					// Cell is occupied by opponent
					if (numberOfConsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
						numberOfConsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					// Current cell is occupied by opponent, next consecutive set may have 2 blocked sides
					blockType = BlockType.BothSideBlock;

				}
				// End of row, check if there were any consecutive stones before we reached right border
				if (numberOfConsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
					//score += getConsecutiveSetScore(numberOfConsecutiveStone, numberOfBlocks, isForBlackStone == isBlackTurn);
				}
				// Reset consecutive stone and blocks count
				numberOfConsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;
			}

			return listPattern;
		}

		public int CalculateScoreForHorizontal(int[,] boardMatrix, bool isForBlackstone, bool isBlackTurn)
			 => GetStonePatternForHorizontal(boardMatrix, isForBlackstone)
				.Sum(x => calculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public int CalculateScoreForVertical(int[,] boardMatrix, bool isForBlackstone, bool isBlackTurn)
			=> GetStonePatternForVertical(boardMatrix, isForBlackstone)
				.Sum(x => calculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public int CalculateScoreForDiagonal(int[,] boardMatrix, bool isForBlackStone, bool isBlackTurn)
			=> GetStonePatternForDiagonal(boardMatrix, isForBlackStone)
				.Sum(x => calculateConsecutiveStoneSequenceScore(x, isBlackTurn));

		public List<StonePattern> GetStonePatternForVertical(int[,] boardMatrix, bool isForBlackstone)
		{

			int numberOfConsecutiveStone = 0;
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
						numberOfConsecutiveStone++;
						continue;
					}
					if (currentCellValue == emptyCellvalue)
					{
						if (numberOfConsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
							numberOfConsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					if (numberOfConsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
						numberOfConsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}
				if (numberOfConsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));

				}
				numberOfConsecutiveStone = 0;
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
			int numberOfConsecutiveStone = 0;
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
						numberOfConsecutiveStone++;
						continue;
					}

					if (boardMatrix[i, j] == emptyCellvalue)
					{
						if (numberOfConsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
							numberOfConsecutiveStone = 0;
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

					if (numberOfConsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
						numberOfConsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}

				if (numberOfConsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));

				}
				numberOfConsecutiveStone = 0;
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
						numberOfConsecutiveStone++;
						continue;
					}
					if (boardMatrix[i, j] == emptyCellvalue)
					{
						if (numberOfConsecutiveStone >= leastStoneForConsecutive)
						{
							blockType--;
							listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
							numberOfConsecutiveStone = 0;
							blockType = BlockType.OneSideBlock;
						}
						else
						{
							blockType = BlockType.OneSideBlock;
						}
						continue;
					}
					if (numberOfConsecutiveStone >= leastStoneForConsecutive)
					{
						listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));
						numberOfConsecutiveStone = 0;
						blockType = BlockType.BothSideBlock;
						continue;
					}

					blockType = BlockType.BothSideBlock;

				}
				if (numberOfConsecutiveStone >= leastStoneForConsecutive)
				{
					listPattern.Add(new StonePattern(numberOfConsecutiveStone, blockType));

				}
				numberOfConsecutiveStone = 0;
				blockType = BlockType.BothSideBlock;
			}
			return listPattern;
		}

		//These readonly field are public because test cases need to access them

		public static readonly int wonScore = 50000000;
		public static readonly int confirmWinScore = 1000000;
		public static readonly int conSecutiveStone4_Not_MyTurn_0Block = 200000;
		public static readonly int conSecutiveStone4_Not_MyTurn_MoreThan0Block = 300;

		public static readonly int conSecutiveStone3_MyTurn_0Block = 50000;
		public static readonly int conSecutiveStone3_MyTurn_MoreThan0Block = 20;

		public static readonly int conSecutiveStone3_Not_MyTurn_0Block = 100;
		public static readonly int conSecutiveStone3_Not_MyTurn_MoreThan0Block = 5;

		public static readonly int conSecutiveStone2_MyTurn_0Block = 7;
		public static readonly int conSecutiveStone2_MoreThan0Block = 3;
		public static readonly int conSecutiveStone2_Not_MyTurn_0Block = 5;
		public static readonly int conSecutiveStone1 = 1;
		public int calculateConsecutiveStoneSequenceScore(StonePattern stonePattern, bool isMyTurn)
		{
			return calculateConsecutiveStoneSequenceScore(stonePattern.NumberOfConsecutiveStone,
				stonePattern.BlockType,
				isMyTurn);
		}

		public int calculateConsecutiveStoneSequenceScore(int numberOfConsecutiveStone, BlockType blockType, bool isMyTurn)
		{

			if (numberOfConsecutiveStone >= 5)
			{
				return wonScore;
			}

			if (blockType == BlockType.BothSideBlock
				&& numberOfConsecutiveStone < 5)
			{
				return 0;
			}

			switch (numberOfConsecutiveStone)
			{
				case 4:
					{

						return isMyTurn
							? confirmWinScore
							: blockType == BlockType.ZeroBlock ? conSecutiveStone4_Not_MyTurn_0Block : conSecutiveStone4_Not_MyTurn_MoreThan0Block;
					}
				case 3:
					{

						return blockType == BlockType.ZeroBlock
							? isMyTurn ? conSecutiveStone3_MyTurn_0Block : conSecutiveStone3_Not_MyTurn_0Block
							: isMyTurn ? conSecutiveStone3_MyTurn_MoreThan0Block : conSecutiveStone3_Not_MyTurn_MoreThan0Block;
					}
				case 2:
					{

						return blockType > BlockType.ZeroBlock
							? conSecutiveStone2_MoreThan0Block
							: isMyTurn ? conSecutiveStone2_MyTurn_0Block : conSecutiveStone2_Not_MyTurn_0Block;
					}
				case 1:
					{
						return conSecutiveStone1;
					}

				default:
					break;
			}

			return 0;

		}
	}
}
