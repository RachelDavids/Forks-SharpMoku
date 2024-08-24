using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharpMoku.AI;
using SharpMoku.Board;

using SMBoard = SharpMoku.Board.Board;

namespace Testing.SharpMoku.AI
{
	[TestClass]
	public class EvaluateTestingV2
	{
		private const int ScoreConsecutive2Stone = 7;
		private const int ScoreConsecutive3Stone = 50000;
		private const int ScoreConsecutiveCanWin = 1000000;
		private const int ScoreConsecutiveCanWinIfCurrentTurn = 250000;
		private const int ScoreWin = 50000000;
		[TestMethod]
		public void calculateConsecutiveStoneSequenceScore()
		{
			EvaluateV2 evo = new();

			//SharpMoku.AI.EvaluateLast evo = new SharpMoku.AI.EvaluateLast();
			/*
            Board board = new Board(15);
            board.PutStone(0, 0, Board.CellValue.Black);
            board.PutStone(0, 1, Board.CellValue.Black);
            board.PutStone(0, 2, Board.CellValue.Black);
            board.PutStone(0, 3, Board.CellValue.Black);
            board.PutStone(0, 4, Board.CellValue.Black);
            */
			bool isCurrentTurn = true;
			bool myTurnIsTrue = true;
			bool myTurnIsFalse = false;

			int blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(5, 0, isCurrentTurn);
			Assert.IsTrue(blackScore == EvaluateV2.wonScore);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(4, 0, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.confirmWinScore);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(4, 0, myTurnIsFalse);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone4_Not_MyTurn_0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(4, EvaluateV2.BlockType.OneSideBlock, myTurnIsFalse);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone4_Not_MyTurn_MoreThan0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(3, 0, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone3_MyTurn_0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(3, EvaluateV2.BlockType.OneSideBlock, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone3_MyTurn_MoreThan0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(3, 0, myTurnIsFalse);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone3_Not_MyTurn_0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(3, EvaluateV2.BlockType.OneSideBlock, myTurnIsFalse);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone3_Not_MyTurn_MoreThan0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(2, 0, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone2_MyTurn_0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(2, EvaluateV2.BlockType.OneSideBlock, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone2_MoreThan0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(2, 0, myTurnIsFalse);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone2_Not_MyTurn_0Block);

			blackScore = EvaluateV2.CalculateConsecutiveStoneSequenceScore(1, 0, myTurnIsTrue);
			Assert.IsTrue(blackScore == EvaluateV2.ConsecutiveStone1);
		}

		[TestMethod]
		public void EvaluateHorizontal()
		{
			EvaluateV2 evo = new();
			SMBoard board = new(15);
			bool isForBlack = true;
			bool isPlayerTurn = true;
			;
			Func<int[,], bool, bool, int> evaFunc = evo.CalculateScoreForHorizontal;

			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Black);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(0, 5, CellValue.Black);

			int blackScore = evaFunc(board.Matrix, isForBlack, isPlayerTurn);
			Assert.IsTrue(blackScore == EvaluateV2.wonScore);
			board.RemoveStone(new Position(0, 5));
			//board.PutStone(0, 5, Board.CellValue.Black);
			blackScore = evaFunc(board.Matrix, isForBlack, isPlayerTurn);
			Assert.IsTrue(blackScore == EvaluateV2.confirmWinScore);
		}

		[TestMethod]
		public void GetStonePatternForVertical()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);

			bool isForCalculateBlackStone = true;

			List<EvaluateV2.StonePattern> listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);

