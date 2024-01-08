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
	public bool ignoreCheck = false;

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
				if (this.isWhite == true && MoveHandling((Vector2I)tempPos, this.isWhite, isWhitesTurn))
				{
				}
				else if (this.isWhite == true && CaptureHandling((Vector2I)tempPos, this.isWhite, isWhitesTurn))
				{
				}
				else
				{
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
			}
			// Black's turn
			else
			{
				if (this.isWhite == false && MoveHandling((Vector2I)tempPos, this.isWhite, !isWhitesTurn))
				{

				}
				else if (this.isWhite == false && CaptureHandling((Vector2I)tempPos, this.isWhite, !isWhitesTurn))
				{
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

	public bool MoveHandling(Vector2I tempPos, bool isWhitePiece, bool checkWhitesTurn)
	{
		//Could have broken something by first checking for !IsPieceInTheWay rather than IsValidMove. Castling stops working if I check IsValidMove first.
		if (!IsPieceInTheWay(tempPos) && IsValidMove(tempPos) && !IsCollision(tempPos))
		{
			this.Position = tempPos;
			placeInTileCallCount--;
			GD.Print("tempPos: " + tempPos);
			GD.Print("prevTile: " + prevTile);
			if (IsInCheck())
			{
				if (IsCheck(checkWhitesTurn))
				{
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
				else if (!WasKingInCheck(isWhitePiece))
				{
					GD.Print("I am in check1");
					Vector2 kingPosition = GetKingTilePos(!checkWhitesTurn);
					SpawnRedSquare(redSquare, (Vector2I)kingPosition);
					SetIfKingInCheck(!isWhitePiece, true);
					isWhitesTurn = !isWhitesTurn;

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
				SetIfKingInCheck(!isWhitePiece, false);
				isWhitesTurn = !isWhitesTurn;
			}
			return true;
		}
		return false;
	}
	public bool CaptureHandling(Vector2I tempPos, bool isWhitePiece, bool checkWhitesTurn)
	{
		if (IsValidMove(tempPos) && IsCollisionWithOppositeColor(tempPos))
		{

			ChessPiece pieceToCapture = GetPieceWithoutSelf(tempPos);
			if (pieceToCapture != null)
			{
				pieceToCapture.ignoreCheck = true;
			}
			if (IsInCheck())
			{
				if (IsCheck(checkWhitesTurn))
				{
					if (pieceToCapture != null)
					{
						pieceToCapture.ignoreCheck = false;
					}
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
				else if (!WasKingInCheck(checkWhitesTurn))
				{
					GD.Print("I am in check2");
					Vector2 kingPosition = GetKingTilePos(!checkWhitesTurn);
					SpawnRedSquare(redSquare, (Vector2I)kingPosition);
					SetIfKingInCheck(!isWhitePiece, true);
					GD.Print("piece captured");
					CapturePiece(tempPos);
					isWhitesTurn = !isWhitesTurn;

				}
				else
				{
					if (pieceToCapture != null)
					{
						pieceToCapture.ignoreCheck = false;
					}
					this.Position = tileMap.FromTileToGlobalPos(prevTile);
				}
			}
			else
			{
				GD.Print("I should not be in check");
				RemoveRedSquare();
				SetIfKingInCheck(!isWhitePiece, false);
				GD.Print("piece captured");
				CapturePiece(tempPos);
				isWhitesTurn = !isWhitesTurn;
			}
			return true;
		}
		return false;

	}
	public void SetIfKingInCheck(bool isWhite, bool value)
	{
		if (isWhite)
		{
			whiteWasInCheck = value;
		}
		else
		{
			blackWasInCheck = value;
		}
	}
	public bool WasKingInCheck(bool isWhite)
	{
		if (isWhite)
		{
			return whiteWasInCheck;
		}
		return blackWasInCheck;
	}
	public override void _Input(InputEvent @event)
	{

		if (@event is InputEventMouseButton inputEventMouseButton)
		{
			if (inputEventMouseButton.ButtonIndex == MouseButton.Left)
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

		if (listOfTilesToCheck.Count > 0)
		{
			listOfTilesToCheck.RemoveAt(0);
		}


		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;


				Vector2I pieceTile = tileMap.FromGlobalPosToTile((Vector2I)chessPiece.Position);

				foreach (Vector2 element in listOfTilesToCheck)
				{
					if (pieceTile == element && tempTile != pieceTile)
					{
						GD.Print("Piece is in the way");
						return true;
					}
				}


			}
		}
		return false;
	}

	public bool IsCollisionWithOppositeColor(Vector2 tempPos)
	{

		return GetPieceWithoutSelf(tempPos) != null;
	}
	public ChessPiece GetPieceWithoutSelf(Vector2 tempPos)
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
						return chessPiece;

					}
				}
			}
		}
		return null;
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

		return IsCheck(true) || IsCheck(false);
	}
	public bool IsCheck(bool isWhite)
	{
		Vector2I kingPos = tileMap.FromTileToGlobalPos((Vector2I)GetKingTilePos(isWhite));
		return IsCheck(kingPos, isWhite);
	}
	public bool IsCheck(Vector2I kingPos, bool isWhite)
	{
		foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is ChessPiece)
			{
				ChessPiece chessPiece = (ChessPiece)oNode;

				if (chessPiece.isWhite != isWhite && !chessPiece.ignoreCheck)
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
	public void CastleHandler(Vector2 targetRookTile, ChessPiece king, string castleDirection)
	{
        foreach (var oNode in node2D.GetChildren())
		{
			if (oNode is Rook rook)
			{
				if (rook.Position == tileMap.FromTileToGlobalPos((Vector2I)targetRookTile) && rook.hasMoved == false)
				{
					Vector2 rookTilePos = tileMap.FromGlobalPosToTile((Vector2I)rook.Position);
					Vector2 kingTilePos = tileMap.FromGlobalPosToTile((Vector2I)king.Position);
					
						if (castleDirection == "shortCastle")
						{

							rookTilePos.X = kingTilePos.X - 1;
							rook.Position = tileMap.FromTileToGlobalPos((Vector2I)rookTilePos);
							rook.hasMoved = true;

						}
						if (castleDirection == "longCastle")
						{

							rookTilePos.X = kingTilePos.X + 1;
							rook.Position = tileMap.FromTileToGlobalPos((Vector2I)rookTilePos);
							rook.hasMoved = true;
						}
					
				}
			}
		}
	}
}

