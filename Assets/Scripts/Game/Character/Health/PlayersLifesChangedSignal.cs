using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Character.Health {
    public class PlayersLifesChangedSignal {
        public byte PlayerId;

        public PlayersLifesChangedSignal(byte playerId) {
            this.PlayerId = playerId;
        }
    }
}
