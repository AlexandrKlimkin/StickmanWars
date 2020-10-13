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
            if (_MovementData.CurrentPointPath != null && _MovementData.CurrentPointPath.Count > 0 && _MovementData.TargetPos != null) {
                var sqrDistToTarget = Vector2.SqrMagnitude(_MovementData.TargetPos.Value.ToVector2() - CharacterUnit.transform.position.ToVector2());
                if (sqrDistToTarget > 25) {
                    var move = ProcessMove();
                    ProcessJump();
                    if (move)
                        return TaskStatus.Running;
                    else {
                        _MovementData.TargetPos = null;
                        _MovementData.DestinationType = DestinationType.None;
                        MovementController.SetHorizontal(0);
                        return TaskStatus.Success;
                    }
                } else {
                    _MovementData.TargetPos = null;
                    _MovementData.DestinationType = DestinationType.None;
                    MovementController.SetHorizontal(0);
                    return TaskStatus.Success;
                }
            } else {
                _MovementData.TargetPos = null;
                _MovementData.DestinationType = DestinationType.None;
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
                    if(Vector2.SqrMagnitude(_MovementData.TargetPos.Value.ToVector2() - _MovementData.CurrentPointPath.Last().ToVector2()) > 25)
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

        private bool ProcessMove() {
            var firstPoint = _MovementData.CurrentPointPath[0];

            //if (_MovementData.CurrentPointPath.Count > 1) {
            //    var charPoint = CharacterUnit.Position.ToVector2();
            //    var hit = JumpLinecast(charPoint, firstPoint);
            //    if(hit.collider != null && Vector2.Distance(hit.point, charPoint) < 20f) {
            //        firstPoint = _MovementData.CurrentPointPath[1];
            //    }
            //}

            var firstPointVector = firstPoint - CharacterUnit.transform.position;

            var horDistToFirstPoint = Mathf.Abs(firstPointVector.x);
            var targetVector = _MovementData.TargetPos.Value.ToVector2() - CharacterUnit.transform.position.ToVector2();
            var horDistToTarget = Mathf.Abs(targetVector.x);

            float horizontal = MovementController.Horizontal;


            if (Mathf.Abs(targetVector.x) < 2f && _MovementData.CurrentPointPath.Count < 2) {
                horizontal = 0;
            } else {
                horizontal = firstPointVector.x > 0 ? 1f : -1f;
            }

            MovementController.SetHorizontal(horizontal);
            return horizontal != 0;
        }

        private bool ObjectBetweenPoints(Vector2 point) {
            if (_MovementData.CurrentPointPath.Count <= 2) {
                var point1 = CharacterUnit.Position.ToVector2() + Vector2.up * 5f;
                var point2 = point;
                var hit = Physics2D.Linecast(point1, point2, Layers.Masks.Box);
                return hit.collider != null;
            }
            return false;
        }

        private float _DelayBetweenJumps = 0.3f;
        private float _LastJumpTime = float.NegativeInfinity;

        private void ProcessJump() {
            var timeFromLastJumpLeft = Time.time - _LastJumpTime;
            if (timeFromLastJumpLeft >= _DelayBetweenJumps) {
                if (!JumpOnLinks())
                    if (!JumpIfLedgeHang())
                        if (!JumpAroundObstacles()) { }
            }
        }

        private bool JumpOnLinks() {
            var firstWayPoint = _MovementData.CurrentPath[0];
            if (_MovementData.CurrentPath.Count > 1) {
                var secondWayPoint = _MovementData.CurrentPath[1];
                var linkFirstToSecond = firstWayPoint.Links.FirstOrDefault(_ => _.Neighbour == secondWayPoint);
                if (linkFirstToSecond != null) {
                    if (linkFirstToSecond.IsJumpLink) {
                        HighJump();
                        return true;
                    } else if (linkFirstToSecond.IsLowJumpLink) {
                        LowJump();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool JumpIfLedgeHang() {
            if (_MovementData.CurrentPath.Count > 0) {
                if (MovementController.LedgeHang) {
                    HighJump();
                    return true;
                }
            }
            return false;
        }

        private bool JumpAroundObstacles() {
            if (MovementController.Horizontal == 0)
                return false;
            if (_MovementData.CurrentPointPath == null)
                return false;
            if (_MovementData.CurrentPointPath.Count == 0)
                return false;
            var charPos = CharacterUnit.Position.ToVector2();
            var movePoint = _MovementData.CurrentPointPath[0].ToVector2();

            var pos1 = charPos + Vector2.up * 2f;
            var pos2 = charPos + Vector2.up * 10f;
            var pos3 = charPos + Vector2.up * 20f;
            var velocityVector = Vector2.right * MovementController.Horizontal * 25f;
            var hit1 = JumpLinecast(pos1, pos1 + velocityVector, Layers.Masks.Walkable);
            var hit2 = JumpLinecast(pos2, pos2 + velocityVector, Layers.Masks.Walkable);
            var hit3 = JumpLinecast(pos3, pos3 + velocityVector, Layers.Masks.Walkable);
            if (MovePointIsHigher(hit1) || MovePointIsHigher(hit2) || MovePointIsHigher(hit3)) {
                HighJump();
                return true;
            }

            var hit4 = JumpLinecast(pos1, movePoint, Layers.Masks.Box);
            if (hit1.collider != null && Layers.Masks.LayerInMask(Layers.Masks.Obstacle, hit1.collider.gameObject.layer)
                && hit4.collider != null && _MovementData.CurrentPointPath.Count > 1) {
                HighJump();
                return true;
            }
            return false;
        }

        private RaycastHit2D JumpLinecast(Vector2 point1, Vector2 point2, int layerMask) {
            var hit = Physics2D.Linecast(point1, point2, layerMask);
            Debug.DrawLine(point1, point2, hit.collider == null ? Color.green : Color.red, Time.deltaTime);
            return hit;
        }

        private bool MovePointIsHigher(RaycastHit2D hit) {
            if (hit.collider == null)
                return false;
            var firstPointPos = _MovementData.CurrentPath[0].Position;
            if (firstPointPos.y > hit.point.y)
                return true;
            return false;
        }
        
        private void LowJump() {
            MovementController.Jump();
        }

        private void HighJump() {
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
