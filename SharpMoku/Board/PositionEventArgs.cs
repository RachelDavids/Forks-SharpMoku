using System;

namespace SharpMoku.Board
{
	public class PositionEventArgs(Position position)
		: EventArgs
	{
		public Position Value { get; set; } = position;
	}
}
