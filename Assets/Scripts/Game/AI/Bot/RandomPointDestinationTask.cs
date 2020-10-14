using Game.AI.PathFinding;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class RandomPointDestinationTask : UnitTask {

        [Dependency]
        protected readonly WayPointsMangager _WayPointsMangager;

        private MovementData _MovementData;

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run() {
            if (_MovementData.TargetPos != null)
                return TaskStatus.Failure;
            var randPointIndex = Random.Range(0, _WayPointsMangager.WayPoints.Count);
            _MovementData.TargetPos = _WayPointsMangager.WayPoints[randPointIndex].Position;
            _MovementData.DestinationType = DestinationType.Random;
            return TaskStatus.Success;
        }
    }
}
