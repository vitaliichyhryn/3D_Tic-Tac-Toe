public static class Game
{
	public enum GameMode
	{
		Singleplayer,
		Multiplayer
	}

	public static GameMode CurrentGameMode { get; set; }
}
