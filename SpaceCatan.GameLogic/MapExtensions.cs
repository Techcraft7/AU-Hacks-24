namespace SpaceCatan.GameLogic;

public static class MapExtensions
{
    public static bool HasOutpost(this Map m, int player)
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Planet p = m.GetPlanet(x, y);
                if (p.Kind == PlanetKind.OUTPOST && p.Owner == player)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
