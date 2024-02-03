using Microsoft.EntityFrameworkCore;

namespace SpaceCatan.Data;

public class SpaceCatanContext : DbContext
{
	public DbSet<User> Users { get; set; }
}
