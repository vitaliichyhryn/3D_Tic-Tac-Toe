using Godot;
using System.Linq;

public partial class Board : Node3D
{
	public enum Player
	{
		None,
		Red = -1,
		Blue = 1
	}

	public const int Size = 3;
	public const int SearchDepth = 3;
	public const int ScoreLimit = 5;
	public Cell[,,] Cells = new Cell[Size, Size, Size];
	public Player CurrentPlayer;
	Label CurrentTurn => GetNode<Label>("MarginContainer/CurrentTurn");
	PackedScene WinMessage = GD.Load<PackedScene>("res://Scenes/WinMessage.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Create();
		CurrentPlayer = Player.Red;
		CurrentTurn.Text = "CURRENT TURN: " + GetPlayerName(CurrentPlayer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Create()
	{
		for (int i = 0; i < Size; i++)
		{
			for (int j = 0; j < Size; j++)
			{
				for (int k = 0; k < Size; k++)
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

	public void Delete()
	{
		CurrentTurn.QueueFree();
		foreach (var cell in Cells)
		{
			cell.QueueFree();
		}
	}

	public static string GetPlayerName(Player player)
	{
		if (player == Player.None) return "NONE";
		return (player == Player.Red ? "RED" : "BLUE") + " PLAYER";
	}

	public Player GetWinner(Cell cell)
	{
		// Check X row
		if (Enumerable.Range(0, Size).All(i => Cells[i, cell.j, cell.k].Player == cell.Player)) return cell.Player;

		// Check Y column
		if (Enumerable.Range(0, Size).All(i => Cells[cell.i, i, cell.k].Player == cell.Player)) return cell.Player;

		// Check Z tube
		if (Enumerable.Range(0, Size).All(i => Cells[cell.i, cell.j, i].Player == cell.Player)) return cell.Player;

		// Check XY diagonals
		if (Enumerable.Range(0, Size).All(i => Cells[i, i, cell.k].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[i, Size - 1 - i, cell.k].Player == cell.Player)) return cell.Player;

		// Check YZ diagonals
		if (Enumerable.Range(0, Size).All(i => Cells[cell.i, i, i].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[cell.i, Size - 1 - i, i].Player == cell.Player)) return cell.Player;

		// Check XZ diagonals
		if (Enumerable.Range(0, Size).All(i => Cells[i, cell.j, i].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[Size - 1 - i, cell.j, i].Player == cell.Player)) return cell.Player;
		
		// Check XYZ diagonals
		if (Enumerable.Range(0, Size).All(i => Cells[i, i, i].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[Size - 1 - i, i, i].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[i, Size - 1 - i, i].Player == cell.Player)) return cell.Player;
		if (Enumerable.Range(0, Size).All(i => Cells[i, i, Size - 1 - i].Player == cell.Player)) return cell.Player;

		return Player.None;
	}

	public void NextTurn(Cell cell)
	{
		// Check win condition
		if (GetWinner(cell) != Player.None)
		{
			Delete();
			Game.Winner = CurrentPlayer;
			AddChild(WinMessage.Instantiate());
			return;
		}
		
		// Change current player
		if (CurrentPlayer == Player.Red) CurrentPlayer = Player.Blue;
		else CurrentPlayer = Player.Red;
		CurrentTurn.Text = "CURRENT TURN: " + GetPlayerName(CurrentPlayer);

		// Make best move if singleplayer
		if (Game.CurrentGameMode == Game.GameMode.Singleplayer && CurrentPlayer == Player.Blue)
		{
			MakeBestMove();
		}
	}

	public int NegaMax(Cell origin, int alpha, int beta, int depth)
	{		
		if (GetWinner(origin) != Player.None) return -(1 + depth);
		if (depth == 0) return 0;
		foreach (var cell in Cells)
		{
			if (cell.Player == Player.None)
			{
				cell.Player = origin.Player == Player.Red ? Player.Blue : Player.Red;
				int score = -NegaMax(cell, -beta, -alpha, depth - 1);
				cell.Player = Player.None;
				if (score >= beta)
					return beta;
				if (score > alpha)
					alpha = score;
			}
		}
		return alpha;
	}

	public void MakeBestMove()
	{
		var bestScore = -ScoreLimit;
		Cell bestCell = null;
		foreach (var cell in Cells)
		{
			if (cell.Player == Player.None)
			{
				cell.Player = Player.Blue;
				int score = -NegaMax(cell, -ScoreLimit, ScoreLimit,  SearchDepth);
				cell.Player = Player.None;
				if (score > bestScore)
				{
					bestScore = score;
					bestCell = cell;
				}
			}
		}
		bestCell.Fill();
	}
}