using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.PathFinding {
    [Serializable]
    public class WayPointLink {
        public WayPoint Neighbour;
        public float Cost = 1f;
        public bool IsJumpLink;
        public bool IsLowJumpLink;

        public WayPointLink(WayPoint neighbour) {
            this.Neighbour = neighbour;
        }
    }
}
