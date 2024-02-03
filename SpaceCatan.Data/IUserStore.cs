namespace SpaceCatan.Data;

public interface IUserStore
{
	public Task<(User, Exception?)> CreateUser(User user, CancellationToken token);
	public Task<(User?, Exception?)> GetUser(string id, CancellationToken cancellationToken);
	public Task<Exception?> UpdateUser(User user, CancellationToken cancellationToken);
}
