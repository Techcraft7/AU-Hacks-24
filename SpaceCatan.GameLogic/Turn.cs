namespace SpaceCatan.GameLogic;

public sealed record class Turn(RoadToBuild[] Roads, ColonyToBuild[] Colonies, int DevelopmentCardsPurchased, Dictionary<Resource, int> ChangeInResources);

public readonly record struct RoadToBuild(int X, int Y, Direction Direction);
public readonly record struct ColonyToBuild(int X, int Y);