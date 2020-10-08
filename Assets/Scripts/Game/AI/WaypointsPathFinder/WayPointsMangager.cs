using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityDI;
using UnityEditor;
using UnityEngine;

namespace Game.AI.PathFinding {
    public class WayPointsMangager : MonoBehaviour {

        public Color WayPointsColor;
        public Color WayPointsSelectedColor;
        public Color UsualLinkColor;
        public Color HighJumpLinkColor;
        public Color LowJumpLinkColor;
        public float WayPointRadius;
        public bool ShowCostText;
        public int CostTextSize;
        public List<WayPoint> WayPoints;

        public const string WaypointPrefix = "Waypoint";

        private void Awake() {
            ContainerHolder.Container.RegisterInstance(this);
        }

        public void RegisterWayPoint(WayPoint waypoint) {
            if(WayPoints == null) {
                WayPoints = new List<WayPoint>();
            }
            if (WayPoints.Contains(waypoint)) {
                //Debug.LogError($"Way point {waypoint.gameObject.name} has been already registered.");
                return;
            }
            WayPoints.Add(waypoint);
        }

        public void UnRegisterWayPoint(WayPoint waypoint) {
            if (WayPoints != null && WayPoints.Contains(waypoint)) {
                RemoveAllLinks(waypoint);
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

        public WayPoint GetNearestWaypoint(Vector3 targetPosition) {
            WayPoint nearestWaypoint = null;
            float nearestSqrDistance = float.PositiveInfinity;
            for (int i = 0; i < WayPoints.Count; i++) {
                if (WayPoints[i] != null) {
                    float sqrDistance = Vector3.SqrMagnitude(WayPoints[i].Position - targetPosition);
                    if (sqrDistance < nearestSqrDistance) {
                        nearestWaypoint = WayPoints[i];
                        nearestSqrDistance = sqrDistance;
                    }
                }
            }
            return nearestWaypoint;
        }

        private class DijkstraData {
            public float Cost;
            public WayPoint Previous;
        }

        public List<WayPoint> CalculateGraphPath(WayPoint start, WayPoint end) {
            var unvisitedPoints = WayPoints.ToList();
            var track = new Dictionary<WayPoint, DijkstraData>();
            track[start] = new DijkstraData { Previous = null, Cost = 0 };

            while (true) {
                WayPoint toOpen = null;
                float bestPrice = float.PositiveInfinity;
                foreach(var v in unvisitedPoints) {
                    if(track.ContainsKey(v) && track[v].Cost < bestPrice) {
                        toOpen = v;
                        bestPrice = track[v].Cost;
                    }
                }
                if (toOpen == null)
                    return null;
                if (toOpen == end)
                    break;
                foreach(var link in toOpen.Links) {
                    var currentPrice = track[toOpen].Cost + link.Cost;
                    if(!track.ContainsKey(link.Neighbour) || track[link.Neighbour].Cost > currentPrice) {
                        track[link.Neighbour] = new DijkstraData { Cost = currentPrice, Previous = toOpen };
                    }
                }
                unvisitedPoints.Remove(toOpen);
            }
            var path = new List<WayPoint>();
            while(end != null) {
                path.Add(end);
                end = track[end].Previous;
            }
            path.Reverse();
            return path;
        }

        public List<WayPoint> CalculateGraphPath(Vector3 start, Vector3 end) {
            return CalculateGraphPath(GetNearestWaypoint(start), GetNearestWaypoint(end));
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
                            Handles.color = link.IsJumpLink ? HighJumpLinkColor : link.IsLowJumpLink ? LowJumpLinkColor : UsualLinkColor;
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
#endif

        private void OnDestroy() {
            ContainerHolder.Container.Unregister<WayPointsMangager>();
        }
    }
}
