using System;

namespace SharpMoku
{
	public class PositionEventArgs : EventArgs
	{
		public Position Value { get; set; }
		public PositionEventArgs(Position position)
		{
			Value = position;
		}
	}
}
