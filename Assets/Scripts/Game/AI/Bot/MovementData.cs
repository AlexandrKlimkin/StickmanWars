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
        private DestinationType _DestinationType;

        public DestinationType DestinationType {
            get =>
              this._DestinationType;
            set {
                if (value != _DestinationType) {
                    this._DestinationType = value;
                    //Debug.LogError(_DestinationType);
                }
            }
        }
    }
}