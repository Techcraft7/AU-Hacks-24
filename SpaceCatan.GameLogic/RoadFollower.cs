using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SpaceCatan.GameLogic;

public static class RoadFollower
{
	public static void RemoveDisjointRoads(this Map map, int player, IList<RoadToBuild> list)
	{
		HashSet<(int, int)> visited = [];
		ReadOnlySpan<Direction> allDirs = [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT];
		Map copy = new();
		foreach (var (x, y, d) in list)
		{
			copy.SetRoad(x, y, d, player);
		}
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				Planet p = map.GetPlanet(x, y);
				if (p.Owner != player)
				{
					continue;
				}

				copy.SetPlanetKind(x, y, p.Kind);
				copy.SetPlanetOwner(x, y, p.Owner);

				foreach (Direction i in allDirs)
				{
					if (map.GetRoad(x, y, i) == player)
					{
						copy.SetRoad(x, y, i, player);
					}
				}

				Visit(copy, x, y, player, visited);
			}
		}

		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (!visited.Contains((list[i].X, list[i].Y)))
			{
				list.RemoveAt(i);
			}
		}
	}

	private static void Visit(Map m, int x, int y, int player, HashSet<(int, int)> visited)
	{
		if (visited.Contains((x, y)))
		{
			return;
		}
		Debug.WriteLine($"visiting {x}, {y}");
		visited.Add((x, y));
		ReadOnlySpan<Direction> allDirs = [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT];
		foreach (Direction i in allDirs)
		{
			if (m.GetRoad(x, y, i) != player)
			{
				continue;
			}
			Debug.WriteLine($"found dir {i}");
			(int nextX, int nextY) = i switch
			{
				Direction.UP => (x, y - 1),
				Direction.DOWN => (x, y + 1),
				Direction.LEFT => (x - 1, y),
				Direction.RIGHT => (x + 1, y),
				_ => throw new NotImplementedException()
			};
			Visit(m, nextX, nextY, player, visited);
		}
	}
}
