using Godot;
using System;

public partial class WKing : ChessPiece
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
		
		
		
		if(tempTile.X == prevTile.X || tempTile.Y == prevTile.Y || tempTile.X - 1 == prevTile.X || tempTile.Y -1 == prevTile.Y || tempTile.X + 1 == prevTile.X || tempTile.Y + 1 == prevTile.Y )
		{
		
			// up down left right
			if(tempTile.Y - prevTile.Y == -1 ||tempTile.Y - prevTile.Y == 1|| tempTile.X - prevTile.X == -1 ||  tempTile.X - prevTile.X == 1 )
			{
				return true;
			}
			
		}

		return false;
	}
}
