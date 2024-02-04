namespace SpaceCatan.GameLogic.Tests;

public sealed class MapTests
{
    [Theory]
    // Top side
    [InlineData(0, 0, Direction.UP)]
    [InlineData(1, 0, Direction.UP)]
    [InlineData(2, 0, Direction.UP)]
    [InlineData(3, 0, Direction.UP)]
    [InlineData(4, 0, Direction.UP)]
    // Right side
    [InlineData(4, 0, Direction.RIGHT)]
    [InlineData(4, 1, Direction.RIGHT)]
    [InlineData(4, 2, Direction.RIGHT)]
    [InlineData(4, 3, Direction.RIGHT)]
    [InlineData(4, 4, Direction.RIGHT)]
    // Bottom side
    [InlineData(4, 4, Direction.DOWN)]
    [InlineData(3, 4, Direction.DOWN)]
    [InlineData(2, 4, Direction.DOWN)]
    [InlineData(1, 4, Direction.DOWN)]
    [InlineData(0, 4, Direction.DOWN)]
    // Left side
    [InlineData(0, 4, Direction.LEFT)]
    [InlineData(0, 3, Direction.LEFT)]
    [InlineData(0, 2, Direction.LEFT)]
    [InlineData(0, 1, Direction.LEFT)]
    [InlineData(0, 0, Direction.LEFT)]
    public void GetRoads_Edges_ReturnsNull(int x, int y, Direction dir)
    {
        Map map = new();

        int? road = map.GetRoad(x, y, dir);

        Assert.Null(road);
    }

    [Theory]
    // Top side
    [InlineData(0, 0, Direction.UP)]
    [InlineData(1, 0, Direction.UP)]
    [InlineData(2, 0, Direction.UP)]
    [InlineData(3, 0, Direction.UP)]
    [InlineData(4, 0, Direction.UP)]
    // Right side
    [InlineData(4, 0, Direction.RIGHT)]
    [InlineData(4, 1, Direction.RIGHT)]
    [InlineData(4, 2, Direction.RIGHT)]
    [InlineData(4, 3, Direction.RIGHT)]
    [InlineData(4, 4, Direction.RIGHT)]
    // Bottom side
    [InlineData(4, 4, Direction.DOWN)]
    [InlineData(3, 4, Direction.DOWN)]
    [InlineData(2, 4, Direction.DOWN)]
    [InlineData(1, 4, Direction.DOWN)]
    [InlineData(0, 4, Direction.DOWN)]
    // Left side
    [InlineData(0, 4, Direction.LEFT)]
    [InlineData(0, 3, Direction.LEFT)]
    [InlineData(0, 2, Direction.LEFT)]
    [InlineData(0, 1, Direction.LEFT)]
    [InlineData(0, 0, Direction.LEFT)]
    public void SetRoads_Edges_DoesNotThrow(int x, int y, Direction dir)
    {
        Map map = new();

        map.SetRoad(x, y, dir, 1234);
    }
  
