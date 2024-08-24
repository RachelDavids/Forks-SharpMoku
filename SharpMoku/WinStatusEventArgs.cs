using System;

using SharpMoku.Board;

namespace SharpMoku
{
	public class WinStatusEventArgs(WinStatus winStatus)
		: EventArgs
	{
		public WinStatus WinStatus { get; set; } = winStatus;
	}
}
