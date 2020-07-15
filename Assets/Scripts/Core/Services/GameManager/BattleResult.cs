using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Game {
    public class BattleResult {
        public BattleEndReason BattleEndReason;
        public byte WinnerPlayerId;
    }

    public enum BattleEndReason {
        OnePlayerLeft,
        Time,
    }
}