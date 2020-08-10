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
            var sqrDist = Vector2.SqrMagnitude(_MovementData.CurrentPath.Last().Position.ToVector2() - CharacterUnit.transform.position.ToVector2());
            if (sqrDist > 25) {
                ProcessMove();
                return TaskStatus.Running;
            } else {
                _MovementData.TargetPos = null;
                _MovementData.DestinationType = DestinationType.None;
                return TaskStatus.Success;
            }
        }

        private void FindNewPath() {
            _MovementData.CurrentPath = _WayPointsMangager.CalculateGraphPath(CharacterUnit.transform.position, _MovementData.TargetPos.Value);
        }

        private void ProcessMove() {
            if (_MovementData.CurrentPath.Count < 2)
                return;
            var firstPoint = _MovementData.CurrentPath[1];
            var targetVector = firstPoint.Position - CharacterUnit.transform.position;
            var dist = targetVector.magnitude;
            var horDist = Mathf.Abs(targetVector.x);
            float horizontal = 0;
            if(horDist > 1) {
                horizontal = targetVector.x > 0 ? 1f : -1f;
            }
            MovementController.SetHorizontal(horizontal);

            var vertDist = targetVector.y;
            if(dist <= 100) {
                if (vertDist >= 30) {
                    if(!MovementController.HighJump())
                        MovementController.WallJump();
                } else if (vertDist >= 15) {
                    if (!MovementController.Jump())
                        MovementController.WallJump();
                }
            }
        }
    }

    public enum DestinationType {
        None, Weapon, ShootPosition, Escape, Random
    }
}
