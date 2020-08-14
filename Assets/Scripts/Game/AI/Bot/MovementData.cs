using Game.AI.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.AI {
    public class MovementData : BlackboardData {
        public List<WayPoint> CurrentPath;
        public List<Vector3> CurrentPointPath;
        public Vector3? TargetPos;
        public DestinationType DestinationType;
    }
}
