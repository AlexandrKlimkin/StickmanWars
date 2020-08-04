using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.AI.PathFinding {
    public class WayPointsMangager : MonoBehaviour {

        public Color WayPointsColor;
        public Color WayPointsSelectedColor;
        public Color UsualLinkColor;
        public Color JumpLinkColor;
        public float WayPointRadius;
        public bool ShowCostText;
        public int CostTextSize;
        public List<WayPoint> WayPoints;

        public const string WaypointPrefix = "Waypoint";

        public void RegisterWayPoint(WayPoint waypoint) {
            if(WayPoints == null) {
                WayPoints = new List<WayPoint>();
            }
            if (WayPoints.Contains(waypoint)) {
                Debug.LogError($"Way point {waypoint.gameObject.name} has been already registered.");
                return;
            }
            WayPoints.Add(waypoint);
        }

        public void UnRegisterWayPoint(WayPoint waypoint) {
            if (WayPoints != null && WayPoints.Contains(waypoint)) {
                WayPoints.Remove(waypoint);
            }
        }

        public void AddLink(WayPoint firstPoint, WayPoint secondPoint, bool twoSided) {
            if(firstPoint.Links.FirstOrDefault(_=>_.Neighbour == secondPoint) == null)
                firstPoint.Links.Add(new WayPointLink(secondPoint));
            if(twoSided)
                if(secondPoint.Links.FirstOrDefault(_ => _.Neighbour == firstPoint) == null)
                    secondPoint.Links.Add(new WayPointLink(firstPoint));
        }

        public void RemoveAllLinks(WayPoint waypoint) {
            WayPoints.ForEach(_ => {
                var links = _.Links.Where(link => link.Neighbour == waypoint).ToList();
                if(links != null && links.Count > 0) {
                    links.ForEach(link => _.Links.Remove(link));
                }
            });
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!WayPoints.IsNullOrEmpty()) {
                foreach (var point in WayPoints) {
                    Gizmos.color = Selection.Contains(point.gameObject) ? WayPointsSelectedColor : WayPointsColor;
                    Gizmos.DrawWireSphere(point.Position, WayPointRadius);
                    if (point.Links == null)
                        continue;
                    foreach (var link in point.Links) {
                        if (link.Neighbour != null) {
                            Handles.color = link.IsJumpLink ? JumpLinkColor : UsualLinkColor;
                            Handles.DrawLine(point.Position, link.Neighbour.Position);
                            var middle = (point.Position + link.Neighbour.Position) / 2f;
                            var dir = (link.Neighbour.Position - point.Position).normalized;
                            var arrowBasePoint = middle + dir * 1f;
                            var upArrowDir = Quaternion.Euler(0, 0, 90f) * dir;
                            var upArrowPoint = arrowBasePoint + upArrowDir * 1f;
                            var downArrowDir = Quaternion.Euler(0, 0, -90f) * dir;
                            var downArrowPoint = arrowBasePoint + downArrowDir * 1f;
                            var arrowCornerPoint = middle + dir * 3f;
                            Handles.DrawAAConvexPolygon(upArrowPoint, downArrowPoint, arrowCornerPoint);
                            if (ShowCostText) {
                                var guiStyle = new GUIStyle() { fontSize = CostTextSize, fontStyle = FontStyle.Bold };
                                guiStyle.normal.textColor = Handles.color;
                                Handles.Label(arrowCornerPoint + upArrowDir, link.Cost.ToString(), guiStyle);
                            }
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected() {
            
        }
#endif
    }
}
