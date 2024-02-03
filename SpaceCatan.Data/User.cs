using System.ComponentModel.DataAnnotations;

namespace SpaceCatan.Data;

public sealed class User
{
	[Key]
	public string ID { get; set; } = string.Empty;
	public Guid? CurrentGame { get; set; }
	public int Wins { get; set; }
}
