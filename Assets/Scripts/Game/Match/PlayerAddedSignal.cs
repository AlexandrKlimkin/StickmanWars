using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Match {
    public class PlayerAddedSignal {
        public PlayerData PlayerData;
        public bool SpawnOnMap;

        public PlayerAddedSignal(PlayerData playerData, bool spawnOnMap = true) {
            this.PlayerData = playerData;
            this.SpawnOnMap = spawnOnMap;
        }
    }
}
