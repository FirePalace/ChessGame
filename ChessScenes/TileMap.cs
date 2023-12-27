using Godot;
using System;


public partial class TileMap : Godot.TileMap
{
	Node2D node2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		
		node2D = GetNode<Node2D>("..");
		Sprite2D wRook1 = node2D.GetNode<Sprite2D>("WRook");
		Sprite2D wRook2 = new Rook();
		Sprite2D bRook1 = new Rook();
		Sprite2D bRook2 = new Rook();

		Sprite2D wBishop1 = node2D.GetNode<Sprite2D>("WBishop");
		Sprite2D wBishop2 = new WBishop();
		Sprite2D bBishop1 = new WBishop();
		Sprite2D bBishop2 = new WBishop();

		Sprite2D wKnight1 = node2D.GetNode<Sprite2D>("WKnight");
		Sprite2D wKnight2 = new WKnight();
		Sprite2D bKnight1 = new WKnight();
		Sprite2D bKnight2 = new WKnight();

		Sprite2D wQueen = node2D.GetNode<Sprite2D>("WQueen");
		Sprite2D bQueen = new WQueen();

		Sprite2D wKing = node2D.GetNode<Sprite2D>("WKing");
		Sprite2D bKing = new WKing();
		
		for (int i = 0; i < 8; i++)
		{
			ChessPiece wPawn = new WPawn();
			ChessPiece bPawn = new WPawn();
			bPawn.isWhite = false;
			SpawnPiece(wPawn,"wPawn", "white-pawn", new Vector2I(0+i,6));
			SpawnPiece(bPawn,"bPawn", "black-pawn", new Vector2I(0+i,1));

		}
		
		
		wRook1.Position = FromTileToGlobalPos(new Vector2I(0,7));
		SpawnPiece(wRook2,"wRook2", "white-rook", new Vector2I(7,7));
		SpawnPiece(bRook1,"bRook1", "black-rook", new Vector2I(0,0));
		SpawnPiece(bRook2,"bRook2", "black-rook", new Vector2I(7,0));

		wBishop1.Position = FromTileToGlobalPos(new Vector2I(2,7));
		SpawnPiece(wBishop2, "wBishop2", "white-bishop", new Vector2I(5,7));
		SpawnPiece(bBishop1, "wBishop1", "black-bishop", new Vector2I(2,0));
		SpawnPiece(bBishop2, "bBishop2", "black-bishop", new Vector2I(5,0));

		wKnight1.Position = FromTileToGlobalPos(new Vector2I(1,7));
		SpawnPiece(wKnight2, "wKnight2", "white-knight", new Vector2I(6,7));
		SpawnPiece(bKnight1, "bKnight1", "black-knight", new Vector2I(1,0));
		SpawnPiece(bKnight2, "bKnight2", "black-knight", new Vector2I(6,0));

		wQueen.Position = FromTileToGlobalPos(new Vector2I(3,7));
		SpawnPiece(bQueen, "bQueen", "black-queen", new Vector2I(3,0));

		wKing.Position = FromTileToGlobalPos(new Vector2I(4,7));
		SpawnPiece(bKing, "bKing", "black-king", new Vector2I(4,0));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public Vector2I FromTileToGlobalPos(Vector2I pos)
	{
		 
		pos.X =  (pos.X * this.RenderingQuadrantSize) + 8;
		pos.Y =  (pos.Y * this.RenderingQuadrantSize) + 8;

		return pos;
		
	}
	public Vector2I FromGlobalPosToTile(Vector2I pos)
	{
		pos.X /= this.RenderingQuadrantSize;
		pos.Y /= this.RenderingQuadrantSize;

		
		return pos;
	}
	public void SpawnPiece(Sprite2D piece, string pieceName, string pieceType, Vector2I tilePosition){
		
		
		node2D.CallDeferred("add_child", piece);
		piece.Name = pieceName;

		piece.Texture = (Texture2D)GD.Load("res://ChessAssets/pieces-basic-png/"+ pieceType +".png");
        piece.Scale = new Godot.Vector2(0.125f, 0.125f);
		piece.Position = FromTileToGlobalPos(tilePosition);

	}


}

 	
