using System;

using SharpMoku.UI.Theme;

namespace SharpMoku
{
	[Serializable]
	public class SharpMokuSettings
		: ILogSettings
	{
		public int BotDepth { get; set; } = 1;
		public int BoardSize { get; set; } = 9;
		public Game.GameModeEnum GameMode { get; set; } = Game.GameModeEnum.PlayerVsBot;
		public ThemeFactory.ThemeEnum ThemeEnum { get; set; } = ThemeFactory.ThemeEnum.Gomoku1;
		public bool IsUseBotMouseMove { get; set; } = true;
		public bool IsWriteLog { get; set; } = false;
		public bool IsAllowUndo { get; set; } = false;

	}
}
