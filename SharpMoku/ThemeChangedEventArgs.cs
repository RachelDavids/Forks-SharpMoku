using System;

using SharpMoku.UI.Theme;

namespace SharpMoku
{
	public class ThemeChangedEventArgs(Theme theme)
		: EventArgs
	{
		public Theme Theme { get; private set; } = theme;
	}
}
