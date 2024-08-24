using System;

using SharpMoku.Board;
using SharpMoku.UI;

namespace Testing.SharpMoku
{
	public sealed class MockUi : IUI
	{
		public event EventHandler<PositionEventArgs> CellClicked;
		public event EventHandler HasFinishedMoveCursor;

		public void OnBotFinishedThinking(object sender, EventArgs e)
		{
			// throw new NotImplementedException();
		}

		public void OnBotThinking(object sender, EventArgs e)
		{
			// throw new NotImplementedException();
		}

		public void OnGameFinished(object sender, EventArgs e)
		{
			// throw new NotImplementedException();
		}

		public void MoveCursorTo(Position position)
		{
			// throw new NotImplementedException();
			HasFinishedMoveCursor?.Invoke(this, new EventArgs());
		}

		public void RenderUI()
		{
			// throw new NotImplementedException();
		}

		public void PutStoneByUI(int row, int column)
		{
			CellClicked?.Invoke(this, new PositionEventArgs(new Position(row, column)));
		}
	}
}
