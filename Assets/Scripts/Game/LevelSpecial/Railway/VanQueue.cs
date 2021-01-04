using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    [Serializable]
    public class VanQueue {
        public VanMoveController VanMoveController;
        public Vector2Int QueueCount;
        public int VanStartIndex = 0;
    }
}
