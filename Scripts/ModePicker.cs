using Godot;

public partial class ModePicker : Control
{
	private static PackedScene Board => GD.Load<PackedScene>("res://Scenes/Board.tscn");

	public void OnSinglePlayerPressed()
	{
		Game.CurrentGameMode = Game.GameMode.SinglePlayer;
		GetTree().ChangeSceneToPacked(Board);
	}

	public void OnMultiplayerPressed()
	{
		Game.CurrentGameMode = Game.GameMode.MultiPlayer;
		GetTree().ChangeSceneToPacked(Board);
	}
}
