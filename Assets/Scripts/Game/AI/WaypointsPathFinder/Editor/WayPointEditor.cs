using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Game.AI.PathFinding {
    [CustomEditor(typeof(WayPoint))]
    [CanEditMultipleObjects]

    public class WayPointEditor : Editor {

        private SerializedObject _Waypoint;
        private SerializedProperty _Links;
        private WayPoint _ThisWaypoint;

        public void OnEnable() {
            _Waypoint = new SerializedObject(target);
            _Links = _Waypoint.FindProperty("Links");
            _ThisWaypoint = (WayPoint)target;
        }

        public override void OnInspectorGUI() {
            if (targets.Length == 1) {
                //SHOW LINKS
                EditorGUILayout.PropertyField(_Links);

                //BUTTON TO ADD NEW NEIGHBOUR
                AddNeighbour("<- -> neighbour", true, true);
                AddNeighbour("-> neighbour", true, false);
                AddNeighbour("<- neighbour", false, true);
                AddNeighbour("no link neighbour", false, false);
            }

            //BUTTON TO LINK 2 NEIGHBOURS
            if (targets.Length == 2) {
                Link2Selected("<- -> link selected", true, true);
                Link2Selected("-> link selected", true, false);
                Link2Selected("<- link selected", false, true);
            }
            _Waypoint.ApplyModifiedProperties();
        }

        private void AddNeighbour(string name, bool hasThereLink, bool hasBackLink) {
            if (GUILayout.Button(name)) {
                GameObject newObj = Instantiate(_ThisWaypoint.gameObject, _ThisWaypoint.transform.parent) as GameObject;
                if (newObj) {
                    for (int i = 0; ; i++) {
                        if (GameObject.Find(WayPointsMangager.WaypointPrefix + i.ToString()) == null) {
                            newObj.name = WayPointsMangager.WaypointPrefix + i.ToString();
                            Undo.RegisterCreatedObjectUndo(newObj, newObj.name);
                            break;
                        }
                    }
                    WayPoint newWaypoint = newObj.GetComponent<WayPoint>();
                    newWaypoint.Links.Clear();
                    Selection.activeTransform = newWaypoint.transform;
                    if (hasThereLink) {
                        _ThisWaypoint.Manager.AddLink(_ThisWaypoint, newWaypoint, false);
                    }
                    if (hasBackLink) {
                        _ThisWaypoint.Manager.AddLink(newWaypoint, _ThisWaypoint, false);
                    }
                }
            }
        }

        private void Link2Selected(string buttonName, bool hasThereLink, bool hasBackLink) {
            if (GUILayout.Button(buttonName)) {
                var first = (WayPoint)targets[0];
                var second = (WayPoint)targets[1];
                if (hasThereLink) {
                    first.Manager.AddLink(second, first, false);
                }
                if (hasBackLink) {
                    first.Manager.AddLink(first, second, false);
                }
            }
        }
    }
}
