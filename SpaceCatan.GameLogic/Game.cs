namespace SpaceCatan.GameLogic;

public sealed class Game
{
	public Map Map { get; set; } = new();
	public Player[] Players { get; set; } = new Player[4];
	public int CurrentPlayer => (turnIndex % Players.Length) + 1;
	public bool IsInSetup => turnIndex < Players.Length;
	public DevelopmentCardPlayedData? DevelopmentCardData { get; set; }
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

		// Development Cards
		resources[Resource.COBALT] -= 1 * turn.DevelopmentCardsBought;
		resources[Resource.FOOD] -= 1 * turn.DevelopmentCardsBought;
		resources[Resource.WATER] -= 1 * turn.DevelopmentCardsBought;

		// Update resource counds
		p.Gravitronium += resources[Resource.GRAVITRONIUM];
		p.Cobalt += resources[Resource.COBALT];
		p.Oxygen += resources[Resource.OXYGEN];
		p.Water += resources[Resource.WATER];
		p.Food += resources[Resource.FOOD];
		p.DevelopmentCards += turn.DevelopmentCardsBought;

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

		if (turn.DevelopmentCardUsed)
		{
			p.DevelopmentCards -= 1;
			switch (Random.Shared.Next(6))
			{
				case 0: // Steal 2 resources (Heist)
					int robbedPlayer;
					while (true)
					{
						robbedPlayer = Random.Shared.Next(4);
						if (robbedPlayer + 1 != playerID)
						{
							break;
						}
					}
					for (int i = 0; i < 2; i++)
					{
						if (TakeResource(robbedPlayer, null, 1) is Resource r)
						{
							AddResource(playerID, r, 1);
						}
					}
					DevelopmentCardData = new(DevelopmentCardKind.HEIST, 0, 0, 0);
					break;
				case 1: // Nuke a colony (Nuke)
					int x, y;
					while (true)
					{
						x = Random.Shared.Next(5);
						y = Random.Shared.Next(5);
						int owner = Map.GetPlanet(x, y).Owner;
						if (owner >= 1 && owner != playerID)
						{
							break;
						}
					}
					Map.SetPlanetOwner(x, y, 0);
					Map.SetRoad(x, y, Direction.UP, 0);
					Map.SetRoad(x, y, Direction.DOWN, 0);
					Map.SetRoad(x, y, Direction.LEFT, 0);
					Map.SetRoad(x, y, Direction.RIGHT, 0);
					DevelopmentCardData = new(DevelopmentCardKind.NUKE, x, y, 0);
					break;
				case 2: // Everybody loses 1 of a random resource (Drought)
					Resource droughtResource = Random.Shared.Next(5) switch
					{
						0 => Resource.GRAVITRONIUM,
						1 => Resource.COBALT,
						2 => Resource.OXYGEN,
						3 => Resource.FOOD,
						_ => Resource.WATER,
					};
					for (int i = 0; i < 4; i++)
					{
						_ = TakeResource(i + 1, droughtResource, 1);
					}
					DevelopmentCardData = new(DevelopmentCardKind.HEIST, 0, 0, droughtResource);
					break;
				case 3: // Make it your turn again (Overtime)
					turnIndex--;
					DevelopmentCardData = new(DevelopmentCardKind.OVERTIME, 0, 0, 0);
					break;
				case 4: // Make a random planet empty (Industrialization)
					while (true)
					{
						x = Random.Shared.Next(5);
						y = Random.Shared.Next(5);
						if (Map.GetPlanet(x, y).Kind is not (PlanetKind.EMPTY or PlanetKind.OUTPOST))
						{
							break;
						}
					}
					Map.SetPlanetKind(x, y, PlanetKind.EMPTY);
					DevelopmentCardData = new(DevelopmentCardKind.INDUSTRIALIZATION, x, y, 0);
					break;
				case 5: // A random planet becomes a different resource (Climate Change)
					while (true)
					{
						x = Random.Shared.Next(5);
						y = Random.Shared.Next(5);
						if (Map.GetPlanet(x, y).Kind is not (PlanetKind.EMPTY or PlanetKind.OUTPOST))
						{
							break;
						}
					}
					// Pick random resource (but different from current)
					Span<PlanetKind> kinds = [PlanetKind.GRAVITRONIUM, PlanetKind.COBALT, PlanetKind.OXYGEN, PlanetKind.FOOD, PlanetKind.WATER];
					for (int i = 0; i < kinds.Length; i++)
					{
						if (kinds[i] == Map.GetPlanet(x, y).Kind)
						{
							kinds[i] = kinds[^1];
							kinds = kinds[..^1];
							break;
						}
					}
					Map.SetPlanetKind(x, y, kinds[Random.Shared.Next(kinds.Length)]);
					DevelopmentCardData = new(DevelopmentCardKind.CLIMATE_CHANGE, x, y, 0);
					break;
			}
		}
		else
		{
			DevelopmentCardData = null;
		}

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

	private Resource? TakeResource(int playerID, Resource? resource, int count)
	{
		if (playerID is < 1 or > 4)
		{
			return null;
		}
		Player p = Players[playerID - 1];
		
		// Pick a random resource that they do have
		if (resource is null)
		{
			Span<int> counts = [p.Gravitronium, p.Cobalt, p.Oxygen, p.Food, p.Water];
			Span<Resource> kinds = [Resource.GRAVITRONIUM, Resource.COBALT, Resource.OXYGEN, Resource.FOOD, Resource.WATER];

			// If they have no resources then bail
			// (the while loop below would go forever)
			if (!counts.ContainsAnyExcept(0))
			{
				return null;
			}

			int i;
			while (true)
			{
				i = Random.Shared.Next(counts.Length);
				if (counts[i] != 0)
				{
					break;
				}
			}
			TakeResource(playerID, kinds[i], count);
		}


		switch (resource)
		{
			case Resource.GRAVITRONIUM:
				p.Gravitronium -= count;
				break;
			case Resource.COBALT:
				p.Cobalt -= count;
				break;
			case Resource.OXYGEN:
				p.Oxygen -= count;
				break;
			case Resource.FOOD:
				p.Food -= count;
				break;
			case Resource.WATER:
				p.Water -= count;
				break;
		}
		Players[playerID - 1] = p;
		return resource;
	}

	private void AddResource(int playerID, Resource resource, int count)
	{
		if (playerID is < 1 or > 4)
		{
			return;
		}
		Player p = Players[playerID - 1];
		switch (resource)
		{
			case Resource.GRAVITRONIUM:
				p.Gravitronium += count;
				break;
			case Resource.COBALT:
				p.Cobalt += count;
				break;
			case Resource.OXYGEN:
				p.Oxygen += count;
				break;
			case Resource.FOOD:
				p.Food += count;
				break;
			case Resource.WATER:
				p.Water += count;
				break;
		}
		Players[playerID - 1] = p;
	}
}
