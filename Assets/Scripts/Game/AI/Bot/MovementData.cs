using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.AI {
    public class MovementData : BlackboardData {
        public List<w2dp_Waypoint> CurrentPath;
        public Vector3? TargetPos;
        public DestinationType DestinationType;
    }
}
