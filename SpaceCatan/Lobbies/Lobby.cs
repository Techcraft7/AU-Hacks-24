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
			int[] ids = [1, 2, 3, 4];
			int i = 4;
			foreach (string k in PlayerIDMap.Keys)
			{
				int j = Random.Shared.Next(i);
				PlayerIDMap[k] = ids[j];
				ids[j] = ids[i];
				i--;
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
