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
	public Vector2I prevTile = new Vector2I(1, 1);
	public bool isWhite = true;
	public static bool isWhitesTurn = true;
	public static En_Passant enPassant = new En_Passant();
	public Node2D node2D;
	Sprite2D redSquare = new Sprite2D();
	public static bool whiteWasInCheck = false;
	public static bool blackWasInCheck = false;

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

		Rect2 inBounds = new Rect2I(0, 0, 128, 128);
		Rect2 viewport = new Rect2I(-70, -10, 200, 140);



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
		if (inBounds.HasPoint(this.Position) && IsDragging == dragState.isNotDragging && placeInTileCallCount == 1)
		{
			Vector2 tempPos = this.Position;
			tempPos = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
			tempPos = tileMap.FromTileToGlobalPos((Vector2I)tempPos);

			//Whites turn
			if (isWhitesTurn)
			{
				if (this.isWhite == true && IsValidMove(tempPos) && !IsCollision(tempPos) && !IsPieceInTheWay(tempPos))
				{
					this.Position = tempPos;
					placeInTileCallCount--;
					GD.Print("tempPos: " + tempPos);
					GD.Print("prevTile: " + prevTile);
					if (IsInCheck())
					{
						if (IsCheck(isWhitesTurn))
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
						else if (!whiteWasInCheck)
						{
							GD.Print("I am in check1");
							Vector2 kingPosition = GetKingTilePos(!isWhitesTurn);
							SpawnRedSquare(redSquare, (Vector2I)kingPosition);
							blackWasInCheck = true;
							isWhitesTurn = false;

						}
						else
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
					}
					else
					{
						GD.Print("I should not be in check");
						RemoveRedSquare();
						blackWasInCheck = false;
						isWhitesTurn = false;
					}



				}
				else if (this.isWhite == true && IsValidMove(tempPos) && IsCollisionWithOppositeColor(tempPos))
				{
					GD.Print("black piece captured");
					CapturePiece(tempPos);
					if (IsInCheck())
					{
						if (IsCheck(isWhitesTurn))
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
						else if (!whiteWasInCheck)
						{
							GD.Print("I am in check2");
							Vector2 kingPosition = GetKingTilePos(!isWhitesTurn);
							SpawnRedSquare(redSquare, (Vector2I)kingPosition);
							blackWasInCheck = true;
							isWhitesTurn = false;

						}
						else if (whiteWasInCheck)
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
					}
					else
					{
						GD.Print("I should not be in check");
						RemoveRedSquare();
						blackWasInCheck = false;
						isWhitesTurn = false;
					}
				}
				else
				{

					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
			}
			// Black's turn
			else
			{

				if (this.isWhite == false && IsValidMove(tempPos) && !IsCollision(tempPos) && !IsPieceInTheWay(tempPos))
				{
					this.Position = tempPos;
					placeInTileCallCount--;
					GD.Print("tempPos: " + tempPos);
					GD.Print("prevTile: " + prevTile);
					if (IsInCheck())
					{
						if (IsCheck(!isWhitesTurn))
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
						else if (!blackWasInCheck)
						{
							GD.Print("I am in check3");
							Vector2 kingPosition = GetKingTilePos(!isWhitesTurn);
							SpawnRedSquare(redSquare, (Vector2I)kingPosition);
							whiteWasInCheck = true;
							isWhitesTurn = true;
						}
						else
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
					}
					else
					{
						GD.Print("I should not be in check");
						RemoveRedSquare();
						whiteWasInCheck = false;
						isWhitesTurn = true;
					}

				}
				else if (this.isWhite == false && IsValidMove(tempPos) && IsCollisionWithOppositeColor(tempPos))
				{
					GD.Print("white piece captured");
					CapturePiece(tempPos);

					if (IsInCheck())
					{
						if (IsCheck(!isWhitesTurn))
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
						else if (!blackWasInCheck)
						{
							GD.Print("I am in check3");
							Vector2 kingPosition = GetKingTilePos(!isWhitesTurn);
							SpawnRedSquare(redSquare, (Vector2I)kingPosition);
							whiteWasInCheck = true;
							isWhitesTurn = true;
						}
						else
						{
							this.Position = tileMap.FromTileToGlobalPos(prevTile);
						}
					}

					else
					{
						GD.Print("I should not be in check");
						RemoveRedSquare();
						whiteWasInCheck = false;
						isWhitesTurn = true;
					}

				}
				else
				{
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}

			}
			if (enPassant.IsValid && enPassant.isWhite == isWhitesTurn)
			{
				enPassant.IsValid = false;
			}

		}
		if (IsDragging == dragState.isNotDragging)
		{
			IsDragging = dragState.unknown;
		}


	}
	public override void _Input(InputEvent @event)
	{

		if (@event is InputEventMouseButton inputEventMouseButton)
		{

			if (tileMap.FromGlobalPosToTile((Vector2I)this.Position) == tileMap.FromGlobalPosToTile((Vector2I)GetGlobalMousePosition()))
			{
				if (IsDragging == dragState.isDragging && !inputEventMouseButton.IsPressed())
				{
					IsDragging = dragState.isNotDragging;
				}
				else if (inputEventMouseButton.IsPressed())
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
	public virtual bool IsValidMove(Vector2 tempPos, Vector2I tileStart)
	{
		return false;
	}

	public bool IsCollision(Vector2 tempPos)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
					Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
					if (tempTile.X == pieceTile.X && tempTile.Y == pieceTile.Y)
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
		unknown, isDragging, isNotDragging
	}

	public virtual bool IsPieceInTheWay(Vector2 tempPos)
	{
		return IsPieceInTheWay(tempPos, prevTile);
	}
	public virtual bool IsPieceInTheWay(Vector2 tempPos, Vector2I tileStart)
	{
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		List<Vector2I> listOfTilesToCheck = new List<Vector2I>();
		//listOfTilesToCheck.Add(tempTile);
		Vector2I diffVector = tempTile - tileStart;
		Vector2I destinationTile = tileStart;
		while (destinationTile != tempTile)
		{
			listOfTilesToCheck.Add(destinationTile);
			if (diffVector.X > 0)
			{
				destinationTile.X++;
			}
			if (diffVector.X < 0)
			{
				destinationTile.X--;
			}
			if (diffVector.Y > 0)
			{
				destinationTile.Y++;
			}
			if (diffVector.Y < 0)
			{
				destinationTile.Y--;
			}
			if (listOfTilesToCheck.Count > 9)
			{

				break;
			}
		}
		//?
		listOfTilesToCheck.RemoveAt(0);

		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);

					foreach (Vector2 element in listOfTilesToCheck)
					{
						if (pieceTile == element && tempTile != pieceTile)
						{
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
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
					Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);

					if (tempTile.X == pieceTile.X && tempTile.Y == pieceTile.Y && this.isWhite != chessPiece.isWhite)
					{
						GD.Print("Opposite color");
						return true;

					}
				}
			}
		}
		return false;
	}
	public void CapturePiece(Vector2 tempPos)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece != this)
				{
					Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
					Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);

					if (tempTile == pieceTile)
					{
						node2D.CallDeferred("remove_child", chessPiece);
						this.Position = tileMap.FromTileToGlobalPos(tempTile);

					}
				}
			}
		}
	}
	public bool IsInCheck()
	{
		Vector2I kingPos = new Vector2I();
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;
				if (chessPiece.Name == "WKing")
				{
					kingPos = (Vector2I)chessPiece.Position;
					if (IsCheck(kingPos, false))
					{
						return true;
					}
				}
				if (chessPiece.Name == "bKing")
				{
					kingPos = (Vector2I)chessPiece.Position;
					if (IsCheck(kingPos, true))
					{
						return true;
					}
				}
			}
		}
		return false;
	}
	public bool IsCheck(Vector2I kingPos, bool isWhite)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;

				if (chessPiece.isWhite == isWhite)
				{
					Vector2I currentTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);

					if (chessPiece.IsValidMove(kingPos, currentTile) && !IsPieceInTheWay(kingPos, currentTile))
					{
						GD.Print("test");
						return true;
					}
				}
			}
		}
		return false;
	}
	public bool IsCheck(bool isWhite)
	{
		Vector2I kingPos = tileMap.FromTileToGlobalPos((Vector2I)GetKingTilePos(isWhite));
		return IsCheck(kingPos, isWhite);
	}
	public Vector2 GetKingTilePos(bool isWhite)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;

				if (isWhite && chessPiece.Name == "WKing")
				{
					return tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
				}
				if (!isWhite && chessPiece.Name == "bKing")
				{
					return tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);
				}
			}
		}
		throw new ArgumentException("King's position cannot be null");
	}
	public void SpawnRedSquare(Sprite2D piece, Vector2I tilePosition)
	{
		node2D.CallDeferred("add_child", piece);

		piece.Name = "RedSquare";
		piece.Texture = (Texture2D)GD.Load("res://ChessScenes/Solid_red.svg.png");
		piece.Scale = new Vector2(0.033f, 0.0335f);
		piece.Position = tileMap.FromTileToGlobalPos(tilePosition);
		piece.Position += new Vector2(0, 0.033f);
		piece.ZIndex = 1;
	}
	public void RemoveRedSquare()
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is Sprite2D)
			{
				Sprite2D sprite = (Sprite2D)oNode;
				if (sprite.Name == "RedSquare")
				{
					node2D.CallDeferred("remove_child", sprite);
				}
			}
		}
	}

}
