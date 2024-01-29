using System.Collections.Generic;
using Godot;
public partial class Rook : ChessPiece
{
	public bool hasMoved = true;

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
	public override bool HasValidMove()
	{
		List<Vector2I> listOfPotentialValidTiles = new List<Vector2I>();
		Vector2I tileStart = tileMap.FromGlobalPosToTile((Vector2I)this.Position);


		for (int i = 0; i < 8; i++)
		{
			listOfPotentialValidTiles.Add(new Vector2I(i, tileStart.Y));
			listOfPotentialValidTiles.Add(new Vector2I(tileStart.X, i));
		}
		listOfPotentialValidTiles.RemoveAll(r => r.Equals(tileStart));
		
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
