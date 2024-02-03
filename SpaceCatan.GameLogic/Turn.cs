namespace SpaceCatan.GameLogic;

public sealed record class Turn(RoadToBuild[] Roads, ColonyToBuild[] Colonies, Dictionary<Resource, int> ResourcesTraded, int DevelopmentCardsBought, int DevelopmentCardsUsed);

public readonly record struct RoadToBuild(int X, int Y, Direction Direction);
public readonly record struct ColonyToBuild(int X, int Y);