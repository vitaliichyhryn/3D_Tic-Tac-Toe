using Godot;
using System;

public partial class Cell : Node3D
{
	public Board Board => GetNode("..") as Board;
	public Board.Player State { get; set; } = Board.Player.NoPlayer;
	public MeshInstance3D Mesh => GetNode<MeshInstance3D>("Mesh");
	public int i, j, k;

	public StandardMaterial3D redPlayerMaterial = GD.Load<StandardMaterial3D>("res://Materials/RedPlayer.tres");
	public StandardMaterial3D bluePlayerMaterial = GD.Load<StandardMaterial3D>("res://Materials/BluePlayer.tres");

	[Signal]
	public delegate void FilledEventHandler(Cell cell);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Create(int i, int j, int k)
	{
		Position = new Vector3(2f * (i - 1), 2f * (j - 1), 2f * (k - 1));
		this.i = i;
		this.j = j;
		this.k = k;
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
		
		if (Board.CurrentPlayer == Board.Player.RedPlayer)
		{
			Mesh.MaterialOverride = redPlayerMaterial;
		}
		else
		{
			Mesh.MaterialOverride = bluePlayerMaterial;
		}
		
		EmitSignal(SignalName.Filled, this);
	}
}