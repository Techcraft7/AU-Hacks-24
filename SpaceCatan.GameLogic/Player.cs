using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCatan.GameLogic {
    public class Player {

        private readonly int playerID;
        private string guID;
        public Player(int i) {
            playerID = i;
        }

        public int GetPlayerID() {
            return playerID;
        }

    }
}
