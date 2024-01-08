using Godot;
using System;

public partial class Menu : Control
{
	private void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://ChessScenes/node_2d.tscn");
	}
	private void _on_quit_pressed(){
		GetTree().Quit();
	}
	
}
