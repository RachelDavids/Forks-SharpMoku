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

			PictureBoxGoMoKu pic = new PictureBoxGoMoKu(new global::SharpMoku.Board(15), 38, 38);
			//bool IsUseNotation = true;
			int LastIndex = 14;
			global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum boardPo = pic.GetBoardPosition(0, 14);

			Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.TopRightCorner);

			boardPo = pic.GetBoardPosition(0, 0);
			Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.TopLeftCorner);

			boardPo = pic.GetBoardPosition(LastIndex, 0);
			Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.BottomLeftCorner);

			boardPo = pic.GetBoardPosition(LastIndex, LastIndex);
			Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.BottomRightCorner);

			boardPo = pic.GetBoardPosition(0, 1);
			Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.TopBorder);
			int i;
			for (i = 1; i <= 13; i++)
			{
				boardPo = pic.GetBoardPosition(0, i);
				Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.TopBorder);

				boardPo = pic.GetBoardPosition(LastIndex, i);
				Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.BottomBorder);

				boardPo = pic.GetBoardPosition(i, 0);
				Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.LeftBorder);

				boardPo = pic.GetBoardPosition(i, LastIndex);
				Trace.Assert(boardPo == global::SharpMoku.GomokuCellAttribute.GoBoardPositionEnum.RightBorder);

			}
		}
	}
}
