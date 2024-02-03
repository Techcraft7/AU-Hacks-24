using Microsoft.EntityFrameworkCore;

namespace SpaceCatan.Data;

public class SpaceCatanContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
}
