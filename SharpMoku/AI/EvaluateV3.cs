using System.Collections.Generic;

namespace SharpMoku.AI
{
	//Credit:https://codepen.io/mudrenok/pen/gpMXgg
	/* This EvaluateV3 algorhim was ported from the javascript by Anton Mudrenok
     * https://codepen.io/mudrenok
     */
	public sealed class EvaluateV3
		: IEvaluate
	{
		public struct NumberOfScorePattern
		{
			// TODO: These should be properties
			public int Winning = 0;
			public int Stone4 = 0;
			public int Stone3 = 0;
			public int Stone2 = 0;
			public int BlockStone4 = 0;
			public int BlockStone3 = 0;
			public NumberOfScorePattern()
			{

			}
			public NumberOfScorePattern(int winning,
										int stone4,
										int stone3,
										int stone2,
										int blockStone4,
										int blockStone3)
			{
				Winning = winning;
				Stone4 = stone4;
				Stone3 = stone3;
				Stone2 = stone2;
				BlockStone4 = blockStone4;
				BlockStone3 = blockStone3;

			}
			public readonly bool HasZeroValue => Winning == 0
												 && Stone4 == 0
												 && Stone3 == 0
												 && Stone2 == 0
												 && BlockStone4 == 0
												 && BlockStone3 == 0;
		}
		// Board instance is responsible for board mechanics

		// Win score should be greater than all possible board scores
		public const int WinScore = 1000000000;
		public const int WinGuarantee = WinScore / 10;

		private sealed class ListStonePattern
		{
			public static ListStonePattern Create(bool isForBlack)
			{
				ListStonePattern list = new();
				if (!isForBlack)
				{
					return list;
				}
				list.Stone2WithNoBLock = ConvertListValue(list.Stone2WithNoBLock);
				list.Stone3WithBlock = ConvertListValue(list.Stone3WithBlock);
				list.Stone3WithNoBlock = ConvertListValue(list.Stone3WithNoBlock);
				list.Stone4WithBlock = ConvertListValue(list.Stone4WithBlock);
				list.Stone4WithNoBlock = ConvertListValue(list.Stone4WithNoBlock);
				list.winPattern = ConvertListValue(list.winPattern);

				return list;
			}
			private static List<List<int>> ConvertListValue(List<List<int>> listInput)
			{
				int i;
				int j;
				for (i = 0; i < listInput.Count; i++)
				{
					for (j = 0; j < listInput[i].Count; j++)
					{
						listInput[i][j] = -listInput[i][j];
					}
				}
				return listInput;
			}
			public List<List<int>> winPattern = [[1, 1, 1, 1, 1]];
			public List<List<int>> Stone4WithNoBlock = [[0, 1, 1, 1, 1, 0]];
			public List<List<int>> Stone3WithNoBlock = [
						 [0, 1, 1, 1, 0, 0],
						[0, 0, 1, 1, 1, 0],
						[0, 1, 0, 1, 1, 0],
						[0, 1, 1, 0, 1, 0],

					];
			public List<List<int>> Stone2WithNoBLock = [
						[0, 0, 1, 1, 0, 0],
						[0, 1, 0, 1, 0, 0],
						[0, 0, 1, 0, 1, 0],
						[0, 1, 1, 0, 0, 0],
						[0, 0, 0, 1, 1, 0],
						[0, 1, 0, 0, 1, 0],
					];

			public List<List<int>> Stone4WithBlock = [
						[-1, 1, 0, 1, 1, 1],
						[-1, 1, 1, 0, 1, 1],
						[-1, 1, 1, 1, 0, 1],
						[-1, 1, 1, 1, 1, 0],
						[0, 1, 1, 1, 1, -1],
						[1, 0, 1, 1, 1, -1],
						[1, 1, 0, 1, 1, -1],
						[1, 1, 1, 0, 1, -1],

					];

			public List<List<int>> Stone3WithBlock = [
						 [-1, 1, 1, 1, 0, 0],
						 [-1, 1, 1, 0, 1, 0],
						 [-1, 1, 0, 1, 1, 0],
						 [0, 0, 1, 1, 1, -1],
						 [0, 1, 0, 1, 1, -1],
						 [0, 1, 1, 0, 1, -1],
						 [-1, 1, 0, 1, 0, 1, -1],
						 [-1, 0, 1, 1, 1, 0, -1],
						 [-1, 1, 1, 0, 0, 1, -1],
						 [-1, 1, 0, 0, 1, 1, -1]
					];
		}
		private ListStonePattern listStonePatternForWhite { get; } = ListStonePattern.Create(false);
		private ListStonePattern listStonePatternForBlack { get; } = ListStonePattern.Create(true);

		public NumberOfScorePattern ValuePosition(List<List<int>> listOfDirection, bool isForBlack)
		{

			NumberOfScorePattern scorePattern = new();
			int i;
			ListStonePattern listPattern = isForBlack
				? listStonePatternForBlack
				: listStonePatternForWhite;

			for (i = 0; i < listOfDirection.Count; i++)
			{
				List<int> list = listOfDirection[i];
				if (IsAnyInArrays(listPattern.winPattern, list))
				{
					scorePattern.Winning++;
					continue;
				}
				if (IsAnyInArrays(listPattern.Stone4WithNoBlock, list))
				{
					scorePattern.Stone4++;
					continue;
				}
				if (IsAnyInArrays(listPattern.Stone3WithNoBlock, list))
				{
					scorePattern.Stone3++;
					continue;
				}

				if (IsAnyInArrays(listPattern.Stone4WithBlock, list))
				{
					scorePattern.BlockStone4++;
					continue;
				}
				if (IsAnyInArrays(listPattern.Stone3WithBlock, list))
				{
					scorePattern.BlockStone3++;
					continue;
				}
				if (IsAnyInArrays(listPattern.Stone2WithNoBLock, list))
				{
					scorePattern.Stone2++;
					continue;
				}
			}
			return scorePattern;
		}
		public static bool IsAnyInArrays(List<List<int>> listPattern, List<int> listCellValue)
		{

			int z;
			int j;
			int i;
			for (z = 0; z < listPattern.Count; z++)
			{
				//
				int fCount = listCellValue.Count;
				int sCount = listPattern[z].Count;
				int k;
				for (i = 0; i <= fCount - sCount; i++)
				{
					k = 0;
					for (j = 0; j < sCount; j++)
					{
						if (listCellValue[i + j] == listPattern[z][j])
						{
							k++;
						}
						else
						{
							break;
						}
					}
					if (k == sCount)
					{
						return true;
					}
				}
			}
			return false;
		}
#pragma warning disable IDE0060 // Remove unused parameter
		public double EvaluateBoardForWhite(Board board, bool blacksTurn)
#pragma warning restore IDE0060 // Remove unused parameter
		{

			// Get board score of both players.

			// double blackScore = GetScore(board, true, blacksTurn);
			double whiteScore = GetScore(board);
			return whiteScore;
			/*
            if (blackScore == 0) blackScore = 1.0;

            // Calculate relative score of black against white
            if (blacksTurn)
            {
                return blackScore / whiteScore;
            }
            // Calculate relative score of white against black
            return whiteScore / blackScore;
            */
		}
		public List<List<int>> GetListAllDirection(Board board, Position checkPosition, CellValue cellValue)
		{
			Position positionDeltaNorthSouth = new(1, 0);
			Position positionDeltaWestEast = new(0, 1);
			Position positionDeltaNorthEast = new(1, 1);
			Position positionDeltaSouthWest = new(1, -1);

			/*
                   *Prepare to go though all 8 directions
                   * 4 have 4 lists of News because each list go both way
                   * For example NorthSouth mean from the position to north and from the postion to south
                   */
			List<int> listNorthSouth = GetCellValueInDirection(board.Matrix, cellValue, checkPosition, positionDeltaNorthSouth);
			List<int> listWestEast = GetCellValueInDirection(board.Matrix, cellValue, checkPosition, positionDeltaWestEast);
			List<int> listNorthEast = GetCellValueInDirection(board.Matrix, cellValue, checkPosition, positionDeltaNorthEast);
			List<int> listSouthWest = GetCellValueInDirection(board.Matrix, cellValue, checkPosition, positionDeltaSouthWest);

			List<List<int>> listAllDirection =
			[
				listNorthSouth ,
				listWestEast ,
				listNorthEast ,
				listSouthWest
			];

			return listAllDirection;
		}

		//GetDifferenctScorefromLastPosition
		public int Heuristic(Board newBoard)
		{
			// GetPlayerscore from latest position
			Position LatestPosition = newBoard.LastPositionPut;

			CellValue PlayercellValue = (CellValue)newBoard.Matrix[LatestPosition.Row, LatestPosition.Col];
			List<List<int>> listAreaCheckForPlayer = GetListAllDirection(newBoard, LatestPosition, PlayercellValue);

			bool isForBlack = PlayercellValue == CellValue.Black;

			NumberOfScorePattern scorePattern = ValuePosition(listAreaCheckForPlayer, isForBlack);
			int Player1Value = getScoreByPattern(scorePattern);

			// Put the opponenetcell value into the latest postion.
			// Then get opponenet score
			// to see the differnent
			CellValue opponentCellValue = (CellValue)
											  (-(int)PlayercellValue);
			newBoard.Matrix[LatestPosition.Row, LatestPosition.Col] = (int)opponentCellValue;

			List<List<int>> listAreaCheckForOpponent = GetListAllDirection(newBoard, LatestPosition, opponentCellValue);

			isForBlack = opponentCellValue == CellValue.Black;
			NumberOfScorePattern enemyScorePattern = ValuePosition(listAreaCheckForOpponent, isForBlack);
			int EnemyValue = getScoreByPattern(enemyScorePattern);

			//After get Score for opponenet
			//Set it back to player cell value
			newBoard.Matrix[LatestPosition.Row, LatestPosition.Col] = (int)PlayercellValue;

			return (2 * Player1Value) + EnemyValue;
			//return 0;
		}

		public List<int> GetCellValueInDirection(int[,] matrix, CellValue cellValue, Position positionCheck, Position positionDelta)
		{
			int i;
			List<int> listCell = [];
			bool IsCheckPostionIsNotmatchWithCellValue = matrix[positionCheck.Row, positionCheck.Col] != (int)cellValue;
			HashSet<string> hshCellInaRow = [];
			if (IsCheckPostionIsNotmatchWithCellValue)
			{
				return listCell;
			}
			int opponentCellvalue = -(int)cellValue;
			//First loop Insert cell
			for (i = 1; i < 5; i++)
			{
				Position nextPosition = new(positionCheck.Row - (positionDelta.Row * i),
													positionCheck.Col - (positionDelta.Col * i));
				if (nextPosition.Row < 0 ||
					nextPosition.Row >= matrix.GetLength(0) ||
					nextPosition.Col < 0 ||
					nextPosition.Col >= matrix.GetLength(0))
				{
					break;
				}

				int nextValue = matrix[nextPosition.Row, nextPosition.Col];
				if (!hshCellInaRow.Contains(nextPosition.PositionString()))
				{
					listCell.Insert(0, nextValue); //We insert at the 0 position
				}

				if (nextValue == opponentCellvalue)
				{

					break;
				}
			}
			listCell.Add((int)cellValue); //The cell itself

			//Add
			for (i = 1; i < 5; i++)
			{
				Position nextPosition = new(positionCheck.Row + (positionDelta.Row * i),
													positionCheck.Col + (positionDelta.Col * i));
				if (nextPosition.Row < 0 ||
					nextPosition.Row >= matrix.GetLength(0) ||
					nextPosition.Col < 0 ||
					nextPosition.Col >= matrix.GetLength(0))
				{
					break;
				}
				int nextValue = matrix[nextPosition.Row, nextPosition.Col];

				if (!hshCellInaRow.Contains(nextPosition.PositionString()))
				{
					listCell.Add(nextValue);//We add it to the last position
				}
				if (nextValue == opponentCellvalue)
				{
					// listCell.Insert(0, nextValue);
					break;
				}
				//listCell.Insert(0, nextValue);
			}
			return listCell;
		}
		public int GetScore(Board board)
		{
			int Score = Heuristic(board);
			return Score;

		}
		public int getScoreByPattern(NumberOfScorePattern numberOfPattern)
		{
			if (numberOfPattern.Winning > 0)
			{
				return WinScore * numberOfPattern.Winning;
			}
			if (numberOfPattern.Stone4 > 0)
			{
				return WinGuarantee;

			}

			if (numberOfPattern.BlockStone4 > 1)
			{
				return WinGuarantee / 10;
			}
			if (numberOfPattern.Stone3 > 0
				&& numberOfPattern.BlockStone4 > 0)
			{
				return WinGuarantee / 100;
			}
			if (numberOfPattern.Stone3 > 1)
			{
				return WinGuarantee / 1000;
			}

			if (numberOfPattern.Stone3 == 1)
			{
				return numberOfPattern.Stone2 switch {
					3 => 40000,
					2 => 38000,
					1 => 35000,
					_ => 3450,
				};
			}

			if (numberOfPattern.BlockStone4 == 1)
			{
				return numberOfPattern.Stone2 switch {
					3 => 4500,
					2 => 4200,
					1 => 4100,
					_ => 4050,
				};
			}

			switch (numberOfPattern.BlockStone3)
			{
				case 3:
					if (numberOfPattern.Stone2 == 1)
					{
						return 2800;
					}

					break;
				case 2:
					switch (numberOfPattern.Stone2)
					{
						case 2: return 3000;
						case 1: return 2900;
						default:
							break;
					}
					break;
				case 1:
					switch (numberOfPattern.Stone2)
					{
						case 3: return 3400;
						case 2: return 3300;
						case 1: return 3100;
						default:
							break;
					}
					break;
				default:
					break;
			}

			return numberOfPattern.Stone2 switch {
				4 => 2700,
				3 => 2500,
				2 => 2000,
				1 => 1000,
				_ => 0,
			};
		}

		public double EvaluateBoard(Board board, bool IsMyturn)
		{
			return EvaluateBoardForWhite(board, IsMyturn);
		}
	}
}
