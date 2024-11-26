using Godot;

public partial class Cell : Node3D
{
	public Board.Player Player = Board.Player.None;
	private Board Board => GetNode("..") as Board;
	private MeshInstance3D Mesh => GetNode<MeshInstance3D>("StaticBody/Mesh");
	public int X, Y, Z;

	[Signal]
	public delegate void FilledEventHandler(Cell cell);

	public void Create(int x, int y, int z)
	{
		Position = new Vector3(2f * (x - 1), 2f * (y - 1), 2f * (z - 1));
		X = x;
		Y = y;
		Z = z;
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
    
	public void Fill(Board.Player player)
	{
		Player = player;
		Mesh.SetInstanceShaderParameter("alpha", 1f);
		if (Player == Board.Player.Red) Mesh.SetInstanceShaderParameter("color", new Color("f44c80"));
		if (Player == Board.Player.Blue) Mesh.SetInstanceShaderParameter("color", new Color("7385fa"));
	}
}