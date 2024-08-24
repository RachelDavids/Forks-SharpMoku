using System;

namespace SharpMoku.Utility
{
	public static class Randomizer
	{

		private static readonly Random s_sharedRandom = new();

		public static int GetRandomNumber(int min, int max)
		{
			lock (s_sharedRandom) // synchronize
			{
				return s_sharedRandom.Next(min, max);
			}
		}
	}
}
