public static class Game
{
	public enum GameMode
	{
		SinglePlayer,
		MultiPlayer
	}

	public static GameMode CurrentGameMode { get; set; }
	public static Board.Player Winner { get; set; }
}
