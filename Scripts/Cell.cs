using Godot;
using System;

public partial class Cell : Node3D
{
	MeshInstance3D MeshInstance => GetChild<MeshInstance3D>(0);
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStaticBody3DInputEvent(Camera3D camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeID)
	{
		if (Input.IsActionJustPressed("UILeftClick"))
		{
			GD.Print("yippee" + shapeID);
			StandardMaterial3D material = MeshInstance.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
			material.AlbedoColor = new Color(255, 0, 0, 1);
		}
	}
}