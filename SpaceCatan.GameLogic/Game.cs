namespace SpaceCatan.GameLogic;

public sealed class Game
{
	public Map Map { get; set; } = new();
	public Player[] Players { get; set; } = new Player[4];
	public int CurrentPlayer => (turnIndex % Players.Length) + 1;
	public bool IsInSetup => turnIndex < Players.Length;
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

	public void MakeSetupTurn(int playerID, SetupTurn turn)
	{
		if (!IsInSetup || playerID < 1 || playerID > 4)
		{
			return;
		}

		Map.SetPlanetOwner(turn.X, turn.Y, playerID);
		Map.SetRoad(turn.X, turn.Y, turn.Direction, playerID);
	}

	public void MakeTurn(int playerID, Turn turn)
	{
		if (IsInSetup || playerID < 1 ||  playerID > 4)
		{
			return;
		}
		Player p = Players[playerID - 1];

		Dictionary<Resource, int> resources = new() {
			[Resource.GRAVITRONIUM] = p.Gravitronium,
			[Resource.COBALT] = p.Cobalt,
			[Resource.OXYGEN] = p.Oxygen,
			[Resource.FOOD] = p.Food,
			[Resource.WATER] = p.Water
		};

		foreach (var (resource, count) in turn.ResourcesTraded)
		{
			resources[resource] += count;
		}

		// Roads
		resources[Resource.GRAVITRONIUM] -= 1 * turn.Roads.Length;
		resources[Resource.COBALT] -= 1 * turn.Roads.Length;

		// Colonies
		resources[Resource.GRAVITRONIUM] -= 1 * turn.Colonies.Length;
		resources[Resource.OXYGEN] -= 1 * turn.Colonies.Length;
		resources[Resource.FOOD] -= 1 * turn.Colonies.Length;

		// Update resource counds
		p.Gravitronium += resources[Resource.GRAVITRONIUM];
		p.Cobalt += resources[Resource.COBALT];
		p.Oxygen += resources[Resource.OXYGEN];
		p.Water += resources[Resource.WATER];
		p.Food += resources[Resource.FOOD];

		// Build roads
		foreach (RoadToBuild r in turn.Roads)
		{
			Map.SetRoad(r.X, r.Y, r.Direction, playerID);
		}

		// Build colonies
		foreach (ColonyToBuild c in turn.Colonies)
		{
			Map.SetPlanetOwner(c.X, c.Y, playerID);
		}

		// TODO: apply development cards

		Players[playerID - 1] = p;
		turnIndex++;
	}

	public int? GetWinner()
	{
		Span<int> counts = stackalloc int[4];
		counts.Clear();
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				Planet p = Map.GetPlanet(x, y);
				if (p.Owner is >= 1 and <= 4)
				{
					counts[p.Owner] += 1;
				}
			}
		}

		for (int i = 0; i < 4; i++)
		{
			if (counts[i] >= 5)
			{
				return i + 1;
			}
		}
		return null;
	}
}
