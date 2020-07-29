using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Core.Services.Game {
    class RespawnModeConfig : SingletonScriptableObject<RespawnModeConfig> {
        public bool UseBots;
        public int MaxBotsCount = 4;


    }
}
