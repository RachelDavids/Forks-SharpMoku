using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;
using SharpMoku.UI.Theme;

namespace SharpMoku.UI
{
	public class PictureBoxGoMoKu : PictureBox
	{
		public class PositionEventArgs : EventArgs
		{
			public Position Value { get; set; }
			public PositionEventArgs(Position position)
			{
				Value = position;
			}
		}

		public Point GetLowerRightPoint(Position position)
		{
			Label L = DicLabel[position.Row.ToString("00") +
				position.Col.ToString("00")];
			// Point point = new Point(L.Top, L.Left);
			Point PointResult = Point.Empty;
			Form f = L.FindForm();
			f.Invoke(new MethodInvoker(delegate () {
				PointResult = L.PointToScreen(Point.Empty);
			}));

			PointResult.X += L.Width / 2;
			PointResult.Y += L.Height / 2;
			PointResult.X += 10;
			PointResult.Y += 10;
			return PointResult;


		}

		public delegate void CellClickHandler(PictureBoxGoMoKu pic, PositionEventArgs positionClick);

		public event CellClickHandler CellClicked;

		private Bitmap BoardImage => CurrentTheme.BoardImage;

		private int LastIndex => NoofRow - 1;
		public int NoofRow { get; set; } = 8;
		public int NoofColumn { get; set; } = 8;
		public int BoardSize => NoofRow;

		private Dictionary<string, Label> DicLabel = [];
		private bool IsRenderNotation = true;
		private Board board = null;
		public int cellWidth { get; private set; } = 0;
		public int cellHeight { get; private set; } = 0;
		public void ReleaseResource()
		{
			board = null;
		}
		public void SetBoad(Board board)
		{
			this.board = board;
		}
		public PictureBoxGoMoKu(Board board, int cellWidth, int cellHeight)
		{
			this.board = board;
			this.cellWidth = cellWidth;
			this.cellHeight = cellHeight;
			NoofRow = board.Matrix.GetLength(0);
			NoofColumn = board.Matrix.GetLength(1);

		}
		private Theme.Theme _CurrentTheme = null;
		public Theme.Theme CurrentTheme {
			get {
				_CurrentTheme ??= ThemeFactory.Create(ThemeFactory.ThemeEnum.Gomoku1);
				return _CurrentTheme;
			}
		}

		private List<Tuple<int, int>> positionIntersectionList {
			get {
				if (_positionIntersectionList == null ||
					_positionIntersectionList.Count == 0)
				{
					LoadPositionIntersection(ref _positionIntersectionList);
				}
				return _positionIntersectionList;
			}
		}

