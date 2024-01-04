using Godot;
using System;

public partial class WKnight : ChessPiece
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
		if (tempTile != tileStart)
		{

			if (tempTile.X - 2 == tileStart.X || tempTile.X + 2 == tileStart.X)
			{
				if (tempTile.Y - 1 == tileStart.Y || tempTile.Y + 1 == tileStart.Y)
				{
					return true;
				}
			}

			if (tempTile.Y - 2 == tileStart.Y || tempTile.Y + 2 == tileStart.Y)
			{
				if (tempTile.X - 1 == tileStart.X || tempTile.X + 1 == tileStart.X)
				{
					return true;
				}
			}

		}
		return false;
	}
	public override bool IsPieceInTheWay(Vector2 tempPos)
	{
		return IsPieceInTheWay( tempPos, prevTile);
	}
	public override bool IsPieceInTheWay(Vector2 tempPos, Vector2I tileStart)
	{
		return false;
	}
}
