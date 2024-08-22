using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharpMoku;
using SharpMoku.AI;

namespace Testing.SharpMoku
{
	[TestClass]
	public class EvaluateTestingV3
	{
		public bool IsListValueEqual(List<int> listCellValue, List<int> listVerify)
		{
			if (listCellValue.Count != listVerify.Count)
			{
				return false;
			}
			int i;
			for (i = 0; i < listCellValue.Count; i++)
			{
				if (listCellValue[i] != listVerify[i])
				{
					return false;
				}
			}
			return true;
		}
		[TestMethod]
		public void GetCellValueInDirection()
		{
			global::SharpMoku.AI.EvaluateV3 evo = new();
			Board board = new(15);
			board.PutStone(0, 0, CellValue.White);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);

			Position currentPosition = new(0, 0);
			Position positionDeltaNorthSouth = new(1, 0);
			Position positionDeltaWestEast = new(0, 1);
			Position positionDeltaNorthEast = new(1, 1);
			Position positionDeltaSouthWest = new(1, -1);

			CellValue cellValue = CellValue.White;

			List<int> listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			List<int> listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			List<int> listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			List<int> listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(5, listNorthSouth.Count);
			Assert.AreEqual(5, listWestEast.Count);
			Assert.AreEqual(5, listNorthEast.Count);
			Assert.AreEqual(1, listSouthWest.Count);

			List<int> listNorthSouthExpectedValue = [1, 0, 0, 0, 0];
			List<int> listWestEastExpectedValue = [1, 1, 1, 1, 1];
			List<int> listNorthEastExpectedValue = [1, 0, 0, 0, 0];
			List<int> listSoutWestExpectedValue = [1];
			Assert.AreEqual(true, IsListValueEqual(listNorthSouth, listNorthSouthExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listWestEast, listWestEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listNorthEast, listNorthEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listSouthWest, listSoutWestExpectedValue));

			board.PutStone(0, 1);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);

			Assert.AreEqual(5, listNorthSouth.Count);
			Assert.AreEqual(2, listWestEast.Count);
			Assert.AreEqual(5, listNorthEast.Count);
			Assert.AreEqual(1, listSouthWest.Count);

