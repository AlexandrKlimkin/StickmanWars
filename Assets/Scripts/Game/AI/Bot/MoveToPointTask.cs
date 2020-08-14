using Assets.Scripts.Tools;
using Game.AI.PathFinding;
using System.Collections.Generic;
using System.Linq;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class MoveToPointTask : UnitTask {
        [Dependency]
        protected readonly WayPointsMangager _WayPointsMangager;

        private MovementData _MovementData;

        private float _StopDistance;

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run() {
            FindNewPath();
            if (_MovementData.CurrentPointPath != null && _MovementData.CurrentPointPath.Count > 0) {
                var sqrDist = Vector2.SqrMagnitude(_MovementData.CurrentPointPath.Last().ToVector2() - CharacterUnit.transform.position.ToVector2());
                if (sqrDist > 25) {
                    ProcessMove();
                    return TaskStatus.Running;
                } else {
                    _MovementData.TargetPos = null;
                    _MovementData.DestinationType = DestinationType.None;
                    MovementController.SetHorizontal(0);
                    return TaskStatus.Success;
                }
            } else {
                MovementController.SetHorizontal(0);
                return TaskStatus.Success;
            }
        }

        private void FindNewPath() {
            _MovementData.CurrentPath = new List<WayPoint>();
            if (_MovementData.TargetPos != null) {
                var path = _WayPointsMangager.CalculateGraphPath(CharacterUnit.transform.position, _MovementData.TargetPos.Value);
                if (path != null) {
                    _MovementData.CurrentPath = path;

                    _MovementData.CurrentPointPath = _MovementData.CurrentPath.Select(_ => _.Position).ToList();
                    _MovementData.CurrentPointPath.Add(_MovementData.TargetPos.Value);

                    var pointPathCount = _MovementData.CurrentPointPath.Count;
                    if (pointPathCount > 1) {
                        var characterPos = CharacterUnit.transform.position.ToVector2();

                        var sqrDistToFirst = (characterPos - _MovementData.CurrentPointPath[0].ToVector2()).sqrMagnitude;
                        var sqrDistToSecond = (characterPos - _MovementData.CurrentPointPath[1].ToVector2()).sqrMagnitude;
                        var sqrDistFirstToSecond = (_MovementData.CurrentPointPath[0].ToVector2() - _MovementData.CurrentPointPath[1].ToVector2()).sqrMagnitude;
                        //if (sqrDistToSecond < sqrDistToFirst)
                        //    _MovementData.CurrentPointPath.RemoveAt(0);
                        if (sqrDistToSecond < sqrDistFirstToSecond) {
                            _MovementData.CurrentPointPath.RemoveAt(0);
                            pointPathCount = _MovementData.CurrentPointPath.Count;
                        }
                       if (pointPathCount > 1) {
                            var pos = _MovementData.CurrentPointPath.Count == 2 ? characterPos : _MovementData.CurrentPointPath[pointPathCount - 3].ToVector2();

                            var sqrDistToLast = Vector2.SqrMagnitude(_MovementData.CurrentPointPath.Last().ToVector2() - pos);
                            var sqrDistLastPreLast = Vector2.SqrMagnitude(_MovementData.CurrentPointPath[pointPathCount - 2].ToVector2() - pos);
                            if (sqrDistToLast < sqrDistLastPreLast) {
                                _MovementData.CurrentPointPath.RemoveAt(pointPathCount - 2);
                                pointPathCount = _MovementData.CurrentPointPath.Count;
                            }
                        }
                    }
                }
                
            }
        }
        

        private void ProcessMove() {
            if (_MovementData.CurrentPointPath.Count == 0)
                return;

            var firstWayPoint = _MovementData.CurrentPath[0];
            var firstPoint = _MovementData.CurrentPointPath[0];
            var firstPointVector = firstPoint - CharacterUnit.transform.position;
            var horDistToFirstPoint = Mathf.Abs(firstPointVector.x);

            var targetVector = CharacterUnit.Target.position - CharacterUnit.transform.position;
            var horDistToTarget = Mathf.Abs(targetVector.x);

            var dist = firstPointVector.magnitude;
            float horizontal = MovementController.Horizontal;
            //if (_MovementData.CurrentPath.Count == 1)
            //    horizontal = 0;
            if (horDistToFirstPoint > 5) {
                horizontal = firstPointVector.x > 0 ? 1f : -1f;
            } else {
                if(_MovementData.CurrentPointPath.Count > 1) {
                    var secondndPoint = _MovementData.CurrentPointPath[0];
                    var targetVector2 = secondndPoint - CharacterUnit.transform.position;
                }
                if (_MovementData.CurrentPointPath.Count < 2 && horDistToTarget < 1f)
                    horizontal = 0;
            }
            MovementController.SetHorizontal(horizontal);
            var closest = _WayPointsMangager.GetNearestWaypoint(CharacterUnit.Position);
            var vertDist = firstPointVector.y;

            if(_MovementData.CurrentPointPath.Count > 1) {
                if(_MovementData.CurrentPath[0].Links.Any(_=>_.IsJumpLink)) {
                    if (!MovementController.HighJump())
                        MovementController.WallJump();
                }
            }
        }
    }

    public enum DestinationType {
        None, Weapon, ShootPosition, Escape, Random
    }
}
