using Godot;

public partial class Menu : Control
{
	private static PackedScene ModePicker => GD.Load<PackedScene>("res://Scenes/ModePicker.tscn");

	public void OnPlayPressed()
	{
		GetTree().ChangeSceneToPacked(ModePicker);
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
