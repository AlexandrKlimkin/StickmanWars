using KlimLib.TaskQueueLib;
using UnityDI;

namespace Core.Initialization.Base {
    public class BaseServiceInitializationTask<TBase, TDerived> : AutoCompletedTask where TDerived : class, TBase, new() {
        protected override void AutoCompletedRun() {
            ContainerHolder.Container.RegisterSingleton<TBase, TDerived>();
        }
    }
}