namespace SpaceCatan.GameLogic;

public sealed record class Turn(RoadToBuild[] Roads, ColonyToBuild[] Colonies, Dictionary<Resource, int> ResourcesTraded, int DevelopmentCardsBought, bool DevelopmentCardUsed);

public readonly record struct RoadToBuild(int X, int Y, Direction Direction)
{
	public RoadToBuild Opposite() => Direction switch
	{
		Direction.UP => new(X, Y - 1, Direction.DOWN),
		Direction.DOWN => new(X, Y + 1, Direction.UP),
		Direction.LEFT => new(X - 1, Y, Direction.RIGHT),
		Direction.RIGHT => new(X + 1, Y, Direction.LEFT),
		_ => this
	};
}
public readonly record struct ColonyToBuild(int X, int Y);