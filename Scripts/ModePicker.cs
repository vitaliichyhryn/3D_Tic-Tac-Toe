using Godot;

public partial class ModePicker : Control
{
	PackedScene BoardScene = GD.Load<PackedScene>("res://Scenes/Board.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSingleplayerPressed()
	{
		Game.CurrentGameMode = Game.GameMode.Singleplayer;
		GetTree().ChangeSceneToPacked(BoardScene);
	}

	public void OnMultiplayerPressed()
	{
		Game.CurrentGameMode = Game.GameMode.Multiplayer;
		GetTree().ChangeSceneToPacked(BoardScene);
	}
}
