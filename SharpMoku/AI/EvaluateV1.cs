using System;

namespace SharpMoku.AI
{
	//This is just a random evaluate function for random bot
	internal class EvaluateV1 : IEvaluate
	{
		private static readonly Random s_random = new();
		private static int GetRandomNumber(int min, int max)
		{
			lock (s_random) // synchronize
			{
				return s_random.Next(min, max);
			}
		}
		public double EvaluateBoard(Board board, bool isMyTurn)
		{
			return GetRandomNumber(1, 1000);

		}

		public int GetScore(Board board)
		{
			return GetRandomNumber(1, 1000);

		}
	}
}
