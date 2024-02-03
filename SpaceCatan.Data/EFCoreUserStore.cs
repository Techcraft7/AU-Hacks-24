
using Microsoft.EntityFrameworkCore;

namespace SpaceCatan.Data;

public sealed class EFCoreUserStore(SpaceCatanContext context) : IUserStore
{
	public async Task<(User, Exception?)> CreateUser(string userID, CancellationToken cancellationToken)
	{
		(User? existing, Exception? error) = await GetUser(userID, cancellationToken);
		if (error is not null)
		{
			return (null!, error);
		}
		if (existing is not null)
		{
			return (existing, null);
		}

		User user = new()
		{
			ID = userID,
			CurrentGame = null,
			Wins = 0
		};

		try
		{
			await context.Users.AddAsync(user, cancellationToken);
			return (user, null);
		}
		catch (Exception e)
		{
			return (null!, e);
		}
	}

	public async Task<(User?, Exception?)> GetUser(string id, CancellationToken cancellationToken)
	{
		try
		{
			return (await context.Users.FirstOrDefaultAsync(u => u.ID == id, cancellationToken), null);
		}
		catch (Exception e)
		{
			return (null, e);
		}
	}

	public async Task<Exception?> UpdateUser(User user, CancellationToken cancellationToken)
	{
		(User? current, Exception? error) = await GetUser(user.ID, cancellationToken); 	
		if (error is not null)
		{
			return error;
		}

		if (current is null)
		{
			return null;
		}
		current.Wins = user.Wins;
		current.CurrentGame = user.CurrentGame;
		
		try
		{
			await context.SaveChangesAsync(cancellationToken);
			return null;
		}
		catch (Exception e)
		{
			return e;
		}
	}
}
