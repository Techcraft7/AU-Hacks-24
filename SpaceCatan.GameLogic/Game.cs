namespace SpaceCatan.GameLogic;

public sealed class Game
{
	public Map Map { get; set; } = new();
	public Player[] Players { get; set; } = new Player[4];
	public int CurrentPlayer => (turnIndex % Players.Length) + 1;
	private int turnIndex = 0;

	public Game()
	{
		for (int i = 0; i < 4; i++)
		{
			Players[i] = new Player()
			{
				ID = i + 1
			};
		}
	}

	public void MakeTurn(int playerID, Turn turn)
	{
		if (playerID < 1 ||  playerID > 4)
		{
			return;
		}
		Player p = Players[playerID - 1];

		// TODO: check 

		turnIndex++;
	}
}
