using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Node3D
{
	public const int cellNumber = 27;

	public enum Player
	{
		NoPlayer,
		FirstPlayer,
		SecondPlayer
	}

	public Player CurrentPlayer { get; set; } = Player.FirstPlayer;

	public List<Cell> cells = new List<Cell>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitBoard();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitBoard()
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					var cell = GD.Load<PackedScene>("res://Scenes/Cell.tscn").Instantiate() as Cell;
					cell.InitCell(i, j, k);
					AddChild(cell);
					cells.Add(cell);
					cell.Filled += NextTurn;
				}
			}
		}
	}

	public void NextTurn()
	{
		if (CurrentPlayer == Player.FirstPlayer) CurrentPlayer = Player.SecondPlayer;
		else CurrentPlayer = Player.FirstPlayer;
	}

	public void CheckRows()
	{

	}

	public void CheckDiagonals()
	{

	}
}
