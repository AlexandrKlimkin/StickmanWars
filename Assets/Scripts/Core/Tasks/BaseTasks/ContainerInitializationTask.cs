using KlimLib.TaskQueueLib;
using UnityDI;

namespace Core.Initialization.Base {
    public class ContainerInitializationTask : AutoCompletedTask {
        protected override void AutoCompletedRun() {
            ContainerHolder.Container.RegisterInstance(ContainerHolder.Container);
        }
    }
}
