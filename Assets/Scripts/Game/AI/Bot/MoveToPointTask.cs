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
            //if(MovementController.IsGrounded)
                FindNewPath();
            if (_MovementData.CurrentPointPath != null && _MovementData.CurrentPointPath.Count > 0 && _MovementData.TargetPos != null) {
                var sqrDistToTarget = Vector2.SqrMagnitude(_MovementData.TargetPos.Value.ToVector2() - CharacterUnit.transform.position.ToVector2());
                if (sqrDistToTarget > 25) {
                    ProcessMove();
                    ProcessJump();
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
            var firstPoint = _MovementData.CurrentPointPath[0];
            var firstPointVector = firstPoint - CharacterUnit.transform.position;
            var horDistToFirstPoint = Mathf.Abs(firstPointVector.x);

            var targetVector = _MovementData.TargetPos.Value.ToVector2() - CharacterUnit.transform.position.ToVector2();
            var horDistToTarget = Mathf.Abs(targetVector.x);

            float horizontal = MovementController.Horizontal;

            if (Mathf.Abs(targetVector.x) < 2 && _MovementData.CurrentPointPath.Count < 2) {
                horizontal = 0;
            } else {
                horizontal = firstPointVector.x > 0 ? 1f : -1f;
            }
            MovementController.SetHorizontal(horizontal);
        }

        private float _DelayBetweenJumps = 0.3f;
        private float _LastJumpTime = float.NegativeInfinity;

        private void ProcessJump() {
            var firstWayPoint = _MovementData.CurrentPath[0];
            var timeFromLastJumpLeft = Time.time - _LastJumpTime;
            var pathCount = _MovementData.CurrentPath.Count;
            if (pathCount > 1) {
                var secondWayPoint = _MovementData.CurrentPath[1];
                var linkFirstToSecond = firstWayPoint.Links.FirstOrDefault(_ => _.Neighbour == secondWayPoint);
                if (linkFirstToSecond != null && (linkFirstToSecond.IsJumpLink || MovementController.LedgeHang) && timeFromLastJumpLeft >= _DelayBetweenJumps) {
                    Jump();
                }
            } else if (pathCount > 0) {
                if (MovementController.LedgeHang)
                    Jump();
            }
        }

        private void Jump() {
            if (MovementController.HighJump()) {
                _LastJumpTime = Time.time;
            } else {
                if (MovementController.WallJump()) {

                }
            }
        }
    }

    public enum DestinationType {
        None, Weapon, ShootPosition, Escape, Random
    }
}
