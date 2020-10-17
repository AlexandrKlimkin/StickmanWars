using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Core.Services.Game {
    class RespawnModeConfig : SingletonScriptableObject<RespawnModeConfig> {
        public bool UseBots;
        public int MaxBotsCount = 4;

        public void SetBotsCount(int value) {
            value = Mathf.Clamp(value, 0, GameConstants.MaxPLayersCount);
            SetBotsCount(value);
        }
    }
}
