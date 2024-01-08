using System;

namespace SharpMoku
{

	[Serializable]
	public class Position
	{
		public int Row = -1;
		public int Col = -1;
		public static Position Empty => new(-1, -1);
		public bool IsEmpty => Row == -1 && Col == -1;

		public Position(int pRow, int pCol)
		{
			Row = pRow;
			Col = pCol;
		}
		public override int GetHashCode()
		{
			return (Row + "_" + Col).GetHashCode();
		}
		public Position Clone()
		{
			return new(Row, Col);
		}
		public string PositionString()
		{
			return Row + "," + Col.ToString();
		}
		public bool IsEqual(Position pos2)
		{
			return pos2 != null && PositionString() == pos2.PositionString();
		}
		public bool Is(int row, int column)
		{
			return Row == row && Col == column;
		}
	}
}
