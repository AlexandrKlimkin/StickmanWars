using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.PathFinding {
    [ExecuteInEditMode]
    public class WayPoint : MonoBehaviour {

        public Vector3 Position => transform.position;
        public List<WayPointLink> Links;

        private WayPointsMangager _Mangager;
        private WayPointsMangager Manager => _Mangager ?? GetComponentInParent<WayPointsMangager>();

        private void OnEnable() {
            Manager.RegisterWayPoint(this);
        }

        private void OnDisable() {
            Manager.UnRegisterWayPoint(this);
        }

        private void OnDestroy() {
            Manager.UnRegisterWayPoint(this);
        }

    }
}
