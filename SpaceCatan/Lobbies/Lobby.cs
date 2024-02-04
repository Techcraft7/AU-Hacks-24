namespace SpaceCatan.Lobbies;

public sealed class Lobby
{
	public Guid ID { get; set; }
	public Dictionary<string, int> PlayerIDMap { get; set; } = [];
	public bool HasStarted { get; private set; }
	public int? Winner { get; private set; }
	public Game Game { get; private set; } = new();
	public IReadOnlyList<string> Log => log;
	public event Func<Lobby, Task>? LobbyUpdated;
	private readonly SemaphoreSlim semaphore = new(1);
	private readonly List<string> log = [];
	private readonly Dictionary<string, bool> LeftPlayers = [];

	public async Task<bool> TryAddPlayer(User user)
	{
		if (HasStarted)
		{
			return false;
		}
		if (PlayerIDMap.ContainsKey(user.ID))
		{
			return false;
		}
		await semaphore.WaitAsync();
		PlayerIDMap.Add(user.ID, 0);
		log.Add($"Waiting: {PlayerIDMap.Count}/4");
		if (PlayerIDMap.Count >= 4)
		{
			log.Add($"Game Started!");
			HasStarted = true;
            string[] keys = [..PlayerIDMap.Keys];
			Random.Shared.Shuffle(keys);
			for (int i = 0; i < 4; i++)
			{
				PlayerIDMap[keys[i]] = i + 1;
				LeftPlayers[keys[i]] = false;
			}
		}
		semaphore.Release();
		await Update();
		return true;
	}

	public async Task LeaveAsync(User user)
	{
		await semaphore.WaitAsync();
		if (HasStarted)
		{
			LeftPlayers[user.ID] = true;
			if (Game.CurrentPlayer == PlayerIDMap.GetValueOrDefault(user.ID))
			{
				await PlayBotMove(user.ID);
			}
			return;
		}
		PlayerIDMap.Remove(user.ID);
		log.Add($"Waiting... {PlayerIDMap.Count}/4");
		semaphore.Release();
		await Update();
	}

	private async Task PlayBotMove(string userID)
	{
		if (Game.IsInSetup)
		{
			int x, y;
			Direction dir;
			do
			{
				x = Random.Shared.Next(5);
				y = Random.Shared.Next(5);
				dir = (Direction)Random.Shared.Next(4);
			} while (Game.Map.GetPlanet(x, y).Owner != 0 || Game.Map.GetRoad(x, y, dir) != 0);
			await PlaySetupTurn(userID, new(x, y, dir));
		}
		else
		{
			await PlayTurn(userID, new([], [], [], 0, false));
		}
	}

	public async Task PlaySetupTurn(string userID, SetupTurn turn)
	{
		if (!PlayerIDMap.TryGetValue(userID, out int playerID))
		{
			return;
		}
		await semaphore.WaitAsync();
		if (Game.IsInSetup)
		{
			Game.MakeSetupTurn(playerID, turn);

			// This will happen after the last player finishes the setup
			if (Game.TurnIndex >= 4)
			{
				log.Add($"Picking resource #{Game.GiveResources()}");
			}
		}
		semaphore.Release();
		await Update();
		if (HasCurrentPlayerLeft(out string nextID))
		{
			await PlayBotMove(nextID);
		}
	}

	public async Task PlayTurn(string userID, Turn turn)
	{
		if (!PlayerIDMap.TryGetValue(userID, out int playerID))
		{
			return;
		}
		await semaphore.WaitAsync();
		if (!Game.IsInSetup)
		{
			int current = Game.CurrentPlayer;

			Game.MakeTurn(playerID, turn);

			log.Add($"Picking resource #{Game.GiveResources()}");
			if (Game.DevelopmentCardData is DevelopmentCardPlayedData data)
			{
				log.Add(data.Kind switch
				{
					DevelopmentCardKind.HEIST => "A hiest occured!",
					DevelopmentCardKind.NUKE => "Somebody's Colony has been nuked!",
					DevelopmentCardKind.DROUGHT => "A drought occured! Everybody loses 1 resource!",
					DevelopmentCardKind.OVERTIME => $"Player {current} takes another turn!",
					DevelopmentCardKind.INDUSTRIALIZATION => "A planet has been over-harvested!",
					DevelopmentCardKind.CLIMATE_CHANGE => "A planet has changed resources!",
					_ => "A Chaos Card was played!"
				});
			}

			if (Game.GetWinner() is int winner)
			{
				log.Add($"Player {winner} wins!");
				Winner = winner;
			}
		}
		semaphore.Release();
		await Update();
		if (HasCurrentPlayerLeft(out string nextID))
		{
			await PlayBotMove(nextID);
		}
	}

	private async Task Update()
	{
		await semaphore.WaitAsync();
		semaphore.Release();
		await (LobbyUpdated?.Invoke(this) ?? Task.CompletedTask);
		if (HasStarted && LeftPlayers.Values.All(x => x))
		{
			Winner = 0;
		}
	}

	private bool HasCurrentPlayerLeft(out string userID) => LeftPlayers.GetValueOrDefault(userID = PlayerIDMap.FirstOrDefault(kv => kv.Value == Game.CurrentPlayer).Key);
}
