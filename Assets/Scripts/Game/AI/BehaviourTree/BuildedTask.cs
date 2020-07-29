using Tools.BehaviourTree;
using Tools.Unity;
using UnityDI;

namespace Game.AI {
    public abstract class BuildedTask : Task {
        protected Blackboard Blackboard => BehaviourTree.Blackboard;

        private UnityEventProvider _UnityEventProvider;

        protected bool _UpdatedTask = false;
        public bool UpdatedTask {
            get { return _UpdatedTask; }
            set {
                if (_UpdatedTask != value) {
                    _UpdatedTask = value;
                    if (value)
                        SubscribeOnEvents();
                    else
                        UnsubscrbeFromEvents();
                }
            }
        }

        public override void Init() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected void SubscribeOnEvents() {
            if (!_UnityEventProvider)
                _UnityEventProvider = ContainerHolder.Container.Resolve<UnityEventProvider>();
            _UnityEventProvider.OnUpdate += UpdateRunningTask;
            _UnityEventProvider.OnLateUpdate += LateUpdateRunningTask;
            _UnityEventProvider.OnGizmos += OnDrawGizmos;
        }


        protected void UnsubscrbeFromEvents() {
            if (_UnityEventProvider) {
                _UnityEventProvider.OnUpdate -= UpdateRunningTask;
                _UnityEventProvider.OnLateUpdate -= LateUpdateRunningTask;
                _UnityEventProvider.OnGizmos -= OnDrawGizmos;
            }

        }

        private void UpdateRunningTask() {
            if (Status == TaskStatus.Running)
                Update();
        }

        private void LateUpdateRunningTask() {
            if (Status == TaskStatus.Running)
                LateUpdate();
        }

        public override void Begin() { }

        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void OnDrawGizmos() { }
    }
}