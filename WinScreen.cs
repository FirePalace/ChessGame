using System.Data;
using Godot;


public partial class WinScreen : Control
{
	private static RichTextLabel gameOutcomeText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		gameOutcomeText = (RichTextLabel)GetNode("GameOutcomeText");
		gameOutcomeText.PushFontSize(20);
		if (ChessPiece.didWhiteCheckmate != null)
		{
			switch (ChessPiece.didWhiteCheckmate)
			{
				case false:
					gameOutcomeText.PushFontSize(20);
					gameOutcomeText.Text = " Black has won by checkmate! ";
					break;
				case true:
					gameOutcomeText.PushFontSize(20);
					gameOutcomeText.Text = " White has won by checkmate! ";
					break;
			}
		}
		else if (ChessPiece.didStalemateOccur == true)
		{
			gameOutcomeText.Text = " Stalemate :( ";
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	private void _on_game_outcome_text_finished()
	{
	}
}
