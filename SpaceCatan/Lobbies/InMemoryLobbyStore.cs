
using Microsoft.Extensions.Logging;

namespace SpaceCatan.Lobbies;

public sealed class InMemoryLobbyStore(ILogger<InMemoryLobbyStore> logger) : ILobbyStore
{
	private readonly SemaphoreSlim semaphore = new(1);
	private readonly SortedDictionary<Guid, Lobby> lobbies = [];

	public Task<(Lobby?, Exception?)> GetLobby(Guid id, CancellationToken cancellationToken)
	{
		if (lobbies.TryGetValue(id, out Lobby? lobby))
		{
			return Task.FromResult<(Lobby?, Exception?)>((lobby, null));
		}
		return Task.FromResult<(Lobby?, Exception?)>((null, null));
	}

	public async Task<(Lobby, Exception?)> GetJoinableLobby(CancellationToken cancellationToken)
	{
		await Task.Yield();
		Lobby? lobby = lobbies.Values.FirstOrDefault(l => !l.HasStarted);
		if (lobby is not null)
		{
			return (lobby, null);
		}
		lobby = new()
		{
			ID = Guid.NewGuid()
		};

		await semaphore.WaitAsync(cancellationToken);
		lobbies.Add(lobby.ID, lobby);
		logger.LogInformation("Created lobby {id}", lobby.ID);
		lobby.OnGameEnd += async (l) =>
		{
			await semaphore.WaitAsync();
			lobbies.Remove(l.ID);
			logger.LogInformation("Deleted lobby {id}", lobby.ID);
			semaphore.Release();
		};
		semaphore.Release();

		return (lobby, null);
	}
}
