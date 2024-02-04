namespace SpaceCatan.Data;

public interface IUserStore
{
	public Task<(User, Exception?)> CreateUser(string userID, CancellationToken token);
	public Task<(User?, Exception?)> GetUser(string id, CancellationToken cancellationToken);
	public Task<Exception?> UpdateUser(User user, CancellationToken cancellationToken);
}
