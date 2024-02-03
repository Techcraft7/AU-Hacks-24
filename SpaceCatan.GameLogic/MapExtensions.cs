namespace SpaceCatan.GameLogic;

public static class MapExtensions
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

	public static bool CanBuildRoadAt(this Map map, int x, int y, Direction dir, int player)
	{
		if (x < 0 || y < 0 || x >= 5 || y >= 5)
		{
			return false;
		}

		Span<Direction> allDirs = [ Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT ];
		foreach (Direction i in allDirs)
		{
			if (i == dir)
			{
				continue;
			}
			if (map.GetRoad(x, y, i) == player)
			{
				return true;
			}

			(int dx, int dy) = i switch
			{
				Direction.UP => (0, -1),
				Direction.DOWN => (0, 1),
				Direction.LEFT => (-1, 0),
				Direction.RIGHT => (1, 0),
				_ => (0, 0)
			};

			if (map.GetPlanet(x + dx, y + dy).Owner == player)
			{
				return true;
			}
		}
		return false;
	}
}
