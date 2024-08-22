using System;

namespace SharpMoku
{
	public class WinStatusEventArgs(WinStatus winStatus)
		: EventArgs
	{
		public WinStatus Winstatus { get; set; } = winStatus;
	}
}
