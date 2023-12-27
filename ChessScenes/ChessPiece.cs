using Godot;
using System;
using System.Collections.Generic;

public partial class ChessPiece : Sprite2D
{
	public TileMap tileMap;
	bool IsDragging = false;
	int placeInTileCallCount;
	public Vector2I prevTile;
	public bool isWhite = true;
	Node2D node2D;

	

	//? Bitboard
	/* long wPawnsPos;
	long wKnightsPos;
	long wBishopsPos;
	long wRooksPos;
	long wQueenPos;
	long wKing;
	long bPawnPos;
	long bKnightsPos;
	long bBishopsPos;
	long bRooksPos;
	long bQueenPos;
	long bKing; */

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		node2D = GetNode<Node2D>("..");
		tileMap = node2D.GetNode<TileMap>("Tilemap");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Vector2 MousePos = GetGlobalMousePosition();
		
		Rect2 inBounds = new Rect2I(0,0, 128, 128);
		Rect2 viewport = new Rect2I (-70,-10,200,140);

		

		if (IsDragging)
		{
				
			this.Position = MousePos;
		}
		
			// OutofBounds
		if (!inBounds.HasPoint(this.Position) && !IsDragging)
		{
			//this.Position = tileMap.FromTileToGlobalPos(new Vector2I (3,3));
			this.Position = tileMap.FromTileToGlobalPos(prevTile);
		}
		
			// Place inside a Tile
		if(inBounds.HasPoint(this.Position) && !IsDragging && placeInTileCallCount == 1)
		{
			Vector2 tempPos = this.Position;
			tempPos = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
			tempPos = tileMap.FromTileToGlobalPos((Vector2I)tempPos);
			
			if(isValidMove(tempPos) && !isCollision(tempPos))
			{
				this.Position = tempPos;
				placeInTileCallCount--;
				GD.Print("tempPos: " + tempPos);
				GD.Print("prevTile: " + prevTile);
			}
			else
			{
				this.Position = tileMap.FromTileToGlobalPos(prevTile);
			}
		}
	}
    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton inputEventMouseButton )
		{	
			
			if(tileMap.FromGlobalPosToTile((Vector2I)this.Position) == tileMap.FromGlobalPosToTile((Vector2I)GetGlobalMousePosition()))
			{
				if(IsDragging && !inputEventMouseButton.IsPressed())
				{
				IsDragging = false;
				}
				else if(inputEventMouseButton.IsPressed())
				{
				IsDragging = true;
				placeInTileCallCount = 1;
				PrevTile();
				}
			}
		}
    }
	public Vector2I PrevTile()
	{
	prevTile = tileMap.FromGlobalPosToTile((Vector2I)this.Position);
	return prevTile;
	}

	public virtual bool isValidMove(Vector2 tempPos)
	{
		return false;
	}

	public bool isCollision(Vector2 tempPos)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if(oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if(chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
					Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
					if(tempTile.X == pieceTile.X && tempTile.Y== pieceTile.Y)
					{
						return true;
					}
				}

			}
			
		}
		return false;
	}
	
}