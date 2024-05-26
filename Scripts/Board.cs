using Godot;
using System;
using System.Linq;

public partial class Board : Node3D
{
	public enum Player
	{
		NoPlayer,
		RedPlayer,
		BluePlayer
	}

	public Player CurrentPlayer { get; set; } = Player.RedPlayer;

	public Cell[,,] Cells = new Cell[3, 3, 3];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Create();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Create()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					var cell = GD.Load<PackedScene>("res://Scenes/Cell.tscn").Instantiate() as Cell;
					cell.Create(i, j, k);
					AddChild(cell);
					Cells[i, j, k] = cell;
					cell.Filled += NextTurn;
				}
			}
		}
	}

	public bool IsWin(Cell cell)
	{
		// Check X row
		if (Enumerable.Range(0, 3).All(i => Cells[i, cell.j, cell.k].State == cell.State)) return true;

		// Check Y column
		if (Enumerable.Range(0, 3).All(i => Cells[cell.i, i, cell.k].State == cell.State)) return true;

		// Check Z tube
		if (Enumerable.Range(0, 3).All(i => Cells[cell.i, cell.j, i].State == cell.State)) return true;

		// Check XY diagonals
		if (Enumerable.Range(0, 3).All(i => Cells[i, i, cell.k].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[i, 2 - i, cell.k].State == cell.State)) return true;

		// Check YZ diagonals
		if (Enumerable.Range(0, 3).All(i => Cells[cell.i, i, i].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[cell.i, 2 - i, i].State == cell.State)) return true;

		// Check XZ diagonals
		if (Enumerable.Range(0, 3).All(i => Cells[i, cell.j, i].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[2 - i, cell.j, i].State == cell.State)) return true;
		

		// Check XYZ diagonals
		if (Enumerable.Range(0, 3).All(i => Cells[i, i, i].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[2 - i, i, i].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[i, 2 - i, i].State == cell.State)) return true;
		if (Enumerable.Range(0, 3).All(i => Cells[i, i, 2 - i].State == cell.State)) return true;

		return false;
	}

	public void NextTurn(Cell cell)
	{
		GD.Print("NEXT TURN");
		// Check win condition
		if (IsWin(cell))
		{
			var winMessage = GD.Load<PackedScene>("res://Scenes/WinMessage.tscn").Instantiate() as Control;
			var playerWonLabel = winMessage.GetNode<Label>("Container/Label");
			playerWonLabel.Text = cell.State == Player.RedPlayer ? "RED PLAYER HAS WON" : "BLUE PLAYER HAS WON";
			AddChild(winMessage);
			return;
		}
		
		// Change current player
		if (CurrentPlayer == Player.RedPlayer) CurrentPlayer = Player.BluePlayer;
		else CurrentPlayer = Player.RedPlayer;

		// Make best move if singleplayer
		if (Game.CurrentGameMode == Game.GameMode.Singleplayer && CurrentPlayer == Player.BluePlayer)
		{
			GD.Print("MINIMAX TURN");
			MakeBestMove();
		}
	}

	public int AlphaBeta(Cell node, int depth, int alpha, int beta, bool isMaximizer)
	{
		if (IsWin(node) && node.State == Player.BluePlayer)
		{
			return 1;
		}
		if (IsWin(node) && node.State == Player.RedPlayer)
		{
			return -1;
		}
		if (depth == 4)
		{
			return 0;
		}

		if (isMaximizer)
		{
			int value = int.MinValue;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						var child = Cells[i, j, k];
						if (child.State == Player.NoPlayer)
						{
							child.State = Player.BluePlayer;
							value = Math.Max(value, AlphaBeta(child, depth + 1, alpha, beta, false));
							child.State = Player.NoPlayer;
							if (value >= beta) return value;
							alpha = Math.Max(alpha, value);
						}
					}
				}
			}
			return value;
		}
		else
		{
			int value = int.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						var child = Cells[i, j, k];
						if (child.State == Player.NoPlayer)
						{
							child.State = Player.RedPlayer;
							value = Math.Min(value, AlphaBeta(child, depth + 1, alpha, beta, true));
							child.State = Player.NoPlayer;
							if (value <= alpha) return value;
							beta = Math.Min(beta, value);
						}
					}
				}
			}
			return value;
		}
	}

	public struct Move
	{
		public int i, j, k;
	}		

	public void MakeBestMove()
	{
		int bestValue = int.MinValue;
		Move move = new Move();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					var origin = Cells[i, j, k];
					if (origin.State == Player.NoPlayer)
					{
						origin.State = Player.BluePlayer;
						int value = AlphaBeta(origin, 0, int.MinValue, int.MaxValue, false);
						origin.State = Player.NoPlayer;
						if (value > bestValue)
						{
							bestValue = value;
							move.i = i;
							move.j = j;
							move.k = k;
						}
					}
				}
			}
		}
		Cells[move.i, move.j, move.k].Fill();
		GD.Print("MADE BEST TURN");
	}
}