namespace SpaceCatan.GameLogic;

public static class CanBuildMapExtensions
{
	public static bool CanBuildColonyAt(this Map map, int x, int y, int player)
	{
		if (x < 0 || y < 0 || x >= 5 || y >= 5)
		{
			return false;
		}
		if (map.GetPlanet(x, y).Owner != 0)
		{
			return false;
		}
		Roads roads = map.GetRoads(x, y);
		return roads.Up == player || roads.Down == player || roads.Left == player || roads.Right == player;
	}

	public static bool CanBuildRoadAt(this Map map, int x, int y, Direction dir, int player, IReadOnlyList<RoadToBuild>? extraRoads = null)
	{
		if (x < 0 || y < 0 || x >= 5 || y >= 5)
		{
			return false;
		}

		if (map.GetRoad(x, y, dir) != 0)
		{
			return false;
		}

		Span<Direction> allDirs = [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT];
		foreach (Direction i in allDirs)
		{
			if (map.GetRoad(x, y, i) == player)
			{
				return true;
			}
			if (extraRoads is not null && extraRoads.Contains(new(x, y, i)))
			{
				return true;
			}
		}

		return map.GetPlanet(x, y).Owner == player;
	}
}
