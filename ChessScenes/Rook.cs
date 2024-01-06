using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

using System.Runtime.Serialization;




public partial class Rook : ChessPiece
{
public bool hasMoved = false;

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
		if (tempTile != tileStart && !IsPieceInTheWay(tempPos, tileStart))
		{
			if (tempTile.X == tileStart.X || tempTile.Y == tileStart.Y)
			{
				hasMoved = true;
				return true;
				
			}
		}
		return false;
	}
}
