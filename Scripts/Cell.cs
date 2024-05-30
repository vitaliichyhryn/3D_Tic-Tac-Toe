using Godot;

public partial class Cell : Node3D
{
	public Board Board => GetNode("..") as Board;
	public Board.Player Player = Board.Player.None;
	public MeshInstance3D Mesh => GetNode<MeshInstance3D>("StaticBody/Mesh");
	public int i, j, k;

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

	public void OnStaticBodyMouseEntered()
	{
		if (Player == Board.Player.None)
		{
			Mesh.SetInstanceShaderParameter("alpha", 0.5f);
			if (Board.CurrentPlayer == Board.Player.Red) Mesh.SetInstanceShaderParameter("color", new Color("f44c80"));
			if (Board.CurrentPlayer == Board.Player.Blue) Mesh.SetInstanceShaderParameter("color", new Color("7385fa"));
		}
	}

	public void OnStaticBodyMouseExited()
	{
		if (Player == Board.Player.None)
		{
			Mesh.SetInstanceShaderParameter("alpha", 0.1f);
			Mesh.SetInstanceShaderParameter("color", new Color("ffffff"));
		}
	}

	public void OnStaticBodyInputEvent(Camera3D camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeID)
	{
		
		if (Input.IsActionJustPressed("UILeftClick"))
		{
			if (Player == Board.Player.None) Fill();
		}
	}

	public void Fill()
	{
		Player = Board.CurrentPlayer;
		Mesh.SetInstanceShaderParameter("alpha", 1f);
		if (Player == Board.Player.Red) Mesh.SetInstanceShaderParameter("color", new Color("f44c80"));
		if (Player == Board.Player.Blue) Mesh.SetInstanceShaderParameter("color", new Color("7385fa"));
		EmitSignal(SignalName.Filled, this);
	}
}