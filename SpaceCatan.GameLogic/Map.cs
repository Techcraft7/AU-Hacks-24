namespace SpaceCatan.GameLogic;

public sealed class Map
{
    private readonly Planet[,] planets = new Planet[5, 5];
    private readonly Roads[,] roads = new Roads[5, 5];

    public Map()
    {
        // TODO: fill planets array (place outposts on edge)s
        // TODO: init roads to 0
        // TODO: set out of bounds roads to null
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
