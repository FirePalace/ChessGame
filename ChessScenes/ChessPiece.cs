using Godot;
using System;

public partial class ChessPiece : Sprite2D
{
	TileMap tileMap;
	bool IsDragging = false;
	int placeInTileCallCount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		Node2D node2D = GetNode<Node2D>("..");
		tileMap = node2D.GetNode<TileMap>("Tilemap");



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Vector2 MousePos = GetGlobalMousePosition();
		
		Rect2 inBounds = new Rect2I(0,0, 128, 128);

		if (IsDragging)
		{
			this.Position = MousePos;
		}
		
			// OutofBounds
		if (!inBounds.HasPoint(this.Position) && !IsDragging)
		{
			this.Position = new Vector2(0,0);
		}
		
			// Place inside a Tile
		if(inBounds.HasPoint(this.Position) && !IsDragging && placeInTileCallCount == 1)
		{
			Vector2 tempPos = this.Position;
			tempPos = tileMap.FromGlobalPosToTile((Vector2I)tempPos);
			tempPos = tileMap.FromTileToGlobalPos((Vector2I)tempPos);
			this.Position = tempPos;

			placeInTileCallCount--;
			GD.Print("tempPos: " + tempPos);
		}
	}

    public override void _Input(InputEvent @event)
    {	
		
        if (@event is InputEventMouseButton eventMouseButton)
		{	
			if(tileMap.FromGlobalPosToTile((Vector2I)this.Position) == tileMap.FromGlobalPosToTile((Vector2I)GetGlobalMousePosition()))
			{
				if(IsDragging)
				{
				IsDragging = false;
				}
				else
				{
				IsDragging = true;
				placeInTileCallCount = 1;
				}
			}
		}
    }
	
}