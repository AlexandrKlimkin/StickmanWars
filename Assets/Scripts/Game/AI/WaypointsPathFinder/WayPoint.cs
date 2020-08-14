using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.PathFinding {
    [ExecuteInEditMode]
    public class WayPoint : MonoBehaviour {

        public Vector3 Position => transform.position;
        public List<WayPointLink> Links = new List<WayPointLink>();

        private WayPointsMangager _Mangager;
        public WayPointsMangager Manager => _Mangager ?? GetComponentInParent<WayPointsMangager>();

        private void OnEnable() {
            Manager.RegisterWayPoint(this);
        }

        //private void OnDisable() {
        //    if(Manager != null)
        //        Manager.UnRegisterWayPoint(this);
        //}

        private void OnDestroy() {
            if (Manager != null)
                Manager.UnRegisterWayPoint(this);
        }

    }
}