			Position blankCellPosition = new(10, 10);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, blankCellPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, blankCellPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, blankCellPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, blankCellPosition, positionDeltaSouthWest);
			Assert.AreEqual(0, listNorthSouth.Count);
			Assert.AreEqual(0, listWestEast.Count);
			Assert.AreEqual(0, listNorthEast.Count);
			Assert.AreEqual(0, listSouthWest.Count);

			currentPosition = new Position(10, 10);

			board.PutStone(currentPosition, CellValue.White);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(9, listNorthSouth.Count);
			Assert.AreEqual(9, listWestEast.Count);
			Assert.AreEqual(9, listNorthEast.Count);
			Assert.AreEqual(9, listSouthWest.Count);

			listNorthSouthExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];
			listWestEastExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];
			listNorthEastExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];
			listSoutWestExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];

			Assert.AreEqual(true, IsListValueEqual(listNorthSouth, listNorthSouthExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listWestEast, listWestEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listNorthEast, listNorthEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listSouthWest, listSoutWestExpectedValue));

			board.PutStone(9, 10, CellValue.Black);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(6, listNorthSouth.Count);
			Assert.AreEqual(9, listWestEast.Count);
			Assert.AreEqual(9, listNorthEast.Count);
			Assert.AreEqual(9, listSouthWest.Count);

			listNorthSouthExpectedValue = [-1, 1, 0, 0, 0, 0];
			listWestEastExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];
			listNorthEastExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];
			listSoutWestExpectedValue = [0, 0, 0, 0, 1, 0, 0, 0, 0];

			Assert.AreEqual(true, IsListValueEqual(listNorthSouth, listNorthSouthExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listWestEast, listWestEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listNorthEast, listNorthEastExpectedValue));
			Assert.AreEqual(true, IsListValueEqual(listSouthWest, listSoutWestExpectedValue));

			board.PutStone(9, 11, CellValue.Black);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(6, listNorthSouth.Count);
			Assert.AreEqual(9, listWestEast.Count);
			Assert.AreEqual(9, listNorthEast.Count);
			Assert.AreEqual(6, listSouthWest.Count);

			board.PutStone(11, 11, CellValue.Black);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(6, listNorthSouth.Count);
			Assert.AreEqual(9, listWestEast.Count);
			Assert.AreEqual(6, listNorthEast.Count);
			Assert.AreEqual(6, listSouthWest.Count);

			board.PutStone(11, 9, CellValue.Black);
			listNorthSouth = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthSouth);
			listWestEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaWestEast);
			listNorthEast = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaNorthEast);
			listSouthWest = evo.GetCellValueInDirection(board.Matrix, cellValue, currentPosition, positionDeltaSouthWest);
			Assert.AreEqual(6, listNorthSouth.Count);
			Assert.AreEqual(9, listWestEast.Count);
			Assert.AreEqual(6, listNorthEast.Count);
			Assert.AreEqual(3, listSouthWest.Count);
		}

		[TestMethod]
		public void GetListAllDirection()
		{
			global::SharpMoku.AI.EvaluateV3 evo = new();
			Board board = new(15);
			Position currentPosition = new(10, 10);

			board.PutStone(currentPosition, CellValue.White);
			List<List<int>> listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);

			Assert.AreEqual(4, listCheckArea.Count);
			Assert.AreEqual(9, listCheckArea[0].Count);
			Assert.AreEqual(9, listCheckArea[1].Count);
			Assert.AreEqual(9, listCheckArea[2].Count);
			Assert.AreEqual(9, listCheckArea[3].Count);

		}

		[TestMethod]
		public void IsInArray()
		{

			List<List<int>> Stone4WithNoBlock = [[0, 1, 1, 1, 1, 0]];
			List<List<int>> Stone3WithNoBlock = [
						[0, 1, 1, 1, 0, 0],
						[0, 0, 1, 1, 1, 0],
						[0, 1, 0, 1, 1, 0],
						[0, 1, 1, 0, 1, 0],

					];
			_ = new
			global::SharpMoku.AI.EvaluateV3();
			List<int> listvalue = [0, 1, 1, 1, 1, 0];
			bool isIn = EvaluateV3.IsAnyInArrays(Stone4WithNoBlock, listvalue);

			Assert.AreEqual(true, isIn);
			listvalue = [0, 1, 0, 1, 1, 0];
			isIn = EvaluateV3.IsAnyInArrays(Stone3WithNoBlock, listvalue);
			Assert.AreEqual(true, isIn);

		}

		[TestMethod]
		public void ValuePosition()
		{
			global::SharpMoku.AI.EvaluateV3 evo = new();
			Board board = new(15);

			Position emptyCellPosition = new(10, 10);
			List<List<int>> listCheckArea = evo.GetListAllDirection(board, emptyCellPosition, CellValue.White);
			global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern numberPatternScore = evo.ValuePosition(listCheckArea, false);

			Assert.IsTrue(numberPatternScore.HasZeroValue);

			board.PutStone(0, 0, CellValue.White);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);
			Position currentPosition = new(0, 0);
			List<List<int>> listCheckAreaForBlack = evo.GetListAllDirection(board, currentPosition, CellValue.Black);

			numberPatternScore = evo.ValuePosition(listCheckAreaForBlack, true);
			Assert.IsTrue(numberPatternScore.HasZeroValue);

			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);
			numberPatternScore = evo.ValuePosition(listCheckArea, false);

			Assert.AreEqual(1, numberPatternScore.Winning);
			Assert.AreEqual(0, numberPatternScore.Stone4);
			Assert.AreEqual(0, numberPatternScore.Stone3);
			Assert.AreEqual(0, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

			board = new Board(15);

			board.PutStone(0, 0, CellValue.White);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);

			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);
			numberPatternScore = evo.ValuePosition(listCheckArea, false);
			Assert.AreEqual(0, numberPatternScore.Winning);
			Assert.AreEqual(0, numberPatternScore.Stone4);
			Assert.AreEqual(0, numberPatternScore.Stone3);
			Assert.AreEqual(0, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

			board = new Board(15);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);
			currentPosition = new Position(0, 4);
			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);
			numberPatternScore = evo.ValuePosition(listCheckArea, false);
			Assert.AreEqual(0, numberPatternScore.Winning);
			Assert.AreEqual(1, numberPatternScore.Stone4);
			Assert.AreEqual(0, numberPatternScore.Stone3);
			Assert.AreEqual(0, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

			board = new Board(15);
			//0, 1, 0, 1, 1, 0
			board.PutStone(0, 1, CellValue.White);

			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);
			currentPosition = new Position(0, 4);
			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);

			numberPatternScore = evo.ValuePosition(listCheckArea, false);
			Assert.AreEqual(0, numberPatternScore.Winning);
			Assert.AreEqual(0, numberPatternScore.Stone4);
			Assert.AreEqual(1, numberPatternScore.Stone3);
			Assert.AreEqual(0, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

			board = new Board(15);
			// 0, 1, 0, 1, 0, 0
			board.PutStone(0, 1, CellValue.White);

			board.PutStone(0, 3, CellValue.White);

			currentPosition = new Position(0, 3);
			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.White);
			numberPatternScore = evo.ValuePosition(listCheckArea, false);
			Assert.AreEqual(0, numberPatternScore.Winning);
			Assert.AreEqual(0, numberPatternScore.Stone4);
			Assert.AreEqual(0, numberPatternScore.Stone3);
			Assert.AreEqual(1, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

			board = new Board(15);

			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(2, 3, CellValue.Black);
			board.PutStone(4, 3, CellValue.Black);
			currentPosition = new Position(4, 3);
			listCheckArea = evo.GetListAllDirection(board, currentPosition, CellValue.Black);
			numberPatternScore = evo.ValuePosition(listCheckArea, true);
			Assert.AreEqual(0, numberPatternScore.Winning);
			Assert.AreEqual(0, numberPatternScore.Stone4);
			Assert.AreEqual(1, numberPatternScore.Stone3);
			Assert.AreEqual(0, numberPatternScore.Stone2);
			Assert.AreEqual(0, numberPatternScore.BlockStone4);
			Assert.AreEqual(0, numberPatternScore.BlockStone3);

		}

		[TestMethod]
		public void ScoreByPattern()
		{

			global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern numPattern = new(1, 0, 0, 0, 0, 0);
			global::SharpMoku.AI.EvaluateV3 evo = new();

			int scoreByPattern;
			scoreByPattern = evo.getScoreByPattern(numPattern);

			const int wonPattern = 1000000000;
			const int stone4Pattern = 100000000;
			Assert.AreEqual(scoreByPattern, wonPattern);
			numPattern.Winning = 0;
			numPattern.Stone4 = 1;

			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, stone4Pattern);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 1,
				BlockStone4 = 1
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, global::SharpMoku.AI.EvaluateV3.CONST_winGuarantee / 100);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 2
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, global::SharpMoku.AI.EvaluateV3.CONST_winGuarantee / 1000);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 1,
				Stone2 = 3
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 40000);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 1,
				Stone2 = 2
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 38000);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 1,
				Stone2 = 1
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 35000);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				Stone3 = 1,
				Stone2 = 0
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 3450);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				BlockStone4 = 1,
				Stone2 = 3
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 4500);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				BlockStone4 = 1,
				Stone2 = 2
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 4200);

			numPattern = new global::SharpMoku.AI.EvaluateV3.NumberOfScorePattern {
				BlockStone4 = 1,
				Stone2 = 1
			};
			scoreByPattern = evo.getScoreByPattern(numPattern);
			Assert.AreEqual(scoreByPattern, 4100);

			//double dblScore=    evo.evaluateBoardForOponent(board, IsMyTurn);

		}

		[TestMethod]
		public void GetScore()
		{
			global::SharpMoku.AI.EvaluateV3 evo = new();
			Board board = new(15);
			//  Position currentPosition = new Position(10, 10);

			board.PutStone(0, 0, CellValue.White);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);

			Board cloneBoard = board.Clone();
			_ = evo.GetScore(board);// evo.GetScore(board, IsThisforblack, IsThisBlackTurn);

			//Make sure that evo.GetScore will not update value in the board
			Assert.AreEqual(true, BoardTesting.IsBoardTheSame(board, cloneBoard));

			board = new Board(15);
			board.PutStone(0, 1, CellValue.White);
			board.PutStone(0, 2, CellValue.White);
			board.PutStone(0, 3, CellValue.White);
			board.PutStone(0, 4, CellValue.White);
			board.PutStone(0, 5, CellValue.Black);
			_ = evo.GetScore(board);

			board = new Board(9);
			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(1, 7, CellValue.White);
			board.PutStone(2, 3, CellValue.Black);
			board.PutStone(2, 7, CellValue.White);
			board.PutStone(4, 3, CellValue.Black);
			_ = evo.GetScore(board);

		}
	}
}
