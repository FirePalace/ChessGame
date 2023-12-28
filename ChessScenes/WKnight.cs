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
	 public override bool isValidMove(Vector2 tempPos)
    {
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
				
		if(tempTile.X - 2 == prevTile.X || tempTile.X + 2 == prevTile.X)
		{
			if (tempTile.Y - 1 == prevTile.Y || tempTile.Y + 1 == prevTile.Y)
			{
				return true;
			}
		}

		if (tempTile.Y - 2 == prevTile.Y || tempTile.Y + 2 == prevTile.Y)
		{
			if(tempTile.X - 1 == prevTile.X || tempTile.X + 1 == prevTile.X)
			{
				return true;
			}
		}
		return false;
	}
    public override bool isPieceInTheWay(Vector2 tempPos)
    {
        return false;
    }
}
