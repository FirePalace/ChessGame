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
		Vector2I tempTile = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
		int xOffset = tempTile.X - prevTile.X;
		int yOffset = tempTile.Y - prevTile.Y;
		
		
		
		if(tempTile.X == prevTile.X)
		{	
			//Normal pawn move
			if(isWhite && (tempTile.Y - prevTile.Y) == -1)
			{
				hasMoved = true;
				return true;
			}
			if(!isWhite && (tempTile.Y - prevTile.Y) == 1)
			{
				hasMoved = true;
				return true;
			}
			//double move forward
			if(isWhite && (tempTile.Y - prevTile.Y) == -2 && hasMoved == false)
			{
				hasMoved = true;
				return true;
			}
			if(!isWhite && (tempTile.Y - prevTile.Y) == 2 && hasMoved == false)
			{
				hasMoved = true;
				return true;
			}	
			
		}
		
		//Capturing diagonally
		if(isWhite && xOffset == yOffset && yOffset == -1  || isWhite && Math.Abs(xOffset) - Math.Abs(yOffset) == 0 && yOffset == -1 )
		{

			if(IsCollisionWithOppositeColor(tempPos)){
				hasMoved = true;
				return true;
			}
			
		}
		if(!isWhite && xOffset == yOffset && yOffset == 1  || !isWhite && Math.Abs(xOffset) - Math.Abs(yOffset) == 0 && yOffset == 1 )
		{

			if(IsCollisionWithOppositeColor(tempPos)){
				hasMoved = true;
				return true;
			}
			
		}

        return false;
    }



}
