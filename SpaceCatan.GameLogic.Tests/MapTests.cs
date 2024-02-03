namespace SpaceCatan.GameLogic.Tests;

public class MapTests
{
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
