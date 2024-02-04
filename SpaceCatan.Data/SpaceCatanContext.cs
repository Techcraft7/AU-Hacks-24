using Microsoft.EntityFrameworkCore;

namespace SpaceCatan.Data;

public class SpaceCatanContext : DbContext
{
	public string DbPath { get; set; }

	public SpaceCatanContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = Path.Join(path, "SpaceCatan.db");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={DbPath}");
	}

	public DbSet<User> Users { get; set; }
}
