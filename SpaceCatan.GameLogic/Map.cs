using System.Collections;

namespace SpaceCatan.GameLogic;

public sealed class Map
{
    private readonly Planet[,] planets = new Planet[5, 5];
    private readonly Roads[,] roads = new Roads[5, 5];

    public Map()
    {
        var rand = new Random();
        // center of each edge is guaranteed outpost
        planets[0, 2] = new Planet(PlanetKind.OUTPOST, 0);
        planets[2, 0] = new Planet(PlanetKind.OUTPOST, 0);
        planets[2, 4] = new Planet(PlanetKind.OUTPOST, 0);
        planets[4, 2] = new Planet(PlanetKind.OUTPOST, 0);
        // 4 of each resource, 1 empty
        var poolOfResources = new List<PlanetKind>() {
            PlanetKind.FOOD,PlanetKind.FOOD,PlanetKind.FOOD,PlanetKind.FOOD,
            PlanetKind.WATER,PlanetKind.WATER,PlanetKind.WATER,PlanetKind.WATER,
            PlanetKind.OXYGEN,PlanetKind.OXYGEN,PlanetKind.OXYGEN,PlanetKind.OXYGEN,
            PlanetKind.COBALT,PlanetKind.COBALT,PlanetKind.COBALT,PlanetKind.COBALT,
            PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,
            PlanetKind.EMPTY
        };
        
        // randomly generate board layout
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (GetPlanet(x, y).Kind != PlanetKind.OUTPOST)
                {
                    int next = rand.Next(poolOfResources.Count);
                    planets[x, y] = new Planet(poolOfResources.ElementAt(next), 0);
                    poolOfResources.RemoveAt(next);
                }
            }   
        }

        // initialize roads to unowned, null roads go out of bounds
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++) 
            {
                roads[x, y].Left = 0;
                roads[x, y].Right = 0;
                roads[x, y].Up = 0;
                roads[x, y].Down = 0;
                if (x <= 0) {
                    roads[x, y].Left = null;
                }
                if (x >= 4) {
                    roads[x, y].Right = null;
                }
                if (y <= 0) {
                    roads[x, y].Up = null;
                }
                if (y >= 4) {
                    roads[x, y].Down = null;
                }
            }
        }

    }

    public Planet GetPlanet(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5)
        {
            return default;
        }
        return planets[x, y];
    }

    public void SetPlanetOwner(int x, int y, int player)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5)
        {
            return;
        }
        planets[x, y].Owner = player;
    }

    public Roads GetRoads(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5)
        {
            return default;
        }
        return roads[x, y];
    }

    public int? GetRoad(int x, int y, Direction direction)
    {
        Roads r = GetRoads(x, y);
        return direction switch
        {
            Direction.UP => r.Up,
            Direction.DOWN => r.Down,
            Direction.LEFT => r.Left,
            Direction.RIGHT => r.Right,
            _ => null,
        };
    }

    public void SetRoad(int x, int y, Direction dir, int player)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5 || player < 1)
        {
            return;
        }
        switch (dir)
        {
            case Direction.UP:
                roads[x, y].Up = player;
                if (y > 0)
                {
                    roads[x, y - 1].Down = player;
                }
                break;
            case Direction.DOWN:
                roads[x, y].Down = player;
                if (y < 4)
                {
                    roads[x, y + 1].Up = player;
                }
                break;
            case Direction.LEFT:
                roads[x, y].Left = player;
                if (x > 0)
                {
                    roads[x - 1, y].Right = player;
                }
                break;
            case Direction.RIGHT:
                roads[x, y].Right = player;
                if (x < 4)
                {
                    roads[x + 1, y].Left = player;
                }
                break;
        }
    }
}
