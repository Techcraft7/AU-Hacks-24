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
}
