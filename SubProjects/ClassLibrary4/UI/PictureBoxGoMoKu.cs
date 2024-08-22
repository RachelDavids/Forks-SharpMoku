using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SharpMoku.UI.LabelCustomPaint;
using SharpMoku.UI.Theme;

namespace SharpMoku.UI
{
	public partial class PictureBoxGomoku(Board board, int cellWidth, int cellHeight)
				: PictureBox
	{
		public class PositionEventArgs(Position position)
			: EventArgs
		{
			public Position Value { get; set; } = position;
		}

		public Point GetLowerRightPoint(Position position)
		{
			Label label = _labels[position.Row.ToString("00") + position.Col.ToString("00")];
			// Point point = new Point(L.Top, L.Left);
			Point point = Point.Empty;
			Form f = label.FindForm();
			_ = f.Invoke(new MethodInvoker(delegate () {
				point = label.PointToScreen(Point.Empty);
			}));

			point.X += label.Width / 2;
			point.Y += label.Height / 2;
			point.X += 10;
			point.Y += 10;
			return point;

		}

		public delegate void CellClickHandler(PictureBoxGomoku pic, PositionEventArgs positionClick);

		public event CellClickHandler CellClicked;

		private Bitmap BoardImage => CurrentTheme.BoardImage;

		private int LastIndex => RowNumber - 1;
		public int RowNumber { get; set; } = board.Matrix.GetLength(0);
		public int ColumnNumber { get; set; } = board.Matrix.GetLength(1);
		public int BoardSize => RowNumber;

		private readonly Dictionary<string, Label> _labels = [];
		private readonly bool _renderNotation = true;
		private Board _board = board;
		public int CellWidth { get; private set; } = cellWidth;
		public int CellHeight { get; private set; } = cellHeight;
		public void ReleaseResource()
		{
			_board = null;
		}
		public void SetBoard(Board board)
		{
			CheckDisposed();
			_board = board;
		}

		private Theme.Theme _currentTheme = null;
		public Theme.Theme CurrentTheme {
			get {
				_currentTheme ??= ThemeFactory.Create(ThemeFactory.ThemeEnum.Gomoku1);
				return _currentTheme;
			}
		}

		private List<Tuple<int, int>> PositionIntersectionList {
			get {
				if (_positionIntersectionList == null
					|| _positionIntersectionList.Count == 0)
				{
					LoadPositionIntersection(out _positionIntersectionList);
				}
				return _positionIntersectionList;
			}
		}

		private List<Tuple<int, int>> _positionIntersectionList;// = null;
		private void LoadPositionIntersection(out List<Tuple<int, int>> position)
		{
			position = [];
			int start, end, delta;
			if (BoardSize == 15)
			{
				(start, end, delta) = (3, 11, 4);
			}
			else if (BoardSize == 9)
			{
				(start, end, delta) = (2, 6, 4);
			}
			else
			{
				throw new Exception($"BoardSize must be 9 or 15, it cannot be {BoardSize}");
			}
			for (int i = start; i <= end; i += delta)
			{
				for (int j = start; j <= end; j += delta)
				{
					position.Add(new Tuple<int, int>(i, j));
				}
				int centre = (BoardSize / 2) - 1;
				Tuple<int, int> t = new(centre, centre);
				if (!position.Contains(t))
				{
					position.Add(t);
				}
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
		private bool IsIntersection(int row, int col)
			=> PositionIntersectionList.Contains(new Tuple<int, int>(row, col));

		//public void UpdateTheme(Theme.Theme currentTheme)
		//{

		//}
		public void Initial(Theme.Theme currentTheme)
		{
			_labels.Clear();
			Controls.Clear();

			if (currentTheme != null)
			{
				_currentTheme = currentTheme;
			}
			Label previousLabel = null;
			int spaceBetweenBorderSize = 0;

			BackgroundImage = null;

			int lastIndex = BoardSize - 1;

			int xOffSet = 0;
			int yOffSet = 0;
			if (_renderNotation)
			{
				xOffSet = CellWidth;
				yOffSet = CellHeight;
			}

			spaceBetweenBorderSize = CurrentTheme.SpaceBetweenBorderSize;
			for (int row = 0; row <= lastIndex; row++)
			{
				previousLabel = null;
				for (int col = 0; col <= lastIndex; col++)
				{

					ExtendLabel l = new();

					CurrentTheme.ApplyThemeToCell(l);

					l.CellDetail.Row = row;
					l.CellDetail.Col = col;
					l.CellDetail.IsIntersection = IsIntersection(row, col);

					l.AutoSize = false;
					l.Height = CellHeight;
					l.Width = CellWidth;
					l.BorderStyle = BorderStyle.None;

					if (previousLabel == null)
					{
						l.Top = (row * (l.Height + spaceBetweenBorderSize)) + spaceBetweenBorderSize + yOffSet;
						l.Left = (col * l.Width) + spaceBetweenBorderSize + xOffSet;
					}
					else
					{
						l.Top = previousLabel.Top;
						l.Left = previousLabel.Left + previousLabel.Width + spaceBetweenBorderSize;
					}

					l.Click -= Label_Click;
					l.Click += Label_Click;
					l.CellDetail.GoBoardPosition = GetBoardPosition(row, col);
					if (CurrentTheme.CustomPaint != null)
					{
						Type t = CurrentTheme.CustomPaint.GetType();
						if (t == typeof(GoMokuPaint))
						{
							l.Parent = this;
							l.BackColor = Color.Transparent;

						}
					}

					Controls.Add(l);
					previousLabel = l;
					_labels.Add(row.ToString("00") + col.ToString("00"), l);
				}
			}

			Height = previousLabel.Top + (previousLabel.Height * 2);
			Width = previousLabel.Left + (previousLabel.Width * 2);
			CurrentTheme.ApplyThemeToBoard(this);

			if (_renderNotation)
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
			for (i = 0; i < RowNumber; i++)
			{
				for (j = 0; j < ColumnNumber; j++)
				{
					ExtendLabel Lbl = (ExtendLabel)_labels[i.ToString("00") + j.ToString("00")];
					CellValue cellValue = (CellValue)_board.Matrix[i, j];
					Lbl.CellDetail.IsNeighborCell = _board.dicNeighbour.ContainsKey(new Position(i, j).PositionString());
					Lbl.CellDetail.CellValue = cellValue;
					Lbl.Invalidate();

				}
			}
		}
		private void PictureBoxGoMoKu_Paint(object sender, PaintEventArgs e)
		{

			//int lastIndex = BoardSize - 1;
			//Label TopLeftLabel = Labels["0000"];
			//Label TopRghtLabel = Labels["00" + lastIndex.ToString("00")];
			//Label BottomRightLabel = Labels[lastIndex.ToString("00") + lastIndex.ToString("00")];
			//Label BottomLeftLabel = Labels[lastIndex.ToString("00") + "00"];
			PictureBoxGomoku pic = (PictureBoxGomoku)sender;
			Rectangle rectAll = new(0, 0, pic.Width, pic.Height);
			if (CurrentTheme.HasImage)
			{

				CopyImage(BoardImage, e.Graphics, rectAll);
			}
			/*
             * set to true to display gomoku Notation
             * set to false if you want it to be easier
			 * for you to debug
             */
			//[DEBUG:]
			bool useGomokuNotation = true;

			for (int i = 0; i < BoardSize; i++)
			{
				Label firstColumnLabel = _labels[i.ToString("00") + "00"];
				Label firstRowLabel = _labels["00" + i.ToString("00")];
				StringFormat format = new() {
					Alignment = StringAlignment.Center
				};

				string rowCharacter = i.ToString();
				string colCharacter = i.ToString();

				if (useGomokuNotation)
				{
					string row1To15Character = (BoardSize - i).ToString();
					// or 1 to 9
					const int asciiValueForA = 65;
					string colAtoOCharacter = ((char)(asciiValueForA + i)).ToString();
					// or A to I
					rowCharacter = row1To15Character;
					colCharacter = colAtoOCharacter;

				}

				e.Graphics.DrawString(rowCharacter,
									  ShareGraphicObject.GoMokuBoardFont,
									  ShareGraphicObject.SolidBrush(CurrentTheme.NotationForeColor),
									  new Point(firstColumnLabel.Width / 2, firstColumnLabel.Top),
									  format);
				e.Graphics.DrawString(colCharacter,
									  ShareGraphicObject.GoMokuBoardFont,
									  ShareGraphicObject.SolidBrush(CurrentTheme.NotationForeColor),
									  new Point(firstRowLabel.Left + 10, 0));

			}
		}

		private void Label_Click(object sender, EventArgs e)
		{

			ExtendLabel labelSender = (ExtendLabel)sender;

			Position pos = new(labelSender.CellDetail.Row,
				labelSender.CellDetail.Col);

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

		private Dictionary<string, GoBoardPosition> _DicBoardPositionEnum = null;
		private Dictionary<string, GoBoardPosition> DicBoardPositionEnum {
			get {
				if (_DicBoardPositionEnum == null)
				{
					_DicBoardPositionEnum = new Dictionary<string, GoBoardPosition> {
																						{ "0,0", GoBoardPosition.TopLeftCorner },
																						{ "0," + LastIndex, GoBoardPosition.TopRightCorner },
																						{ LastIndex + ",0", GoBoardPosition.BottomLeftCorner },
																						{ LastIndex + "," + LastIndex, GoBoardPosition.BottomRightCorner }
																					};
					int i;
					for (i = 1; i < LastIndex; i++)
					{
						_DicBoardPositionEnum.Add("0," + i, GoBoardPosition.TopBorder);
						_DicBoardPositionEnum.Add(LastIndex + "," + i, GoBoardPosition.BottomBorder);
						_DicBoardPositionEnum.Add(i + ",0", GoBoardPosition.LeftBorder);
						_DicBoardPositionEnum.Add(i + "," + LastIndex, GoBoardPosition.RightBorder);
					}
				}
				return _DicBoardPositionEnum;
			}
		}
		public GoBoardPosition GetBoardPosition(int Row, int Col)
		{

			if (!(Row == 0 || Col == 0 || Row == LastIndex || Col == LastIndex))
			{
				return GoBoardPosition.Center;
			}

			Position pos = new(Row, Col);

			return !DicBoardPositionEnum.ContainsKey(pos.PositionString())
				? throw new Exception("DicBoard does not contain " + pos.PositionString())
				: DicBoardPositionEnum[pos.PositionString()];
		}
	}
}
