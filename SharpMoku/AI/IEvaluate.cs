namespace SharpMoku.AI
{
	public interface IEvaluate
	{
		double EvaluateBoard(Board.Board board, bool isMyTurn);
		int GetScore(Board.Board board);
	}
}
