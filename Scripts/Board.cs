using System.Linq;
using Godot;

public partial class Board : Node3D
{
    public enum Player
    {
        None = 0,
        Red = -1,
        Blue = 1
    }

    private const int Size = 3;
    private const int SearchDepth = 3;
    private const int ScoreLimit = 10;
    private readonly Cell[,,] _cells = new Cell[Size, Size, Size];
    public Player CurrentPlayer { get; private set; } = Player.Red;
    private Player ComputerPlayer { get; set; } = GD.Randi() % 2 == 0 ? Player.Red : Player.Blue;
    private Label CurrentTurn => GetNode<Label>("MarginContainer/CurrentTurn");
    private static PackedScene WinMessage => GD.Load<PackedScene>("res://Scenes/WinMessage.tscn");
    private bool _gameWon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Create();
        CurrentTurn.Text = "CURRENT TURN: " + GetPlayerName(CurrentPlayer);
        if (Game.CurrentGameMode == Game.GameMode.SinglePlayer && CurrentPlayer == ComputerPlayer) MakeBestMove();
    }

    private void Create()
    {
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        for (var k = 0; k < Size; k++)
        {
            var cell = GD.Load<PackedScene>("res://Scenes/Cell.tscn").Instantiate() as Cell;
            cell.Create(i, j, k);
            AddChild(cell);
            _cells[i, j, k] = cell;
            cell.Filled += NextTurn;
        }
    }

    private void Delete()
    {
        CurrentTurn.Text = "";
        foreach (var cell in _cells) cell.QueueFree();
    }

    public static string GetPlayerName(Player player)
    {
        if (player == Player.None) return "NONE";
        return (player == Player.Red ? "RED" : "BLUE") + " PLAYER";
    }

    private bool IsWin(Cell cell)
    {
        // Check X row
        if (Enumerable.Range(0, Size).All(i => _cells[i, cell.Y, cell.Z].Player == cell.Player)) return true;

        // Check Y column
        if (Enumerable.Range(0, Size).All(i => _cells[cell.X, i, cell.Z].Player == cell.Player)) return true;

        // Check Z tube
        if (Enumerable.Range(0, Size).All(i => _cells[cell.X, cell.Y, i].Player == cell.Player)) return true;

        // Check XY diagonals
        if (Enumerable.Range(0, Size).All(i => _cells[i, i, cell.Z].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[i, Size - 1 - i, cell.Z].Player == cell.Player)) return true;

        // Check YZ diagonals
        if (Enumerable.Range(0, Size).All(i => _cells[cell.X, i, i].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[cell.X, Size - 1 - i, i].Player == cell.Player)) return true;

        // Check XZ diagonals
        if (Enumerable.Range(0, Size).All(i => _cells[i, cell.Y, i].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[Size - 1 - i, cell.Y, i].Player == cell.Player)) return true;

        // Check XYZ diagonals
        if (Enumerable.Range(0, Size).All(i => _cells[i, i, i].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[Size - 1 - i, i, i].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[i, Size - 1 - i, i].Player == cell.Player)) return true;
        if (Enumerable.Range(0, Size).All(i => _cells[i, i, Size - 1 - i].Player == cell.Player)) return true;

        return false;
    }

    private void NextTurn(Cell cell)
    {
        if (IsWin(cell))
        {
            _gameWon = true;
            Delete();
            Game.Winner = CurrentPlayer;
            AddChild(WinMessage.Instantiate());
            return;
        }

        CurrentPlayer = CurrentPlayer == Player.Red ? Player.Blue : Player.Red;
        CurrentTurn.Text = "CURRENT TURN: " + GetPlayerName(CurrentPlayer);

        if (Game.CurrentGameMode == Game.GameMode.SinglePlayer && CurrentPlayer == ComputerPlayer) MakeBestMove();
    }

    private int Negamax(Cell origin, int alpha, int beta, int depth)
    {
        if (IsWin(origin)) return -(1 + depth);
        if (depth == 0) return 0;
        foreach (var cell in _cells)
            if (cell.Player == Player.None)
            {
                cell.Player = origin.Player == Player.Red ? Player.Blue : Player.Red;
                var score = -Negamax(cell, -beta, -alpha, depth - 1);
                cell.Player = Player.None;
                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }

        return alpha;
    }

    private void MakeBestMove()
    {
        var bestScore = -ScoreLimit;
        Cell bestCell = null;
        foreach (var cell in _cells)
            if (cell.Player == Player.None)
            {
                cell.Player = ComputerPlayer;
                var score = -Negamax(cell, -ScoreLimit, ScoreLimit, SearchDepth);
                cell.Player = Player.None;
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCell = cell;
                }
            }

        bestCell.Fill();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (Input.IsActionJustPressed("Save") && !_gameWon)
        {
            Save();
            GD.Print("saved");
        }

        if (Input.IsActionJustPressed("Load") && !_gameWon)
        {
            Load();
            GD.Print("loaded");
        }
    }

    public void Save()
    {
        using var file = FileAccess.Open("res://save.dat", FileAccess.ModeFlags.Write);
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        for (var k = 0; k < Size; k++)
            file.StoreVar((int)_cells[i, j, k].Player);
        file.StoreVar((int)ComputerPlayer);
        file.StoreVar((int)CurrentPlayer);
    }

    public void Load()
    {
        Delete();
        Create();
        using var file = FileAccess.Open("res://save.dat", FileAccess.ModeFlags.Read);
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        for (var k = 0; k < Size; k++)
        {
            var player = (Player)file.GetVar().AsInt32();
            if (player != Player.None) _cells[i, j, k].Fill(player);
        }

        ComputerPlayer = (Player)file.GetVar().AsInt32();
        CurrentPlayer = (Player)file.GetVar().AsInt32();
        CurrentTurn.Text = "CURRENT TURN: " + GetPlayerName(CurrentPlayer);
    }
}