namespace SpaceCatan.GameLogic;

public sealed class Game
{
	public Map Map { get; set; } = new();
	public Player[] Players { get; set; } = new Player[4];

	public Game()
	{
		for (int i = 0; i < 4; i++)
		{
			Players[i] = new Player()
			{
				ID = i
			};
		}
	}

	public void MakeTurn(Turn turn)
	{

	}
}
