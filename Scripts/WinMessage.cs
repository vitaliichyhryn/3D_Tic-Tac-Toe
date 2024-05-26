using Godot;
using System;

public partial class WinMessage : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMenuPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
	}

	public void OnRestartPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Board.tscn");
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
