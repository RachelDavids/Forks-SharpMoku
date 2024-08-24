namespace SharpMoku.AI
{
	public interface IEvaluate
	{
		double EvaluateBoard(Board board, bool isMyTurn);
		int GetScore(Board board);
	}
}
