using Godot;
using System;

public partial class WBishop : ChessPiece
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	public override bool IsValidMove(Vector2 tempPos)
	{
		return IsValidMove( tempPos, prevTile);
	}
	public override bool IsValidMove(Vector2 tempPos, Vector2I tileStart)
	{
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		int xOffset = tempTile.X - tileStart.X;
		int yOffset = tempTile.Y - tileStart.Y;
		if (tempTile != tileStart && !IsPieceInTheWay(tempPos))
		{
			if (yOffset == xOffset || yOffset - xOffset == 0 || yOffset + xOffset == 0)
			{
				return true;
			}
		}
		return false;
	}
}