			EvaluateV2.StonePattern pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board = new SMBoard(15);
			board.PutStone(0, 0, CellValue.Empty);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);
			board.PutStone(5, 0, CellValue.Black);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			pattern = listPattern[0];
			Assert.IsTrue(listPattern.Count == 1);
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(0, 0, CellValue.White);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(6, 0, CellValue.White);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.BothSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board = new SMBoard(15);
			int i;
			for (i = 0; i < 15; i++)
			{
				board.PutStone(1, i, CellValue.Black);
				board.PutStone(2, i, CellValue.Black);
				board.PutStone(3, i, CellValue.Black);
				board.PutStone(4, i, CellValue.Black);
				board.PutStone(5, i, CellValue.Black);
			}
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 15);
			for (i = 0; i < 15; i++)
			{
				pattern = listPattern[i];
				Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
				Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);
			}

			board = new SMBoard(15);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Empty);
			board.PutStone(4, 0, CellValue.Black);
			board.PutStone(5, 0, CellValue.Black);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			board.PutStone(3, 0, CellValue.White);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

		}

		[TestMethod]
		public void GetStonePatternForDiagonal()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 1, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 3, CellValue.Black);
			board.PutStone(4, 4, CellValue.Black);

			bool isForCalculateBlackStone = true;
			//bool isBlackTurn = true;
			;
			//int blackScore = 0;
			//evaluateFunction evaFunc = evo.CalculateScoreForVertical;

			List<EvaluateV2.StonePattern> listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);

			EvaluateV2.StonePattern pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board = new SMBoard(15);
			board.PutStone(0, 0, CellValue.Empty);
			board.PutStone(1, 1, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 3, CellValue.Black);
			board.PutStone(4, 4, CellValue.Black);
			board.PutStone(5, 5, CellValue.Black);
			listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			pattern = listPattern[0];
			Assert.IsTrue(listPattern.Count == 1);
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(0, 0, CellValue.White);
			listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(6, 6, CellValue.White);
			listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.BothSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);
			/*
            board = new Board(15);
            int i;
            for (i = 0; i < 15; i++)
            {
                board.PutStone(1, i, Board.CellValue.Black);
                board.PutStone(2, i, Board.CellValue.Black);
                board.PutStone(3, i, Board.CellValue.Black);
                board.PutStone(4, i, Board.CellValue.Black);
                board.PutStone(5, i, Board.CellValue.Black);
            }
            listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
            Assert.IsTrue(listPattern.Count == 15);
            for (i = 0; i < 15; i++)
            {
                pattern = listPattern[i];
                Assert.IsTrue(pattern.blockType == EvaluateV2.BlockType.ZeroBlock);
                Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);
            }
            */

			board = new SMBoard(15);
			board.PutStone(1, 1, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 3, CellValue.Empty);
			board.PutStone(4, 4, CellValue.Black);
			board.PutStone(5, 5, CellValue.Black);
			listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			board.PutStone(3, 3, CellValue.White);
			listPattern = evo.GetStonePatternForDiagonal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);
		}
		[TestMethod]
		public void GetStonePatternForVerticalNoPattern()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);

			// Number of White stone pattern, it must be 0 because there is no white stone
			List<EvaluateV2.StonePattern> listPattern = evo.GetStonePatternForVertical(board.Matrix, false);
			Assert.IsTrue(listPattern.Count == 0);

			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);
			listPattern = evo.GetStonePatternForVertical(board.Matrix, false);
			Assert.IsTrue(listPattern.Count == 0);

		}

		[TestMethod]
		public void GetStonePatternForHorizontal()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Black);
			board.PutStone(0, 4, CellValue.Black);

			bool isForCalculateBlackStone = true;
			//bool isBlackTurn = true;
			;
			//int blackScore = 0;
			//     evaluateFunction evaFunc = evo.CalculateScoreForVertical;

			List<EvaluateV2.StonePattern> listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);

			EvaluateV2.StonePattern pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board = new SMBoard(15);
			board.PutStone(0, 0, CellValue.Empty);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Black);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(0, 5, CellValue.Black);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			pattern = listPattern[0];
			Assert.IsTrue(listPattern.Count == 1);
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(0, 0, CellValue.White);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board.PutStone(0, 6, CellValue.White);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 1);
			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.BothSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);

			board = new SMBoard(15);
			int i;
			for (i = 0; i < 15; i++)
			{
				board.PutStone(i, 1, CellValue.Black);
				board.PutStone(i, 2, CellValue.Black);
				board.PutStone(i, 3, CellValue.Black);
				board.PutStone(i, 4, CellValue.Black);
				board.PutStone(i, 5, CellValue.Black);
			}
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 15);
			for (i = 0; i < 15; i++)
			{
				pattern = listPattern[i];
				Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
				Assert.IsTrue(pattern.NumberOfConsecutiveStones == 5);
			}

			board = new SMBoard(15);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Empty);
			board.PutStone(0, 4, CellValue.Black);
			board.PutStone(0, 5, CellValue.Black);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.ZeroBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			board.PutStone(0, 3, CellValue.White);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, isForCalculateBlackStone);
			Assert.IsTrue(listPattern.Count == 2);

			pattern = listPattern[0];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);

			pattern = listPattern[1];
			Assert.IsTrue(pattern.BlockType == EvaluateV2.BlockType.OneSideBlock);
			Assert.IsTrue(pattern.NumberOfConsecutiveStones == 2);
		}

		[TestMethod]
		public void GetStonePatternForHorizontalNoPattern()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);
			// Number of White stone pattern, it must be 0 because there is no white stone
			List<EvaluateV2.StonePattern> listPattern = evo.GetStonePatternForHorizontal(board.Matrix, false);
			Assert.IsTrue(listPattern.Count == 0);

			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);
			board.PutStone(0, 3, CellValue.Black);
			board.PutStone(0, 4, CellValue.Black);
			listPattern = evo.GetStonePatternForHorizontal(board.Matrix, false);
			Assert.IsTrue(listPattern.Count == 0);

		}
		[TestMethod]
		public void EvaluatVertical()
		{

			EvaluateV2 evo = new();
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 0, CellValue.Black);
			board.PutStone(2, 0, CellValue.Black);
			board.PutStone(3, 0, CellValue.Black);
			board.PutStone(4, 0, CellValue.Black);

			bool isForCalculateBlackStone = true;
			bool isBlackTurn = true;
			;
			Func<int[,], bool, bool, int> evaFunc = evo.CalculateScoreForVertical;

			int blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(blackScore == EvaluateV2.wonScore);

			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, !isBlackTurn);
			Assert.IsTrue(blackScore == EvaluateV2.wonScore);

			blackScore = evaFunc(board.Matrix, !isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(blackScore == 0);

			board.RemoveStone(2, 0);
			board.PutStone(2, 0, CellValue.White);
			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(blackScore == 3);
			//Only 1 white stone between black
			int whiteScore = evaFunc(board.Matrix, !isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(whiteScore == 0);

			board.PutStone(0, 5, CellValue.Black);
			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(blackScore == 3);

			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, !isBlackTurn);
			Assert.IsTrue(blackScore == 3);

			board = new SMBoard(9);
			board.PutStone(1, 1, CellValue.White);
			board.PutStone(1, 2, CellValue.White);
			board.PutStone(2, 1, CellValue.White);

			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 2, CellValue.Black);
			board.PutStone(4, 2, CellValue.Black);
			board.PutStone(5, 2, CellValue.Black);
			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Assert.IsTrue(blackScore == 1000000);

			blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, !isBlackTurn);
			Assert.IsTrue(blackScore == 300);
		}
		[TestMethod]
		public void searchBotLosingPosition()
		{
			SMBoard board = new(9);
			board.PutStone(1, 1, CellValue.White);
			board.PutStone(1, 2, CellValue.White);
			board.PutStone(2, 1, CellValue.White);

			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 2, CellValue.Black);
			board.PutStone(4, 2, CellValue.Black);
			board.PutStone(5, 2, CellValue.Black);

			board.PutStone(0, 2, CellValue.White);
			if (board.CurrentTurn == Turn.White)
			{
				board.SwitchTurn();
			}
			//Minimax minimax = new Minimax(board, new EvaluateV2(), null);
			EvaluateV2 bot = new();
			_ = bot.GetScore(board);
			List<EvaluateV2.StonePattern> list = bot.GetStonePatternForVertical(board.Matrix, isForBlackstone: true);
			_ = EvaluateV2.CalculateConsecutiveStoneSequenceScore(list[0], true);

			/*
            Minimax minimax = new Minimax(board, new EvaluateV2(), null);
            MoveScore moveScore= minimax.searchBotLosingPosition(board, new EvaluateV2());
            */

			//blackScore = evaFunc(board.Matrix, isForCalculateBlackStone, isBlackTurn);

		}

		[TestMethod]
		public void EvaluateDiagonal()
		{
			EvaluateV2 evo = new();
			SMBoard board = new(15);
			bool isForCalculateBlackStone = true;
			bool isBlackTurn = true;

			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(1, 1, CellValue.Black);
			board.PutStone(2, 2, CellValue.Black);
			board.PutStone(3, 3, CellValue.Black);
			board.PutStone(4, 4, CellValue.Black);

			int blackScore = evo.CalculateScoreForDiagonal(board.Matrix,
										   isForBlackStone:
										   isForCalculateBlackStone,
										   isBlackTurn);

			Assert.IsTrue(blackScore == EvaluateV2.wonScore);

		}

		[TestMethod]
		public void Test01()
		{
			EvaluateV2 evo = new();
			SMBoard board = new(15);
			board.PutStone(0, 0, CellValue.Black);
			board.PutStone(0, 1, CellValue.Black);
			board.PutStone(0, 2, CellValue.Black);

			board.PutStone(8, 0, CellValue.White);
			board.PutStone(8, 1, CellValue.White);
			board.PutStone(8, 2, CellValue.White);

			bool isForCalculateBlackStone = true;
			//bool isPlayerTurn = true;
			;
			//int blackScore = 0;

			//    bool isItForMe = true;
			bool isBlackTurn = true;
			double score = evo.CalculateScoreForHorizontal(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Console.WriteLine("score::" + score);

			isForCalculateBlackStone = false;
			isBlackTurn = true;
			score = evo.CalculateScoreForHorizontal(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Console.WriteLine("score::" + score);

			isForCalculateBlackStone = true;
			isBlackTurn = false;
			score = evo.CalculateScoreForHorizontal(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Console.WriteLine("score::" + score);

			isForCalculateBlackStone = false;
			isBlackTurn = false;
			score = evo.CalculateScoreForHorizontal(board.Matrix, isForCalculateBlackStone, isBlackTurn);
			Console.WriteLine("score::" + score);
		}
	}
}
