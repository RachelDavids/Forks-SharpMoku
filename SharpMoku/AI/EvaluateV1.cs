using System;

namespace SharpMoku.AI
{
	//This is just a random evaluate function for random bot
	internal class EvaluateV1 : IEvaluate
	{
		private static Random random = new();
		private static int GetRandomNumber(int min, int max)
		{
			lock (random) // synchronize
			{
				return random.Next(min, max);
			}
		}
		public double evaluateBoard(Board board, bool IsMyturn)
		{
			return GetRandomNumber(1, 1000);

		}

		public int getScore(Board board)
		{
			return GetRandomNumber(1, 1000);

		}
	}
}
