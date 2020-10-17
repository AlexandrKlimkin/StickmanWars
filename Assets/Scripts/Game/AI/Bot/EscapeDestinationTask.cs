using Assets.Scripts.Tools;
using Game.AI.PathFinding;
using Game.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class EscapeDestinationTask : UnitTask {

        [Dependency]
        private readonly WayPointsMangager _WayPointsMangager;
        [Dependency]
        private MatchService _MatchService;

        private MovementData _MovementData;

        private float _EscapeTime;
        private float _EscapeStartTime;
        private Vector2 _EscapeTimeVector;
        private bool _Escaping;

        private CharacterUnit _EscapedUnit;

        public EscapeDestinationTask(Vector2 EscapeTimeVector) {
            this._EscapeTimeVector = EscapeTimeVector;
        }

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run() {
            if (_Escaping) {
                if (_EscapeStartTime + _EscapeTime < Time.time) {
                    _MovementData.TargetPos = null;
                    _EscapedUnit = null;
                    _Escaping = false;
                    return TaskStatus.Success;
                }
                if (!_EscapedUnit)
                    return TaskStatus.Failure;
                if(_MovementData.TargetPos == null) {
                    GetEscapePoint();
                }
                return TaskStatus.Running;
            } else {
                var topDmgr = CharacterUnit.DamageBuffer.TopBufferedDamager();
                if (topDmgr == null)
                    return TaskStatus.Failure;
                _EscapedUnit = CharacterUnit.Characters.FirstOrDefault(_ => _.OwnerId == topDmgr);
                if (!_EscapedUnit)
                    return TaskStatus.Failure;
                _EscapeStartTime = Time.time;
                _Escaping = true;
                _EscapeTime = UnityEngine.Random.Range(_EscapeTimeVector.x, _EscapeTimeVector.y);
                GetEscapePoint();
                return TaskStatus.Running;
            }
        }

        private void GetEscapePoint() {
            var escapedUnitPos = _EscapedUnit.transform.position;
            var unitPos = CharacterUnit.transform.position;
            var vector = unitPos - escapedUnitPos;
            var normVector = vector.normalized;
            var closestPointsList = new List<WayPoint>();
            var closestPointsDistances = new List<float>();
            var closestWayPoint = _WayPointsMangager.GetNearestWaypoint(unitPos + normVector * 150f);
            var dist = Vector2.Distance(closestWayPoint.Position, escapedUnitPos);
            closestPointsList.Add(closestWayPoint);
            closestPointsDistances.Add(dist);
            if (dist < 50f) {
                var dir = UnityEngine.Random.value == 0 ? -1f : 1f;
                closestWayPoint = _WayPointsMangager.GetNearestWaypoint(unitPos + Vector3.up * dir * 150f);
                dist = Vector2.Distance(closestWayPoint.Position, escapedUnitPos);
                closestPointsList.Add(closestWayPoint);
                closestPointsDistances.Add(dist);
                if (dist < 50f) {
                    closestWayPoint = _WayPointsMangager.GetNearestWaypoint(unitPos + Vector3.up * -dir * 150f);
                    dist = Vector2.Distance(closestWayPoint.Position, escapedUnitPos);
                    closestPointsList.Add(closestWayPoint);
                    closestPointsDistances.Add(dist);
                    if (dist < 50f) {
                        closestWayPoint = _WayPointsMangager.GetNearestWaypoint(escapedUnitPos - normVector * 150f);
                        dist = Vector2.Distance(closestWayPoint.Position, escapedUnitPos);
                        closestPointsList.Add(closestWayPoint);
                        closestPointsDistances.Add(dist);
                    }
                }
            }
            var closestDist = float.MaxValue;
            for(var i = 0; i < closestPointsList.Count; i++) {
                var wayPoint = closestPointsList[i];
                dist = closestPointsDistances[i];
                if (closestDist > dist) {
                    closestDist = dist;
                    closestWayPoint = wayPoint;
                }
            }
            //if (closestPointsList.Count > 1)
            //    Debug.LogError(closestPointsList.Count);
            _MovementData.TargetPos = closestWayPoint.Position;
            _MovementData.DestinationType = DestinationType.Escape;
        }
    }
}