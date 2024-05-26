using Godot;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/ModePicker.tscn");
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
