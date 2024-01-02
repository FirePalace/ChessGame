using Godot;
using System;


public partial class TileMap : Godot.TileMap
{
	Node2D node2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		node2D = GetNode<Node2D>("..");

		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece.Name == "wRook1")
				{
					chessPiece.Position =FromTileToGlobalPos(new Vector2I(0, 7));
					chessPiece.prevTile = new Vector2I(0, 7);

				}
				if (chessPiece.Name == "wPawn")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(0, 6));
					chessPiece.prevTile = new Vector2I(0, 6);
				}

				if (chessPiece.Name == "wBishop1")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(2, 7));
					chessPiece.prevTile = new Vector2I(2, 7);
				}
				if (chessPiece.Name == "wKnight1")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(1, 7));
					chessPiece.prevTile = new Vector2I(1, 7);
				}
				if(chessPiece.Name == "WQueen")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(3, 7));
					chessPiece.prevTile = new Vector2I(3, 7);
				}
				if(chessPiece.Name == "WKing")
				{
					chessPiece.Position = FromTileToGlobalPos(new Vector2I(4, 7));
					chessPiece.prevTile = new Vector2I(4, 7);
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

	}

	public Vector2I FromTileToGlobalPos(Vector2I pos)
	{

		pos.X = (pos.X * this.RenderingQuadrantSize) + 8;
		pos.Y = (pos.Y * this.RenderingQuadrantSize) + 8;

		return pos;

	}
	public Vector2I FromGlobalPosToTile(Vector2I pos)
	{
		pos.X /= this.RenderingQuadrantSize;
		pos.Y /= this.RenderingQuadrantSize;


		return pos;
	}
	public void SpawnPiece(Sprite2D piece, string pieceName, string pieceType, Vector2I tilePosition)
	{


		node2D.CallDeferred("add_child", piece);
		piece.Name = pieceName;

		piece.Texture = (Texture2D)GD.Load("res://ChessAssets/pieces-basic-png/" + pieceType + ".png");
		piece.Scale = new Godot.Vector2(0.125f, 0.125f);
		piece.Position = FromTileToGlobalPos(tilePosition);
		((ChessPiece)piece).prevTile = tilePosition;

	}
}


