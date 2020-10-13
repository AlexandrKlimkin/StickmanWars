using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.BehaviourTree;

namespace Game.AI {
    public class TransformDestinationTask : UnitTask {

        private MovementData _MovementData;

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override Tools.BehaviourTree.TaskStatus Run() {
            if(CharacterUnit.Target != null) {
                _MovementData.TargetPos = CharacterUnit.Target.position;
            }
            return Tools.BehaviourTree.TaskStatus.Success;
        }
    }
}
