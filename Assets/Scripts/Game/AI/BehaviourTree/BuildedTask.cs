using Tools.BehaviourTree;
using UnityDI;

namespace Game.AI {
    public abstract class BuildedTask : Task {
        protected Blackboard Blackboard => BehaviourTree.Blackboard;
        public override void Init() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        public override void Begin() { }
    }
}