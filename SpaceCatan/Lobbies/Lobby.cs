namespace SpaceCatan.Lobbies;

public sealed class Lobby
{
	public Guid ID { get; set; }
	public Dictionary<string, int> PlayerIDMap { get; set; } = [];
	public bool HasStarted { get; private set; }
	public Game Game { get; set; } = new();
	public event Func<Lobby, Task>? LobbyUpdated;
	private readonly SemaphoreSlim semaphore = new(1);

	public async Task<bool> TryAddPlayer(User user)
	{
		if (HasStarted)
		{
			return false;
		}
		await semaphore.WaitAsync();
		PlayerIDMap.Add(user.ID, 0);
		if (PlayerIDMap.Count >= 4)
		{
			HasStarted = true;
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