    [Fact]
    public void InitializeMap_CheckNotUnknownPlanets()
    {
        Map mapTest = new();
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                PlanetKind checkedPlanetKind = mapTest.GetPlanet(x, y).Kind;
                Assert.NotEqual(PlanetKind.UNKNOWN, checkedPlanetKind);
            }
        }
    }

    [Fact]
    public void InitalizeMap_CheckTopEdgeIsOutpost()
    {
        Map mapTest = new();
        PlanetKind checkedPlanetKind = mapTest.GetPlanet(2, 0).Kind;
        Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
    }

    [Fact]
    public void InitalizeMap_CheckLeftEdgeIsOutpost()
    {
        Map mapTest = new();
        PlanetKind checkedPlanetKind = mapTest.GetPlanet(0, 2).Kind;
        Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
    }

    [Fact]
    public void InitalizeMap_CheckRightEdgeIsOutpost()
    {
        Map mapTest = new();
        PlanetKind checkedPlanetKind = mapTest.GetPlanet(4, 2).Kind;
        Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
    }

    [Fact]
    public void InitalizeMap_CheckBottomEdgeIsOutpost()
    {
        Map mapTest = new();
        PlanetKind checkedPlanetKind = mapTest.GetPlanet(2, 4).Kind;
        Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
    }

    [Fact]
    public void InitializeMap_CheckAllResourcesProperlyInitialized()
    {
        Map mapTest = new();
        List<PlanetKind> poolOfResources = new() {
        PlanetKind.FOOD,PlanetKind.FOOD,PlanetKind.FOOD,PlanetKind.FOOD,
        PlanetKind.WATER,PlanetKind.WATER,PlanetKind.WATER,PlanetKind.WATER,
        PlanetKind.OXYGEN,PlanetKind.OXYGEN,PlanetKind.OXYGEN,PlanetKind.OXYGEN,
        PlanetKind.COBALT,PlanetKind.COBALT,PlanetKind.COBALT,PlanetKind.COBALT,
        PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,PlanetKind.GRAVITRONIUM,
        PlanetKind.EMPTY
        };
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                PlanetKind checkedPlanetKind = mapTest.GetPlanet(x, y).Kind;
                if (checkedPlanetKind != PlanetKind.OUTPOST)
                {
                    bool containsResource = poolOfResources.Contains(checkedPlanetKind);
                    Assert.True(containsResource);
                    poolOfResources.Remove(checkedPlanetKind);
                }
            }
        }
    }


    [Fact]
    public void InitializeMap_CheckIfTopEdgeRoadsHaveNulls()
    {
        Map mapTest = new();
        for (int x = 0; x < 5; x++)
        {
            int? dir = mapTest.GetRoad(x, 0, Direction.UP);
            Assert.Null(dir);
        }
    }

    [Fact]
    public void InitializeMap_CheckIfLeftEdgeRoadsHaveNulls()
    {
        Map mapTest = new();
        for (int y = 0; y < 5; y++)
        {
            int? dir = mapTest.GetRoad(0, y, Direction.LEFT);
            Assert.Null(dir);
        }
    }

    [Fact]
    public void InitializeMap_CheckIfRightEdgeRoadsHaveNulls()
    {
        Map mapTest = new();
        for (int y = 0; y < 5; y++)
        {
            int? dir = mapTest.GetRoad(4, y, Direction.RIGHT);
            Assert.Null(dir);
        }
    }

    [Fact]
    public void InitializeMap_CheckIfBottomEdgeRoadsHaveNulls()
    {
        Map mapTest = new();
        for (int x = 0; x < 5; x++)
        {
            int? dir = mapTest.GetRoad(x, 4, Direction.DOWN);
            Assert.Null(dir);
        }
    }

    [Fact]
    public void InitializeMap_CheckDownValidRoads()
    {
        Map mapTest = new();
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                Roads cell = mapTest.GetRoads(x, y);
                Assert.NotNull(cell.Down);
            }
        }
    }

    [Fact]
    public void InitializeMap_CheckLeftValidRoads()
    {
        Map mapTest = new();
        for (int y = 0; y < 5; y++)
        {
            for (int x = 1; x < 5; x++)
            {
                Roads cell = mapTest.GetRoads(x, y);
                Assert.NotNull(cell.Left);
            }
        }
    }

    [Fact]
    public void InitializeMap_CheckRightValidRoads()
    {
        Map mapTest = new();
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Roads cell = mapTest.GetRoads(x, y);
                Assert.NotNull(cell.Right);
            }
        }
    }

    [Fact]
    public void InitializeMap_CheckUpValidRoads()
    {
        Map mapTest = new();
        for (int y = 1; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                Roads cell = mapTest.GetRoads(x, y);
                Assert.NotNull(cell.Up);
            }
        }
    }
}