		private List<Tuple<int, int>> _positionIntersectionList = null;
		private void LoadPositionIntersection(ref List<Tuple<int, int>> position)
		{
			position = [];
			int i;
			int j;
			if (BoardSize == 15)
			{
				for (i = 3; i <= 11; i += 4)
				{
					for (j = 3; j <= 11; j += 4)
					{
						position.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			else if (BoardSize == 9)
			{
				for (i = 2; i <= 6; i += 4)
				{
					for (j = 2; j <= 6; j += 4)
					{
						position.Add(new Tuple<int, int>(i, j));
					}
				}
				position.Add(new Tuple<int, int>(4, 4));
			}
			else
			{
				throw new Exception(String.Format("BoardSize must be 9 or 15, it cannot be {0}", BoardSize));
			}
			/* For board size 15
             * The loop above give the same result as these below code
            position.Add(new Tuple<int, int>(3, 3));
            position.Add(new Tuple<int, int>(3, 7));
            position.Add(new Tuple<int, int>(3, 11));
            position.Add(new Tuple<int, int>(7, 3));
            position.Add(new Tuple<int, int>(7, 7));
            position.Add(new Tuple<int, int>(7, 11));
            position.Add(new Tuple<int, int>(11, 3));
            position.Add(new Tuple<int, int>(11, 7));
            position.Add(new Tuple<int, int>(11, 11));
            */

		}
		private bool IsIntersecton(int Row, int Col)
			=> positionIntersectionList.Contains(new Tuple<int, int>(Row, Col));

		public void UpdateTheme(Theme.Theme currentTheme)
		{

		}
		public void Initial(Theme.Theme currentTheme)
		{
			int i;
			int j;
			DicLabel.Clear();
			Controls.Clear();

			if (currentTheme != null)
			{
				_CurrentTheme = currentTheme;
			}
			Label LPreviousLabel = null;
			int SpaceBetweenBorderSize = 0;

			BackgroundImage = null;

			int LastIndex = BoardSize - 1;

			int XOffSet = 0;
			int YOffSet = 0;
			if (IsRenderNotation)
			{
				XOffSet = cellWidth;
				YOffSet = cellHeight;
			}


			SpaceBetweenBorderSize = CurrentTheme.SpaceBetweenBorderSize;
			for (i = 0; i <= LastIndex; i++)
			{
				LPreviousLabel = null;
				for (j = 0; j <= LastIndex; j++)
				{

					ExtendLabel L = new();

					CurrentTheme.ApplyThemeToCell(L);

					L.CellAttribute.Row = i;
					L.CellAttribute.Col = j;
					L.CellAttribute.IsIntersection = IsIntersecton(i, j);

					L.AutoSize = false;
					L.Height = cellHeight;
					L.Width = cellWidth;
					L.BorderStyle = BorderStyle.None;


					if (LPreviousLabel == null)
					{
						L.Top = (i * (L.Height + SpaceBetweenBorderSize)) + SpaceBetweenBorderSize + YOffSet;
						L.Left = (j * L.Width) + SpaceBetweenBorderSize + XOffSet;
					}
					else
					{
						L.Top = LPreviousLabel.Top;
						L.Left = LPreviousLabel.Left + LPreviousLabel.Width + SpaceBetweenBorderSize;
					}

					L.Click -= Label_Click;
					L.Click += Label_Click;
					L.CellAttribute.GoboardPosition = GetBoardPosition(i, j);
					if (CurrentTheme.CustomPaint != null)
					{
						Type t = CurrentTheme.CustomPaint.GetType();
						if (t == typeof(GoMokuPaint))
						{
							L.Parent = this;
							L.BackColor = Color.Transparent;

						}
					}

					Controls.Add(L);
					LPreviousLabel = L;
					DicLabel.Add(i.ToString("00") + j.ToString("00"), L);
				}
			}

			Height = LPreviousLabel.Top + (LPreviousLabel.Height * 2);
			Width = LPreviousLabel.Left + (LPreviousLabel.Width * 2);
			CurrentTheme.ApplyThemeToBoard(this);

			if (IsRenderNotation)
			{
				Paint -= PictureBoxGoMoKu_Paint;
				Paint += PictureBoxGoMoKu_Paint;
			}
		}

		private void CopyImage(Bitmap fromImage, Graphics toGraphic, Rectangle rec)
		{
			toGraphic.DrawImage(fromImage, rec.Left, rec.Top, rec, GraphicsUnit.Pixel);

		}
		public void ReRender()
		{

			SetBoardValueToLabel();
		}
		public void SetBoardValueToLabel()
		{
			int i;
			int j;
			for (i = 0; i < NoofRow; i++)
			{
				for (j = 0; j < NoofColumn; j++)
				{
					ExtendLabel Lbl = (ExtendLabel)DicLabel[i.ToString("00") + j.ToString("00")];
					Board.CellValue cellValue = (Board.CellValue)board.Matrix[i, j];
					Lbl.CellAttribute.IsNeighborCell = board.dicNeighbour.ContainsKey(new Position(i, j).PositionString());
					Lbl.CellAttribute.CellValue = cellValue;
					Lbl.Invalidate();

				}
			}
		}
		private void PictureBoxGoMoKu_Paint(object sender, PaintEventArgs e)
		{

			int LastIndex = BoardSize - 1;
			Label TopLeftLabel = DicLabel["0000"];
			Label TopRghtLabel = DicLabel["00" + LastIndex.ToString("00")];
			Label BottomRightLabel = DicLabel[LastIndex.ToString("00") + LastIndex.ToString("00")];
			Label BottomLeftLabel = DicLabel[LastIndex.ToString("00") + "00"];

			PictureBoxGoMoKu pic = (PictureBoxGoMoKu)sender;
			Rectangle rectAll = new(0, 0, pic.Width, pic.Height);
			if (CurrentTheme.HasImage)
			{

				CopyImage(BoardImage, e.Graphics, rectAll);
			}

			int i;

			/*
             * set it to true to display gomoku Notation
             * set it to false if you want it to be more
             * easy for you to debug
             */
			//[DEBUG:]
			bool isUseGomokuNotation = true;

			for (i = 0; i < BoardSize; i++)
			{
				Label FirstColumnLabel = DicLabel[i.ToString("00") + "00"];
				Label FirstRowLabel = DicLabel["00" + i.ToString("00")];
				StringFormat format = new() {
					Alignment = StringAlignment.Center
				};

				string rowCharacter = i.ToString();
				string colCharacter = i.ToString();

				if (isUseGomokuNotation)
				{
					string row1to15Character = (BoardSize - i).ToString();
					// or 1 to 9

					const int asciiValueForA = 65;
					string colAtoOCharacter = ((char)(asciiValueForA + i)).ToString();
					// or A to I

					rowCharacter = row1to15Character;
					colCharacter = colAtoOCharacter;

				}

				e.Graphics.DrawString(rowCharacter,
						ShareGraphicObject.GoMokuBoardFont,
						ShareGraphicObject.SolidBrush(CurrentTheme.NotationForeColor),
						new Point(FirstColumnLabel.Width / 2, FirstColumnLabel.Top),
						format);
				e.Graphics.DrawString(colCharacter,
					ShareGraphicObject.GoMokuBoardFont,
					ShareGraphicObject.SolidBrush(CurrentTheme.NotationForeColor),
					new Point(FirstRowLabel.Left + 10, 0));

			}
		}

		private void Label_Click(object sender, EventArgs e)
		{

			ExtendLabel labelSender = (ExtendLabel)sender;

			Position pos = new(labelSender.CellAttribute.Row,
				labelSender.CellAttribute.Col);

			PositionEventArgs posEvent = new(pos);
			CellClicked?.Invoke(this, posEvent);

		}

		private void DrawLabel(Label L, int x, int y, int xOffSet, int yOffset)
		{
			Bitmap NewImage = new(L.Width, L.Height);

			Graphics g = Graphics.FromImage(NewImage);

			NewImage.SetResolution(g.DpiX, g.DpiY);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

			int lX = x * L.Width;
			int lY = y * L.Height;

			Rectangle rect = new(lX + xOffSet, lY + yOffset, L.Width, L.Height);
			if (BoardImage != null)
			{
				g.DrawImage(BoardImage, 0, 0, rect, GraphicsUnit.Pixel);
			}

			L.Image = NewImage;

		}

		private Dictionary<string, GomokuCellAttribute.GoBoardPositionEnum> _DicBoardPositionEnum = null;
		private Dictionary<string, GomokuCellAttribute.GoBoardPositionEnum> DicBoardPositionEnum {
			get {
				if (_DicBoardPositionEnum == null)
				{
					_DicBoardPositionEnum = new Dictionary<string, GomokuCellAttribute.GoBoardPositionEnum> {
						{ "0,0", GomokuCellAttribute.GoBoardPositionEnum.TopLeftCorner },
						{ "0," + LastIndex, GomokuCellAttribute.GoBoardPositionEnum.TopRightCorner },
						{ LastIndex + ",0", GomokuCellAttribute.GoBoardPositionEnum.BottomLeftCorner },
						{ LastIndex + "," + LastIndex, GomokuCellAttribute.GoBoardPositionEnum.BottomRightCorner }
					};
					int i;
					for (i = 1; i < LastIndex; i++)
					{
						_DicBoardPositionEnum.Add("0," + i, GomokuCellAttribute.GoBoardPositionEnum.TopBorder);
						_DicBoardPositionEnum.Add(LastIndex + "," + i, GomokuCellAttribute.GoBoardPositionEnum.BottomBorder);
						_DicBoardPositionEnum.Add(i + ",0", GomokuCellAttribute.GoBoardPositionEnum.LeftBorder);
						_DicBoardPositionEnum.Add(i + "," + LastIndex, GomokuCellAttribute.GoBoardPositionEnum.RightBorder);
					}
				}
				return _DicBoardPositionEnum;
			}
		}
		public GomokuCellAttribute.GoBoardPositionEnum GetBoardPosition(int Row, int Col)
		{

			if (!(Row == 0 || Col == 0 || Row == LastIndex || Col == LastIndex))
			{
				return GomokuCellAttribute.GoBoardPositionEnum.Center;
			}

			Position pos = new(Row, Col);

			return !DicBoardPositionEnum.ContainsKey(pos.PositionString())
				? throw new Exception("DicBoard does not contain " + pos.PositionString())
				: DicBoardPositionEnum[pos.PositionString()];
		}
	}
}
