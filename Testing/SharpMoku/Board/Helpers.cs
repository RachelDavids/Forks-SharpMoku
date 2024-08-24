namespace Testing.SharpMoku.Board
{
	public static class Helpers
	{
		public static bool IsBoardTheSame(this global::SharpMoku.Board.Board board1, global::SharpMoku.Board.Board board2)
		{
			if (board1.CurrentTurn != board2.CurrentTurn)
			{
				return false;
			}
			if (board1.CurrentTurnCellValue != board2.CurrentTurnCellValue)
			{
				return false;
			}
			if (board1.CanUndo != board2.CanUndo)
			{
				return false;
			}
			if (board1.ListHistory != null && board2.ListHistory == null)
			{
				return false;
			}
			if (board2.ListHistory != null && board1.ListHistory == null)
			{
				return false;
			}

			if (board1.ListHistory.Count != board2.ListHistory.Count)
			{
				return false;
			}

			if (board1.CheckWinStatus() != board2.CheckWinStatus())
			{
				return false;
			}

			if (board1.Neighbours.Count != board2.Neighbours.Count)
			{
				return false;
			}
			if (board1.BoardSize != board2.BoardSize)
			{
				return false;
			}
			if (board1.BlackStones.Count != board2.BlackStones.Count)
			{
				return false;
			}
			if (board1.WhiteStones.Count != board2.WhiteStones.Count)
			{
				return false;
			}
			int i;
			int j;
			for (i = 0; i < board1.BoardSize; i++)
			{
				for (j = 0; j < board1.BoardSize; j++)
				{
					if (board1.Matrix[i, j] != board2.Matrix[i, j])
					{
						return false;
					}
				}
			}
			foreach (string posString in board1.WhiteStones.Keys)
			{
				if (!board1.WhiteStones[posString].IsEqual(
						board2.WhiteStones[posString]))
				{
					return false;
				}
			}
			foreach (string posString in board1.Neighbours.Keys)
			{
				if (!board1.Neighbours[posString].IsEqual(
						board2.Neighbours[posString]))
				{
					return false;
				}
			}
			foreach (string posString in board1.BlackStones.Keys)
			{
				if (!board1.BlackStones[posString].IsEqual(
						board2.BlackStones[posString]))
				{
					return false;
				}
			}

			for (i = 0; i < board1.ListHistory.Count; i++)
			{
				if (!board1.ListHistory[i].IsEqual(board2.ListHistory[i]))
				{
					return false;
				}
			}

			return true;
		}
	}
}
