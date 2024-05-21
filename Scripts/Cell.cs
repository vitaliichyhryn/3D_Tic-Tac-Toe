using Godot;
using System;

public partial class Cell : Node3D
{
	public Board Board => GetNode("..") as Board;
	public Board.Player State { get; set; } = Board.Player.NoPlayer;
	public MeshInstance3D Mesh => GetNode<MeshInstance3D>("Mesh");

	public StandardMaterial3D redPlayerMaterial = new StandardMaterial3D();
	public StandardMaterial3D bluePlayerMaterial = new StandardMaterial3D();

	[Signal]
	public delegate void FilledEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		redPlayerMaterial.AlbedoColor = new Color(255, 0, 0, 0.5f);
		bluePlayerMaterial.AlbedoColor = new Color(0, 0, 255, 0.5f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitCell(int i, int j, int k)
	{
		Position = new Vector3(2f * i, 2f * j, 2f *k);
	}

	public void OnStaticBody3DInputEvent(Camera3D camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeID)
	{
		if (Input.IsActionJustPressed("UILeftClick"))
		{
			Fill();
		}
	}

	public void Fill()
	{
		if (State != Board.Player.NoPlayer) return;
		
		State = Board.CurrentPlayer;
		
		if (Board.CurrentPlayer == Board.Player.FirstPlayer)
		{
			Mesh.MaterialOverride = redPlayerMaterial;
		}
		else
		{
			Mesh.MaterialOverride = bluePlayerMaterial;
		}
		
		EmitSignal(SignalName.Filled);
	}
}