using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI {
    public class RandomPointDestinationTask : UnitTask {

        private MovementData _MovementData;

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run() {
            if (_MovementData.TargetPos != null)
                return TaskStatus.Failure;
            var randPointIndex = Random.Range(0, w2dp_WaypointManager.AllWaypoints.Count);
            _MovementData.TargetPos = w2dp_WaypointManager.AllWaypoints[randPointIndex].Position;
            _MovementData.DestinationType = DestinationType.Random;
            return TaskStatus.Success;
        }
    }
}
