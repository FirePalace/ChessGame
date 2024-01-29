using Godot;
using System;

public partial class PromotionMenu : OptionButton
{
	ChessPiece chessPiece = new ChessPiece();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	private void _on_item_selected(int index)
	{
		chessPiece.PromotionHandler(index, true);
	}
	private void _on_item_selected_black(int index)
	{
		chessPiece.PromotionHandler(index, false);
	}
}
