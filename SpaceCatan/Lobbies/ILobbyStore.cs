namespace SpaceCatan.Lobbies;

public interface ILobbyStore
{
	public Task<(Lobby?, Exception?)> GetLobby(Guid id, CancellationToken cancellationToken);
	public Task<(Lobby, Exception?)> GetJoinableLobby(CancellationToken cancellationToken);
}
