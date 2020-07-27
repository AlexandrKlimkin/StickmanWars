using Assets.Scripts.Tools;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI {
    public class MoveToPointTask : UnitTask {

        private w2dp_PathCalculator calculator = new w2dp_PathCalculator();
        private List<w2dp_Waypoint> currentPath;
        private Transform _Target;

        public MoveToPointTask(Transform target) {
            _Target = target;
        }

        private float _StopDistance;

        public override TaskStatus Run() {
            if (_Target == null)
                return TaskStatus.Failure;
            var sqrDist = Vector2.SqrMagnitude(_Target.position.ToVector2() - CharacterUnit.transform.position.ToVector2());
            if (sqrDist > 4) {
                FindNewPath();
                ProcessMove();
                return TaskStatus.Running;
            } else
                return TaskStatus.Success;
        }

        private void FindNewPath() {
            currentPath = calculator.GetPath(CharacterUnit.transform.position, _Target.position);
        }

        private void ProcessMove() {
            if (currentPath.Count == 0)
                return;
            var firstPoint = currentPath[0];
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
}
