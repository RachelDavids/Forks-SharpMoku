using System;

using SharpMoku.Logging;
using SharpMoku.UI.Theme;

namespace SharpMoku
{
	[Serializable]
	public class SharpMokuSettings
		: ILogSettings
	{
		public int BotDepth { get; set; } = 1;
		public int BoardSize { get; set; } = 9;
		public GameMode GameMode { get; set; } = GameMode.PlayerVsBot;
		public KnownTheme KnownTheme { get; set; } = KnownTheme.Gomoku1;
		public bool IsUseBotMouseMove { get; set; } = true;
		public bool IsWriteLog { get; set; }// = false;
		public bool IsAllowUndo { get; set; }// = false;

	}
}
