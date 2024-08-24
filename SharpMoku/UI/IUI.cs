using System;

using SharpMoku.Board;

namespace SharpMoku.UI
{
	public interface IUI
	{

		// Raise event CellClicked so the game object will handle the operation
		event EventHandler<PositionEventArgs> CellClicked;

		void RenderUI();
		void MoveCursorTo(Position position);

		//To tell the game object that bot has finished moving a cursor to the position to put.
		event EventHandler HasFinishedMoveCursor;

		/*
         These 3 Method will be trigged from Game Object.
         OnGameFinished : UI wil display the result.
         OnBotThinking : UI will change the cursor to an hourglass.
         OnBotFinishedThinking : Ui will change the cursor back to default cursor and allow user to input
        */
		void OnGameFinished(object sender, EventArgs e);
		void OnBotThinking(object sender, EventArgs e);
		void OnBotFinishedThinking(object sender, EventArgs e);
	}
}
