using Godot;
using System;

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
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		int xOffset = tempTile.X - prevTile.X;
		int yOffset = tempTile.Y - prevTile.Y;
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

		if (tempTile != prevTile && !IsPieceInTheWay(tempPos) && !IsCheck(this.isWhite))
		{
			if (tempTile.X == prevTile.X || tempTile.Y == prevTile.Y)
			{
				if (tempTile.Y - prevTile.Y == -1 || tempTile.Y - prevTile.Y == 1 || tempTile.X - prevTile.X == -1 || tempTile.X - prevTile.X == 1)
				{
					hasMoved = true;
					return true;
				}
				if (this.Name == "WKing")
				{
					if ((tempTile.X - prevTile.X == shortCastleDirection) && hasMoved == false)
					{
						CastleHandler( new Vector2(7, 7), this, "shortCastle");
						GD.Print("CastleHandler called shortCastle");
						hasMoved = true;
						return true;
					}
					if ((tempTile.X - prevTile.X == longCastleDirection) && hasMoved == false)
					{
						CastleHandler (new Vector2(0, 7), this, "longCastle");
						GD.Print("CastleHandler called longCastle");
						hasMoved = true;
						return true;
					}
				}
				else
				{
					

					if ((tempTile.X - prevTile.X == shortCastleDirection) && hasMoved == false)
					{
						CastleHandler(new Vector2(7, 0), this, "shortCastle");
						GD.Print("CastleHandler called shortCastle");
						hasMoved = true;
						return true;
					}
					if ((tempTile.X - prevTile.X == longCastleDirection) && hasMoved == false)
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

		return false;
	}
}
