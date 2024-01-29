using System.Data;
using Godot;


public partial class WinScreen : Control
{
	private static RichTextLabel gameOutcomeText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		gameOutcomeText = (RichTextLabel)GetNode("GameOutcomeText");
		if (ChessPiece.didWhiteCheckmate != null)
		{
			switch (ChessPiece.didWhiteCheckmate)
			{
				case false:

					gameOutcomeText.Text = "[font_size={20}][center]Black has won by checkmate!";
					break;
				case true:

					gameOutcomeText.Text = "[font_size={20}][center]White has won by checkmate! ";
					break;
			}
			ChessPiece.didWhiteCheckmate = null;
		}
		else if (ChessPiece.didStalemateOccur == true)
		{
			gameOutcomeText.Text = "[font_size={20}][center]Stalemate :( ";
			ChessPiece.didStalemateOccur = false;
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
	private void _on_play_again_pressed()
	{
		GetTree().ChangeSceneToFile("res://ChessScenes/menu.tscn");
	}
}
