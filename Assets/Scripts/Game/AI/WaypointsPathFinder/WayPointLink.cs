﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.PathFinding {
    [Serializable]
    public class WayPointLink {
        public WayPoint Neighbour;
        public float Cost;
        public bool IsJumpLink;

        public WayPointLink(WayPoint neighbour) {
            this.Neighbour = neighbour;
        }
    }
}
