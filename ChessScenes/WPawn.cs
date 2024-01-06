using Godot;
using System;


public partial class WPawn : ChessPiece
{
	ChessPiece chessPiece = new ChessPiece();
	bool hasMoved = false;

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
		int direction;

		if (isWhite)
		{
			direction = -1;
		}
		else
		{
			direction = 1;
		}

		if (tempTile != tileStart && !IsPieceInTheWay(tempPos, tileStart))
		{
			if (tempTile.X == tileStart.X)
			{
				//Normal pawn move
				if ((tempTile.Y - tileStart.Y) == direction)
				{
					if (!IsCollisionWithOppositeColor(tempPos))
					{
						hasMoved = true;
						return true;
					}

				}
				//double move forward
				if ((tempTile.Y - tileStart.Y) == 2 * direction && hasMoved == false)
				{
					if (!IsCollisionWithOppositeColor(tempPos))
					{
						hasMoved = true;
						enPassant.isWhite = isWhite;
						enPassant.currentTile = new Vector2I(tempTile.X, tempTile.Y - direction);
						enPassant.chessPiece = this;
						GD.Print("enPassant.currentTile:" + enPassant.currentTile);
						return true;
					}
				}
			}

			//Capturing diagonally
			if (isWhite && xOffset == yOffset && yOffset == -1 || isWhite && Math.Abs(xOffset) - Math.Abs(yOffset) == 0 && yOffset == -1)
			{
				if (IsCollisionWithOppositeColor(tempPos))
				{
					hasMoved = true;
					return true;

				}
				if (enPassant.IsValid && enPassant.isWhite == !isWhite && enPassant.currentTile == tempTile)
				{
					node2D.CallDeferred("remove_child", enPassant.chessPiece);
					enPassant.IsValid = false;
					return true;
				}

			}
			if (!isWhite && xOffset == yOffset && yOffset == 1 || !isWhite && Math.Abs(xOffset) - Math.Abs(yOffset) == 0 && yOffset == 1)
			{

				if (IsCollisionWithOppositeColor(tempPos))
				{
					hasMoved = true;
					return true;
				}
				if (enPassant.IsValid && enPassant.isWhite == !isWhite && enPassant.currentTile == tempTile)
				{
					node2D.CallDeferred("remove_child", enPassant.chessPiece);
					enPassant.IsValid = false;
					return true;
				}
			}
		}
		return false;
	}

}
public struct En_Passant
{
	public Vector2I? currentTile;
	public bool isWhite;
	public ChessPiece chessPiece;
	public bool IsValid
	{
        get
		{
			return chessPiece != null;
		}
		set
		{
			chessPiece = null;
		}
	}
}

