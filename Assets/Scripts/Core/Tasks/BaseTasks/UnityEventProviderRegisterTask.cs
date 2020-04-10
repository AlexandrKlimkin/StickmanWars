using System.Collections;
using System.Collections.Generic;
using KlimLib.TaskQueueLib;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Initialization.Base {
    public class UnityEventProviderRegisterTask : AutoCompletedTask {
        protected override void AutoCompletedRun() {
            var obj = new GameObject("EventProvider");
            var eventProvider = obj.AddComponent<UnityEventProvider>();
            ContainerHolder.Container.RegisterInstance(eventProvider);
        }
    }
}
