using System;

namespace SharpMoku
{
	public class PositionEventArgs(Position position)
		: EventArgs
	{
		public Position Value { get; set; } = position;
	}
}
