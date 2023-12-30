using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

public partial class ChessPiece : Sprite2D
{
	public TileMap tileMap;
	dragState IsDragging = dragState.unknown;
	int placeInTileCallCount;
	public Vector2I prevTile;
	public bool isWhite = true;
	public static bool isWhitesTurn = true;
	Node2D node2D;

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

		

		if (IsDragging == dragState.isDragging)
		{
				
			this.Position = MousePos;
		}
		
			// OutofBounds
		if (!inBounds.HasPoint(this.Position) && IsDragging == dragState.isNotDragging && viewport.HasPoint(this.Position))
		{
			//this.Position = tileMap.FromTileToGlobalPos(new Vector2I (3,3));
			this.Position = tileMap.FromTileToGlobalPos(prevTile);
		}
		
			// Place inside a Tile
		if(inBounds.HasPoint(this.Position) && IsDragging == dragState.isNotDragging && placeInTileCallCount == 1)
		{
			Vector2 tempPos = this.Position;
			tempPos = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
			tempPos = tileMap.FromTileToGlobalPos((Vector2I)tempPos);
			
			if(isWhitesTurn)
			{
				if(IsValidMove(tempPos) && !IsCollision(tempPos) && !IsPieceInTheWay(tempPos) && this.isWhite == true)
				{
					this.Position = tempPos;
					placeInTileCallCount--;
					GD.Print("tempPos: " + tempPos);
					GD.Print("prevTile: " + prevTile);
					isWhitesTurn = false;
		
				}
				else if (IsValidMove(tempPos) && IsCollisionWithOppositeColor(tempPos) && this.isWhite == true)
				{
					GD.Print("white piece captured");
					CapturePiece(tempPos);
					isWhitesTurn = false;
				}
				else
				{

				this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
						
					
				
			}
			// Black's turn
			else
			{
				if(IsValidMove(tempPos) && !IsCollision(tempPos) &&  !IsPieceInTheWay(tempPos) && this.isWhite == false)
				{
					this.Position = tempPos;
					placeInTileCallCount--;
					GD.Print("tempPos: " + tempPos);
					GD.Print("prevTile: " + prevTile);
					isWhitesTurn = true;
		
				}
				else if (IsValidMove(tempPos) && IsCollisionWithOppositeColor(tempPos) && this.isWhite == false)
				{
					GD.Print("black piece captured");
					CapturePiece(tempPos);
					isWhitesTurn = true;
					
				}
				else
				{
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}

			}
			
		}
		if(IsDragging == dragState.isNotDragging)
		{
			IsDragging = dragState.unknown;
		}
	}
    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton inputEventMouseButton )
		{	
			
			if(tileMap.FromGlobalPosToTile((Vector2I)this.Position) == tileMap.FromGlobalPosToTile((Vector2I)GetGlobalMousePosition()))
			{
				if(IsDragging == dragState.isDragging && !inputEventMouseButton.IsPressed())
				{
				IsDragging = dragState.isNotDragging;
				}
				else if(inputEventMouseButton.IsPressed())
				{
				IsDragging = dragState.isDragging;
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

	public virtual bool IsValidMove(Vector2 tempPos)
	{
		return false;
	}

	public bool IsCollision(Vector2 tempPos)
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
					if(tempTile.X == pieceTile.X && tempTile.Y == pieceTile.Y)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public enum dragState
	{
		unknown ,isDragging, isNotDragging
	}
	
	public virtual bool IsPieceInTheWay(Vector2 tempPos)
	{
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		List<Vector2I> listOfTilesToCheck = new List<Vector2I>();
		listOfTilesToCheck.Add(tempTile);
		Vector2I diffVector = tempTile - prevTile;
		Vector2I destinationTile = prevTile;
		while(destinationTile != tempTile)
		{
			listOfTilesToCheck.Add(destinationTile);
			if(diffVector.X > 0)
			{
				destinationTile.X++;
			}
			if(diffVector.X < 0)
			{
				destinationTile.X--;
			}
			if(diffVector.Y > 0)
			{
				destinationTile.Y++;
			}
			if(diffVector.Y < 0)
			{
				destinationTile.Y--;
			}
			
		}

		foreach (var oNode in node2D.GetChildren())
		{
			if(oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if(chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);

					foreach (Vector2 element in listOfTilesToCheck )
					{
						if(pieceTile == element)
						{
							GD.Print("Piece is in the way at: " + element);
							return true;
							
						}
					}

				}
			}
		}
		return false;
	}
	public bool IsCollisionWithOppositeColor(Vector2 tempPos)
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

					if(tempTile.X == pieceTile.X && tempTile.Y == pieceTile.Y && this.isWhite != chessPiece.isWhite)
					{
						GD.Print("Opposite color");
						return true;
						
					}
				}
			}
		}
		return false;
	}
	public void CapturePiece(Vector2 tempPos){
		foreach (var oNode in node2D.GetChildren())
		{
			if(oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if(chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
					Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
						if(tempTile == pieceTile)
						{
							node2D.CallDeferred("remove_child", chessPiece);
							this.Position = tileMap.FromTileToGlobalPos(tempTile);
							
						}	
				}
			}
		}
	}
}