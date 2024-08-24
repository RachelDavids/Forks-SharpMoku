using SharpMoku.Board;

namespace SharpMoku.AI
{
	// Contains move position and calculated score from Minimax
	public struct MoveScore
	{
		public double Score { get; set; }
		public int Row { get; set; }
		public int Col { get; set; }
		public MoveScore(double pScore)
		{
			Score = pScore;
			Row = -1;
			Col = -1;
		}
		public readonly Position GetPosition()
		{
			return new(Row, Col);
		}
		public static MoveScore Max(MoveScore moveScore1, MoveScore moveScore2)
		{
			return moveScore1.Score > moveScore2.Score
				? new(moveScore1.Score, moveScore1.Row, moveScore1.Col)
				: new MoveScore(moveScore2.Score, moveScore2.Row, moveScore2.Col);
		}
		public static MoveScore Min(MoveScore moveScore1, MoveScore moveScore2)
		{
			return moveScore1.Score < moveScore2.Score
				? new(moveScore1.Score, moveScore1.Row, moveScore1.Col)
				: new MoveScore(moveScore2.Score, moveScore2.Row, moveScore2.Col);
		}
		public MoveScore(double score, int row, int col)
		{
			Score = score;
			Row = row;
			Col = col;
		}
	}
}
