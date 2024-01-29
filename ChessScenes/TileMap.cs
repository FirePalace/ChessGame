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
		if (Menu.fenPieces == null)
		{
			StandardReady();
		}
		else
		{
			FENReady();
		}
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
	public void StandardReady()
	{


		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece chessPiece)
			{
				if (chessPiece.Name == "wRook1")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(0, 7));
					chessPiece.prevTile = new Vector2I(0, 7);
					chessPiece.ZIndex = 2;

				}
				if (chessPiece.Name == "wPawn")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(0, 6));
					chessPiece.prevTile = new Vector2I(0, 6);
					chessPiece.ZIndex = 2;
				}

				if (chessPiece.Name == "wBishop1")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(2, 7));
					chessPiece.prevTile = new Vector2I(2, 7);
					chessPiece.ZIndex = 2;
				}
				if (chessPiece.Name == "wKnight1")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(1, 7));
					chessPiece.prevTile = new Vector2I(1, 7);
					chessPiece.ZIndex = 2;
				}
				if (chessPiece.Name == "wQueen")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(3, 7));
					chessPiece.prevTile = new Vector2I(3, 7);
					chessPiece.ZIndex = 2;
				}
				if (chessPiece.Name == "wKing")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(4, 7));
					chessPiece.prevTile = new Vector2I(4, 7);
					chessPiece.ZIndex = 2;
				}
			}
		}


		ChessPiece wRook2 = new Rook();
		ChessPiece bRook1 = new Rook();
		bRook1.isWhite = false;
		ChessPiece bRook2 = new Rook();
		bRook2.isWhite = false;

		ChessPiece wBishop1 = new WBishop();
		ChessPiece wBishop2 = new WBishop();
		ChessPiece bBishop1 = new WBishop();
		bBishop1.isWhite = false;
		ChessPiece bBishop2 = new WBishop();
		bBishop2.isWhite = false;

		ChessPiece wKnight1 = new WKnight();
		ChessPiece wKnight2 = new WKnight();
		ChessPiece bKnight1 = new WKnight();
		bKnight1.isWhite = false;
		ChessPiece bKnight2 = new WKnight();
		bKnight2.isWhite = false;

		ChessPiece wQueen = new WQueen();
		ChessPiece bQueen = new WQueen();
		bQueen.isWhite = false;

		ChessPiece wKing = new WKing();
		ChessPiece bKing = new WKing();
		bKing.isWhite = false;

		for (int i = 0; i < 8; i++)
		{

			ChessPiece wPawn = new WPawn();
			ChessPiece bPawn = new WPawn();
			bPawn.isWhite = false;

			if (i != 0)
			{
				SpawnPiece(wPawn, "wPawn", "white-pawn", new Vector2I(0 + i, 6));
			}
			SpawnPiece(bPawn, "bPawn", "black-pawn", new Vector2I(0 + i, 1));

		}

		SpawnPiece(wRook2, "wRook2", "white-rook", new Vector2I(7, 7));
		SpawnPiece(bRook1, "bRook1", "black-rook", new Vector2I(0, 0));
		SpawnPiece(bRook2, "bRook2", "black-rook", new Vector2I(7, 0));

		SpawnPiece(wBishop2, "wBishop2", "white-bishop", new Vector2I(5, 7));
		SpawnPiece(bBishop1, "wBishop1", "black-bishop", new Vector2I(2, 0));
		SpawnPiece(bBishop2, "bBishop2", "black-bishop", new Vector2I(5, 0));

		SpawnPiece(wKnight2, "wKnight2", "white-knight", new Vector2I(6, 7));
		SpawnPiece(bKnight1, "bKnight1", "black-knight", new Vector2I(1, 0));
		SpawnPiece(bKnight2, "bKnight2", "black-knight", new Vector2I(6, 0));

		SpawnPiece(bQueen, "bQueen", "black-queen", new Vector2I(3, 0));

		SpawnPiece(bKing, "bKing", "black-king", new Vector2I(4, 0));
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


