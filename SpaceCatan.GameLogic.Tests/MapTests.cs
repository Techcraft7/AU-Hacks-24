using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCatan.GameLogic.Tests
{
    public class MapTests
    {
        [Fact]
        public void InitializeMap_CheckNotUnknownPlanets()
        {
            var mapTest = new Map();
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
            var mapTest = new Map();
            PlanetKind checkedPlanetKind = mapTest.GetPlanet(2, 0).Kind;
            Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
        }
        [Fact]
        public void InitalizeMap_CheckLeftEdgeIsOutpost()
        {
            var mapTest = new Map();
            PlanetKind checkedPlanetKind = mapTest.GetPlanet(0, 2).Kind;
            Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
        }
        [Fact]
        public void InitalizeMap_CheckRightEdgeIsOutpost()
        {
            var mapTest = new Map();
            PlanetKind checkedPlanetKind = mapTest.GetPlanet(4, 2).Kind;
            Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
        }
        [Fact]
        public void InitalizeMap_CheckBottomEdgeIsOutpost()
        {
            var mapTest = new Map();
            PlanetKind checkedPlanetKind = mapTest.GetPlanet(2, 4).Kind;
            Assert.Equal(PlanetKind.OUTPOST, checkedPlanetKind);
        }

        [Fact]
        public void InitializeMap_CheckAllResourcesProperlyInitialized()
        {
            var mapTest = new Map();
            var poolOfResources = new List<PlanetKind>() {
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
    }
}
