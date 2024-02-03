using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCatan.GameLogic {
    public class GameManager {

        Map map;
        Player[] players;

        public GameManager() {
            map = new Map();
            // TODO: change map initialization process
            players = new Player[4];

            players[0] = new Player(1);
            players[1] = new Player(2);
            players[2] = new Player(3);
            players[3] = new Player(4);

        }

        public Map GetMap() {
            return map;
        }

        public Player GetPlayerByID(int id) {
            foreach (Player p in players) {
                if (p.GetPlayerID() == id) return p;
            }

            return null; // prob should change later
        }

        public List<Planet> GetPlayerOwnedPlanets(int id) {
            List<Planet> planets = new();
            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    if (map.GetPlanet(x, y).Owner == id) {
                        planets.Add(map.GetPlanet(x, y));
                    }
                }
            }
            return planets;
        }

        public List<Roads> GetPlayerOwnedRoads(int id) {
            List<Roads> roads = new();


            return roads;
        }
    }
}
