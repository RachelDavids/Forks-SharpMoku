namespace SharpMoku.UI
{
	public sealed class GomokuCellDetail
	{
		public GoBoardPosition GoBoardPosition { get; set; } = GoBoardPosition.Center;
		public int Row { get; set; }
		public int Col { get; set; }
		public bool IsIntersection { get; set; } = false;
		public CellValue CellValue { get; set; }
		public bool IsNeighborCell { get; set; } = false;

	}
}
