using Godot;
using System;
using System.Collections.Generic;

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
		return IsValidMove(tempPos, prevTile);
	}

	public override bool IsValidMove(Vector2 tempPos, Vector2I tileStart)
	{
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		int xOffset = tempTile.X - tileStart.X;
		int yOffset = tempTile.Y - tileStart.Y;
		if (tempTile != tileStart && !IsPieceInTheWay(tempPos, tileStart))
		{
			if (yOffset == xOffset || yOffset - xOffset == 0 || yOffset + xOffset == 0)
			{
				return true;
			}
		}
		return false;
	}
	public override bool HasValidMove()
	{
		List<Vector2I> listOfPotentialValidTiles = new List<Vector2I>();
		Vector2I tileStart = tileMap.FromGlobalPosToTile((Vector2I)this.Position);

		for (int i = 0; i < 8; i++)
		{
			listOfPotentialValidTiles.Add(new Vector2I(tileStart.X + i, tileStart.Y + i));
			listOfPotentialValidTiles.Add(new Vector2I(tileStart.X - i, tileStart.Y - i));
			listOfPotentialValidTiles.Add(new Vector2I(tileStart.X - i, tileStart.Y + i));
			listOfPotentialValidTiles.Add(new Vector2I(tileStart.X + i, tileStart.Y - i));
		}
		listOfPotentialValidTiles.RemoveAll(r => r.X > 7 || r.X < 0 || r.Y > 7 || r.Y < 0 || r.Equals(tileStart));
		
		foreach (Vector2I tile in listOfPotentialValidTiles)
		{
			Vector2I tempPos = tileMap.FromTileToGlobalPos(tile);
			if (!IsPieceInTheWay(tempPos, tileStart) && (!IsCollision(tempPos) || IsCollisionWithOppositeColor(tempPos)) && IsCheck(this.isWhite) == null)
			{
				return true;
			}
		}

		return false;
	}
}
