using Godot;
using System;
using System.Net.NetworkInformation;


public partial class TileMap : Godot.TileMap
{
	Node2D node2D;
	static int callcount = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		node2D = GetNode<Node2D>("..");
		FENReady();
	}
	public void FENReady()
	{
		RemoveAllPieces();

		foreach (FENPiece piece in Menu.fenPieces)
		{
			ChessPiece chessPiece = null;
			if (piece.pieceType.Contains("rook"))
			{
				chessPiece = new Rook();
			}
			else if (piece.pieceType.Contains("bishop"))
			{
				chessPiece = new WBishop();
			}
			else if (piece.pieceType.Contains("knight"))
			{
				chessPiece = new WKnight();
			}
			else if (piece.pieceType.Contains("queen"))
			{
				chessPiece = new WQueen();
			}
			else if (piece.pieceType.Contains("king"))
			{
				chessPiece = new WKing();
			}
			else if (piece.pieceType.Contains("pawn"))
			{
				chessPiece = new WPawn();
			}

			SpawnPiece(chessPiece, piece.pieceName, piece.pieceType, piece.tilePos);
		}

	}

	public void RemoveAllPieces()
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece chessPiece)
			{
				node2D.CallDeferred("remove_child", chessPiece);
			}
		}
	}
	public void AssignCastlingRights()
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is Rook rook)
			{
				foreach (Vector2I tile in Menu.castleVectors)
				{
					if (tile == FromGlobalPosToTile((Vector2I)rook.Position))
					{
						rook.hasMoved = false;
					}
				}
			}
			if (oNode is WKing king)
			{
				if (king.isWhite == true && new Vector2I(4, 7) != FromGlobalPosToTile((Vector2I)king.Position))
				{
					king.hasMoved = true;
				}
				else if (king.isWhite == false && new Vector2I(4, 0) != FromGlobalPosToTile((Vector2I)king.Position))
				{
					king.hasMoved = true;
				}
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (callcount == 0)
		{
			AssignCastlingRights();
			callcount++;
		}
	}

	public Vector2I FromTileToGlobalPos(Vector2I pos)
	{

		pos.X = (pos.X * this.RenderingQuadrantSize) + 8;
		pos.Y = (pos.Y * this.RenderingQuadrantSize) + 8;

		return pos;

	}
	public Vector2I FromGlobalPosToTile(Vector2I pos)
	{
		if (pos.X < 0)
		{
			pos.X -= 100000;
			pos.X /= this.RenderingQuadrantSize;
		}
		else if (pos.Y < 0)
		{
			pos.Y -= 100000;
			pos.Y /= this.RenderingQuadrantSize;
		}
		else
		{
			pos.X /= this.RenderingQuadrantSize;
			pos.Y /= this.RenderingQuadrantSize;
		}

		return pos;
	}
	public void SpawnPiece(ChessPiece piece, string pieceName, string pieceType, Vector2I tilePosition)
	{


		node2D.CallDeferred("add_child", piece);
		piece.Name = pieceName;
		if (pieceType.Contains("black"))
		{
			piece.isWhite = false;
		}

		piece.Texture = (Texture2D)GD.Load("res://ChessAssets/pieces-basic-png/" + pieceType + ".png");
		piece.Scale = new Godot.Vector2(0.125f, 0.125f);
		piece.Position = FromTileToGlobalPos(tilePosition);
		piece.ZIndex = 2;
		((ChessPiece)piece).prevTile = tilePosition;

	}
}


