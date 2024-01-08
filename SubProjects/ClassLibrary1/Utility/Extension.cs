using System;
using System.Linq;

namespace SharpMoku.Utility
{
	public static class Extensions
	{
		public static bool In<T>(this T item, params T[] items)
		{
			return items?.Contains(item) ?? throw new ArgumentNullException(nameof(items));
		}
	}
}
