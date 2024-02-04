namespace SpaceCatan.Lobbies;

public sealed class Lobby
{
	public Guid ID { get; set; }
	public Dictionary<string, int> PlayerIDMap { get; set; } = [];
	public bool HasStarted { get; private set; }
	public Game Game { get; private set; } = new();
	public IReadOnlyList<string> Log => log;
	public event Func<Lobby, Task>? LobbyUpdated;
	private readonly SemaphoreSlim semaphore = new(1);
	private readonly List<string> log = [];

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
			}
		}
		semaphore.Release();
		await Update();
		return true;
	}

	public async Task PlaySetupTurn(User user, SetupTurn turn)
	{
		if (!PlayerIDMap.TryGetValue(user.ID, out int playerID))
		{
			return;
		}
		await semaphore.WaitAsync();
		if (Game.IsInSetup)
		{
			Game.MakeSetupTurn(playerID, turn);
		}
		semaphore.Release();
		await Update();
	}

	public async Task PlayTurn(User user, Turn turn)
	{
		if (!PlayerIDMap.TryGetValue(user.ID, out int playerID))
		{
			return;
		}
		await semaphore.WaitAsync();
		if (!Game.IsInSetup)
		{
			Game.MakeTurn(playerID, turn);
		}
		semaphore.Release();
		await Update();
	}

	private async Task Update() => await (LobbyUpdated?.Invoke(this) ?? Task.CompletedTask);
}
