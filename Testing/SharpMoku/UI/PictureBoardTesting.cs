using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharpMoku.UI;

namespace Testing.SharpMoku.UI
{
	[TestClass]
	public class PictureBoardTesting
	{
		[DataRow(15)]
		[DataRow(9)]
		[TestMethod]
		public void GetBoardPosition(int size)
		{
			using PictureBoxGomoku pic = new(new(size), 38, 38);
			int lastIndex = pic.BoardSize - 1;
			GoBoardPosition pos = pic.GetBoardPosition(0, lastIndex);
			Assert.AreEqual(pos, GoBoardPosition.TopRightCorner);
			pos = pic.GetBoardPosition(0, 0);
			Assert.AreEqual(pos, GoBoardPosition.TopLeftCorner);
			pos = pic.GetBoardPosition(lastIndex, 0);
			Assert.AreEqual(pos, GoBoardPosition.BottomLeftCorner);
			pos = pic.GetBoardPosition(lastIndex, lastIndex);
			Assert.AreEqual(pos, GoBoardPosition.BottomRightCorner);
			pos = pic.GetBoardPosition(0, 1);
			Assert.AreEqual(pos, GoBoardPosition.TopBorder);
			int i;
			for (i = 1; i < lastIndex; i++)
			{
				pos = pic.GetBoardPosition(0, i);
				Assert.AreEqual(pos, GoBoardPosition.TopBorder);
				pos = pic.GetBoardPosition(lastIndex, i);
				Assert.AreEqual(pos, GoBoardPosition.BottomBorder);
				pos = pic.GetBoardPosition(i, 0);
				Assert.AreEqual(pos, GoBoardPosition.LeftBorder);
				pos = pic.GetBoardPosition(i, lastIndex);
				Assert.AreEqual(pos, GoBoardPosition.RightBorder);
			}
		}
	}
}
