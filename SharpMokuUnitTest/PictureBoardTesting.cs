using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharpMoku.UI;

namespace Testing.SharpMoku
{
	[TestClass]
	public class PictureBoardTesting
	{
		[TestMethod]
		public void GetBoardPosition()
		{

			PictureBoxGomoku pic = new(new global::SharpMoku.Board(15), 38, 38);
			//bool IsUseNotation = true;
			int LastIndex = 14;
			global::SharpMoku.GoBoardPosition boardPo = pic.GetBoardPosition(0, 14);

			Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.TopRightCorner);

			boardPo = pic.GetBoardPosition(0, 0);
			Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.TopLeftCorner);

			boardPo = pic.GetBoardPosition(LastIndex, 0);
			Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.BottomLeftCorner);

			boardPo = pic.GetBoardPosition(LastIndex, LastIndex);
			Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.BottomRightCorner);

			boardPo = pic.GetBoardPosition(0, 1);
			Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.TopBorder);
			int i;
			for (i = 1; i <= 13; i++)
			{
				boardPo = pic.GetBoardPosition(0, i);
				Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.TopBorder);

				boardPo = pic.GetBoardPosition(LastIndex, i);
				Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.BottomBorder);

				boardPo = pic.GetBoardPosition(i, 0);
				Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.LeftBorder);

				boardPo = pic.GetBoardPosition(i, LastIndex);
				Trace.Assert(boardPo == global::SharpMoku.GoBoardPosition.RightBorder);

			}
		}
	}
}
