using Godot;

public partial class WinMessage : Control
{
	private Label Winner => GetNode<Label>("WinMessageContainer/Winner");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Winner.Text = Board.GetPlayerName(Game.Winner) + " HAS WON!";
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
