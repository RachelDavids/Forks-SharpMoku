using System;
using System.Collections.Generic;
using System.Drawing;

namespace SharpMoku.UI.Theme
{
	public class ThemeFactory
	{

		public enum ThemeEnum
		{
			Gomoku1 = 0,
			Gomoku2,
			Gomoku3,
			Gomoku4,
			Gomoku5,
			TicTacToe1,
			TicTacToe2,
			TicTacToe3,
			TableTennis,

		}

		//Credit board file name
		//https://yewang.github.io/besogo/img/shinkaya1.jpg
		private static readonly string BoardFileName = Utility.FileUtility.ResourcesPath + @"\shinkaya1.jpg";

		//Credit Stone image
		//https://github.com/featurecat/lizzie
		private static readonly string WhiteStoneFilePath = Utility.FileUtility.ResourcesPath + @"\lizzie_White0.png";
		private static readonly string BlackStoneFilePath = Utility.FileUtility.ResourcesPath + @"\lizzie_Black0.png";
		private static Dictionary<ThemeEnum, Color> dicBackColor = null;
		private static Dictionary<ThemeEnum, Color> dicForeColor = null;

		public static Color BackColor(ThemeEnum theme)
		{
			dicBackColor ??= new Dictionary<ThemeEnum, Color> {
				{ ThemeEnum.Gomoku1, Color.FromArgb(253, 188, 68) },
				{ ThemeEnum.Gomoku2, Color.FromArgb(167, 145, 82) },
				{ ThemeEnum.Gomoku3, Color.FromArgb(0, 43, 54) },
				{ ThemeEnum.Gomoku4, Color.FromArgb(215, 192, 174) },
				{ ThemeEnum.Gomoku5, Color.FromArgb(196, 215, 178) },
				{ ThemeEnum.TicTacToe1, Color.FromArgb(40, 50, 70) },
				{ ThemeEnum.TicTacToe2, Color.FromArgb(53, 152, 219) },
				{ ThemeEnum.TicTacToe3, Color.White },
				{ ThemeEnum.TableTennis, Color.FromArgb(30, 143, 213) }
			};
			return dicBackColor[theme];

		}
		public static Color ForeColor(ThemeEnum theme)
		{
			dicForeColor ??= new Dictionary<ThemeEnum, Color> {
				{ ThemeEnum.Gomoku1, Color.Black },
				{ ThemeEnum.Gomoku2, Color.FromArgb(42, 68, 61) },
				{ ThemeEnum.Gomoku3, Color.White },
				{ ThemeEnum.Gomoku4, Color.FromArgb(96, 108, 93) },
				{ ThemeEnum.Gomoku5, Color.Black },
				{ ThemeEnum.TicTacToe1, Color.White },
				{ ThemeEnum.TicTacToe2, Color.White },
				{ ThemeEnum.TicTacToe3, Color.Black },
				{ ThemeEnum.TableTennis, Color.White }
			};
			return dicForeColor[theme];
		}
		public static Theme Create(ThemeEnum theme)
		{

			return theme switch {
				ThemeEnum.Gomoku1 => new GomokuThemeBuilder().BoardImageFile(BoardFileName)
					.WhiteStoneImagePath(WhiteStoneFilePath)
					.BlackStoneImagePath(BlackStoneFilePath)
					.Build(),
				ThemeEnum.Gomoku2 => new GomokuThemeBuilder()
					.BoardBackColor(Color.FromArgb(167, 145, 82))
					.WhiteStoneBackColor(Color.FromArgb(238, 232, 213))
					.WhiteStoneBorderColor(Color.Black)
					.BlackStoneBackColor(Color.FromArgb(0, 43, 54))
					.NotationForeColor(Color.FromArgb(42, 68, 61))
					.PenTable(ShareGraphicObject.Pen(Color.FromArgb(37, 61, 54), 0.8f))
					.PenBorder(ShareGraphicObject.Pen(Color.FromArgb(37, 61, 54), 2f))
					.Build(),
				ThemeEnum.Gomoku3 => new GomokuThemeBuilder()
					.BoardBackColor(Color.FromArgb(0, 43, 54))
					.WhiteStoneBackColor(Color.FromArgb(238, 232, 213))
					.WhiteStoneBorderColor(Color.FromArgb(88, 110, 117))
					.BlackStoneBackColor(Color.FromArgb(88, 110, 117))
					.BlackStoneBorderColor(Color.FromArgb(88, 110, 117))
					.NotationForeColor(Color.White)
					.PenTable(ShareGraphicObject.Pen(Color.FromArgb(66, 93, 102), 1.8f))
					.Build(),
				ThemeEnum.Gomoku4 => new GomokuThemeBuilder()
					.BoardBackColor(Color.FromArgb(215, 192, 174))
					.WhiteStoneBackColor(Color.FromArgb(150, 126, 118))
					.WhiteStoneBorderColor(Color.FromArgb(150, 126, 118))
					.BlackStoneBackColor(Color.FromArgb(238, 227, 203))
					.BlackStoneBorderColor(Color.FromArgb(238, 227, 203))
					.NotationForeColor(Color.FromArgb(96, 108, 93))
					.PenTable(ShareGraphicObject.Pen(Color.FromArgb(66, 93, 102), 1.8f))
					.Build(),
				ThemeEnum.Gomoku5 => new GomokuThemeBuilder()

					.BoardBackColor(Color.FromArgb(196, 215, 178))
					.BlackStoneBackColor(Color.FromArgb(247, 255, 229))
					.BlackStoneBorderColor(Color.FromArgb(247, 255, 229))
					.WhiteStoneBackColor(Color.FromArgb(160, 196, 157))
					.WhiteStoneBorderColor(Color.FromArgb(160, 196, 157))
					.NotationForeColor(Color.Black)
					.PenTable(ShareGraphicObject.Pen(Color.FromArgb(66, 93, 102), 1.8f))
					.Build(),
				ThemeEnum.TicTacToe1 => new SleekTheme(),
				ThemeEnum.TicTacToe2 => new TicTacToe2Theme(),
				ThemeEnum.TicTacToe3 => new TicTacToe3Theme(),
				ThemeEnum.TableTennis => new TableTennisTheme(),
				_ => throw new ArgumentException("ThemeEnum is not valid"),
			};
		}
	}
}
