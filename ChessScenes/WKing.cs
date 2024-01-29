using Godot;
using System;
using System.Collections.Generic;

public partial class WKing : ChessPiece
{
	public bool hasMoved = false;
	public bool isInCheck = false;
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
		this.ignoreCheck = true;
		try
		{
			Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
			int xOffset = tempTile.X - tileStart.X;
			int yOffset = tempTile.Y - tileStart.Y;
			int shortCastleDirection;
			int longCastleDirection;

			if (isWhite)
			{
				shortCastleDirection = 2;
				longCastleDirection = -2;
			}
			else
			{
				shortCastleDirection = 2;
				longCastleDirection = -2;
			}

			if (tempTile != tileStart && !IsPieceInTheWay(tempPos) && IsCheck(this.isWhite) == null)
			{
				if (tempTile.X == tileStart.X || tempTile.Y == tileStart.Y)
				{
					if (tempTile.Y - tileStart.Y == -1 || tempTile.Y - tileStart.Y == 1 || tempTile.X - tileStart.X == -1 || tempTile.X - tileStart.X == 1)
					{
						hasMoved = true;
						return true;
					}
					if (this.Name == "wKing")
					{
						if ((tempTile.X - tileStart.X == shortCastleDirection) && hasMoved == false && !HasTargetRookMoved(new Vector2I(7, 7)))
						{
							CastleHandler(new Vector2(7, 7), this, "shortCastle");
							GD.Print("CastleHandler called shortCastle");
							hasMoved = true;
							return true;
						}
						if ((tempTile.X - tileStart.X == longCastleDirection) && hasMoved == false && !HasTargetRookMoved(new Vector2I(0, 7)))
						{
							CastleHandler(new Vector2(0, 7), this, "longCastle");
							GD.Print("CastleHandler called longCastle");
							hasMoved = true;
							return true;
						}
					}
					else
					{


						if ((tempTile.X - tileStart.X == shortCastleDirection) && hasMoved == false && !HasTargetRookMoved(new Vector2I(7, 0)))
						{
							CastleHandler(new Vector2(7, 0), this, "shortCastle");
							GD.Print("CastleHandler called shortCastle");
							hasMoved = true;
							return true;
						}
						if ((tempTile.X - tileStart.X == longCastleDirection) && hasMoved == false && !HasTargetRookMoved(new Vector2I(0, 0)))
						{
							CastleHandler(new Vector2(0, 0), this, "longCastle");
							GD.Print("CastleHandler called longCastle");
							hasMoved = true;
							return true;
						}
					}
				}
				if (Math.Abs(yOffset) == 1 && Math.Abs(xOffset) == 1)
				{
					if (yOffset == xOffset || yOffset - xOffset == 0 || yOffset + xOffset == 0)
					{
						hasMoved = true;
						return true;
					}
				}
			}
		}
		finally
		{
			this.ignoreCheck = false;
		}
		return false;
	}
	public override bool HasValidMove()
	{
		List<Vector2I> listOfPotentialValidTiles = new List<Vector2I>();
		Vector2I tileStart = tileMap.FromGlobalPosToTile((Vector2I)this.Position);

		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X, tileStart.Y + 1));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X, tileStart.Y - 1));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X + 1, tileStart.Y));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X - 1, tileStart.Y));

		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X - 1, tileStart.Y - 1));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X + 1, tileStart.Y + 1));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X - 1, tileStart.Y + 1));
		listOfPotentialValidTiles.Add(new Vector2I(tileStart.X + 1, tileStart.Y - 1));

		listOfPotentialValidTiles.RemoveAll(r => r.X > 7 || r.X < 0 || r.Y > 7 || r.Y < 0 || r.Equals(tileStart));

		foreach (Vector2I tile in listOfPotentialValidTiles)
		{
			Vector2I tempPos = tileMap.FromTileToGlobalPos(tile);


			//TODO
			if ((!IsCollision(tempPos) || IsCollisionWithOppositeColor(tempPos)) && !IsPieceInTheWay(tempPos, tileStart) && IsCheck(tempPos, this.isWhite) == null)
			{
				return true;
			}
		}



		return false;
	}
}
