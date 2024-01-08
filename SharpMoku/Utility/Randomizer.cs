using System;

namespace SharpMoku.Utility
{
	public static class Randomizer
	{

		private static readonly Random SharedRandom = new();

		public static int GetRandomNumber(int min, int max)
		{
			lock (SharedRandom) // synchronize
			{
				return SharedRandom.Next(min, max);
			}
		}
	}
}
