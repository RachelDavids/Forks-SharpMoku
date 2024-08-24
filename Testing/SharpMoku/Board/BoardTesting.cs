using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharpMoku.Board;

using SMBoard = global::SharpMoku.Board.Board;

namespace Testing.SharpMoku.Board
{
	[TestClass]
	public class BoardTesting
	{
		[TestMethod]
		public void PutStoneInvalidStoneValue()
		{
			SMBoard board = new(15);
			try
			{
				board.PutStone(-1, 0, 2);
				Trace.Fail("Must not allow to put into 2 value");
			}
			catch
			{

			}
		}
		[TestMethod]
		public void PutStoneInvalidPosition()
		{
			SMBoard board = new(15);
			try
			{
				board.PutStone(-1, 0, CellValue.Black);
				Trace.Fail("Must not allow to put into -1,0 position");
			}
			catch
			{

			}

			int[,] arrInvalidPosition = new int[,]
			{
				{-1,-1 },
				{15,0 },
				{15,15 },
				{15,-5 }
			};
			int i;
			for (i = 0; i < arrInvalidPosition.Length; i++)
			{
				try
				{
					board.PutStone(arrInvalidPosition[i, 0],
						arrInvalidPosition[i, 1], CellValue.Black);
					Trace.Fail(String.Format("Must not allow to put into {0},{1} position ",
						arrInvalidPosition[i, 0],
						arrInvalidPosition[i, 1]));
				}
				catch
				{

				}
			}
		}
		public static bool ContainsAllKeys(Dictionary<string, Position> dicPosition,
										   List<Position> listPosition)
		{
			return listPosition.All(position => dicPosition.ContainsKey(position.PositionString()));
		}
		[TestMethod]
		public void PutStone()
		{
			SMBoard board = new(15);
			Position position = new(0, 0);
			Assert.IsTrue(board.IsEmpty);

			board.PutStone(position, CellValue.Black);

			Assert.IsFalse(board.IsEmpty);

			Assert.IsTrue(board.Matrix[0, 0] == (int)CellValue.Black);

			List<Position> listNeighbor =
			[
				new Position (0,1),
				new Position (1,0),
				new Position (1,1)
			];

			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 1);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 0);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.BlackStones.ContainsKey(position.PositionString()));
			Assert.IsTrue(board.Neighbours.Count == 3);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			position = new Position(0, 1);
			board.PutStone(position, CellValue.White);
			listNeighbor =
			[
				new Position (0,2),
				new Position (1,0),
				new Position (1,1),
				new Position (1,2)
			];

			Assert.IsTrue(board.Matrix[0, 1] == (int)CellValue.White);

			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 2);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 1);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.WhiteStones.ContainsKey(position.PositionString()));
			Assert.IsTrue(board.Neighbours.Count == 4);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			board = new SMBoard(15);
			position = new Position(5, 5);
			board.PutStone(position, CellValue.White);

			listNeighbor =
			[
				new Position (4,4),
				new Position (4,5),
				new Position (4,6),
				new Position (5,6),
				new Position (6,6),
				new Position (6,5),
				new Position (6,4),
				new Position (5,4),
			];
			Assert.IsTrue(board.Matrix[5, 5] == (int)CellValue.White);

			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 1);
			Assert.IsTrue(board.BlackStones.Count == 0);
			Assert.IsTrue(board.WhiteStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.ContainsKey(position.PositionString()));
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.Neighbours.Count == 8);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			board.PutStone(5, 6, CellValue.Black);
			listNeighbor =
			[
				new Position (4,4),
				new Position (4,5),
				new Position (4,6),
				new Position (4,7),

				new Position (5,7),
				new Position (6,7),
				new Position (6,6),
				new Position (6,5),
				new Position (6,4),
				new Position (5,4),
			];
			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 2);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 1);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.Neighbours.Count == 10);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			board.PutStone(5, 8, CellValue.White);
			listNeighbor =
			[
				new Position (4,4),
				new Position (4,5),
				new Position (4,6),
				new Position (4,7),

				new Position (5,7),
				new Position (6,7),
				new Position (6,6),
				new Position (6,5),
				new Position (6,4),
				new Position (5,4),

				new Position (4,8),
				new Position (4,9),
				new Position (5,9),
				new Position (6,8),
				new Position (6,9)
			];
			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 3);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 2);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.Neighbours.Count == 15);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			Position lastPosition = board.LastPositionPut;
			Assert.IsTrue(lastPosition.Row == 5 && lastPosition.Col == 8);
			//  Assert.IsTrue (board.dic
		}

		[TestMethod]
		public void PutStoneAndSwitchTurn()
		{
			SMBoard board = new(15);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.IsEmpty);

			board.PutStoneAndSwitchTurn(0, 0, CellValue.Black);
			Assert.IsTrue(board.Matrix[0, 0] == (int)CellValue.Black);

			Assert.IsTrue(board.CurrentTurn == Turn.White);
			board.PutStoneAndSwitchTurn(0, 1, CellValue.White);
			Assert.IsTrue(board.Matrix[0, 1] == (int)CellValue.White);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);

		}

		[TestMethod]
		public void BoardStatusNotDecidedYet()
		{
			SMBoard board = new(15);
			Assert.IsTrue(board.IsEmpty);

			board.PutStone(0, 0, CellValue.Black);
			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.NotDecidedYet);

		}

		[TestMethod]
		public void WinStatusWestAndEast()
		{
			SMBoard board = new(15);
			Assert.IsTrue(board.IsEmpty);

			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Black);
			board.PutStone(0, 4, CellValue.Black);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.BlackWon);

		}

		[TestMethod]
		public void WinStatusWhiteNorthAndSouth()
		{
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.White);
			board.PutStone(1, 0, CellValue.White);
			board.PutStone(2, 0, CellValue.White);
			board.PutStone(3, 0, CellValue.White);
			board.PutStone(4, 0, CellValue.White);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.WhiteWon);

		}

		[TestMethod]
		public void WinStatusWhiteNorthWestAndSouthEast()
		{
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.White);
			board.PutStone(1, 1, CellValue.White);
			board.PutStone(2, 2, CellValue.White);
			board.PutStone(3, 3, CellValue.White);
			board.PutStone(4, 4, CellValue.White);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.WhiteWon);

		}

		[TestMethod]
		public void WinStatusWhiteNorthEastAndSouthWest()
		{
			SMBoard board = new(15);
			board.PutStone(0, 4, CellValue.White);
			board.PutStone(1, 3, CellValue.White);
			board.PutStone(2, 2, CellValue.White);
			board.PutStone(3, 1, CellValue.White);
			board.PutStone(4, 0, CellValue.White);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.WhiteWon);

		}

		[TestMethod]
		public void WinStatusBlackNorthAndSouth()
		{
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.BlackWon);

		}

		[TestMethod]
		public void WinStatusBlackNorthWestAndSouthEast()
		{
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 1, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 3, CellValue.Black);
			board.PutStone(4, 4, CellValue.Black);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.BlackWon);

		}

		[TestMethod]
		public void WinStatusBlackNorthEastAndSouthWest()
		{
			SMBoard board = new(15);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 1, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);

			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.BlackWon);

		}

		[TestMethod]
		public void Draw()
		{
			SMBoard board = new(9);
			int i;
			int j;
			for (i = 0; i <= 8; i++)
			{
				CellValue cellValue = i % 2 == 0 ? CellValue.Black : CellValue.White;
				int iCountCellValue = 0;
				for (j = 0; j <= 8; j++)
				{
					iCountCellValue++;
					board.PutStone(new Position(i, j), cellValue);
					if (iCountCellValue == 2)
					{
						cellValue = cellValue == CellValue.Black ? CellValue.White : CellValue.Black;
						iCountCellValue = 0;
					}
				}
			}
			Assert.IsTrue(board.IsFull);
			WinStatus winStatus = board.CheckWinStatus();
			Assert.IsTrue(winStatus == WinStatus.Draw);

			List<Position> listNeighbor = board.generateNeighbourMoves();
			Assert.IsTrue(listNeighbor.Count == 0);

			//   board.generateNeighbourMoves();
		}
		[TestMethod]
		public void Undo()
		{
			SMBoard board = new(15);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 1, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);
			board.PutStone(6, 6, CellValue.White);
			//
			SMBoard newBoard = new(board);
			newBoard.PutStone(8, 8, CellValue.White);

			Assert.IsTrue(!Helpers.IsBoardTheSame(board, newBoard));

			newBoard.Undo();
			board.SwitchTurn();
			Assert.IsTrue(Helpers.IsBoardTheSame(board, newBoard));

		}
		[TestMethod]
		public void GetListNeighborPosition()
		{
			SMBoard board = new(15);
			Position position = new(0, 0);
			board.PutStone(position, CellValue.Black);
			Assert.IsTrue(board.Matrix[0, 0] == (int)CellValue.Black);

			List<Position> list = board.GetListNeighborPosition(new Position(0, 0));
			Assert.IsTrue(list.Count == 3);
			List<Position> listNeighbor00 =
			[
				new Position (0,1),
				new Position (1,0),
				new Position (1,1)
			];

			int i;
			for (i = 0; i < listNeighbor00.Count; i++)
			{
				bool isExist = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (listNeighbor00[i].Row == list[j].Row &&
						listNeighbor00[i].Col == list[j].Col)
					{
						isExist = true;
					}
				}
				if (!isExist)
				{
					Assert.Fail($"Postion {listNeighbor00[i].Row},{listNeighbor00[i].Col} does not exist");
				}
			}
		}

		[TestMethod]
		public void AdjustEmptyNeighborOf()
		{
			SMBoard board = new(15);
			Position position = new(0, 0);
			board.PutStone(position, CellValue.Black);
			Assert.IsTrue(board.Matrix[0, 0] == (int)CellValue.Black);

			List<Position> listNeighbor00 =
			[
				new Position (0,1),
				new Position (1,0),
				new Position (1,1)
			];

			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 1);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 0);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.BlackStones.ContainsKey(position.PositionString()));
			Assert.IsTrue(board.Neighbours.Count == 3);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor00));

			position = new Position(0, 1);
			board.PutStone(position, CellValue.White);
			List<Position> listNeighbor =
			[
				new Position (0,2),
				new Position (1,0),
				new Position (1,1),
				new Position (1,2)
			];

			Assert.IsTrue(board.Matrix[0, 1] == (int)CellValue.White);
			Assert.IsTrue(board.CheckWinStatus() == WinStatus.NotDecidedYet);
			Assert.IsTrue(board.ListHistory.Count == 2);
			Assert.IsTrue(board.BlackStones.Count == 1);
			Assert.IsTrue(board.WhiteStones.Count == 1);
			Assert.IsTrue(board.CurrentTurn == Turn.Black);
			Assert.IsTrue(board.WhiteStones.ContainsKey(position.PositionString()));
			Assert.IsTrue(board.Neighbours.Count == 4);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor));

			board.RemoveStone(new Position(0, 1));
			board.AdjustEmptyNeighborOf(new Position(0, 1));
			Assert.IsTrue(board.Neighbours.Count == listNeighbor00.Count);
			Assert.IsTrue(ContainsAllKeys(board.Neighbours, listNeighbor00));

		}

		[TestMethod]
		public void Undo2Times()
		{
			SMBoard board = new(15);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 1, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);
			board.PutStone(6, 6, CellValue.White);
			//
			SMBoard NewBoard = new(board);
			NewBoard.PutStone(8, 8, CellValue.White);
			NewBoard.PutStone(7, 7, CellValue.Black);

			//Assert.IsTrue(!IsBoardTheSame(board, NewBoard));

			NewBoard.Undo();
			board.SwitchTurn();
			NewBoard.Undo();
			board.SwitchTurn();
			Assert.IsTrue(Helpers.IsBoardTheSame(board, NewBoard));

		}

		[TestMethod]
		public void CloneBoard()
		{
			SMBoard board = new(9);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(1, 3, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 1, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);

			SMBoard newBoard = new(board);
			Assert.IsTrue(newBoard.BoardSize == 9);
			Assert.IsTrue(newBoard.Matrix[0, 4] == (int)CellValue.Black);
			Assert.IsTrue(newBoard.Matrix[1, 3] == (int)CellValue.Black);
			Assert.IsTrue(newBoard.Matrix[2, 2] == (int)CellValue.Black);
			Assert.IsTrue(newBoard.Matrix[3, 1] == (int)CellValue.Black);
			Assert.IsTrue(newBoard.Matrix[4, 0] == (int)CellValue.Black);
			//Assert.IsTrue(NewBoard.Matrix[0, 4] == (int)Board.CellValue.Black);
			Assert.IsTrue(newBoard.BlackStones.Count == 5);
			Assert.IsTrue(newBoard.WhiteStones.Count == 0);

			Assert.IsTrue(board.IsBoardTheSame(newBoard));
		}
		//[TestMethod]
		//public void WinStatusProblem()
		//{
		//	string fileName = @"D:\CODE\visual studio 2010\Projects\SharpMoku\SharpMoku\SharpMoku\bin\Debug\AppInfo\Board.bin";
		//	Board board = SharpMoku.Utility.SerializeUtility.DeserializeBoard(fileName);
		//	Board.WinStatus winStatus = board.CheckWinStatus();
		//	Assert.IsTrue(winStatus == Board.WinStatus.NotDecidedYet);
		//}
	}
}
