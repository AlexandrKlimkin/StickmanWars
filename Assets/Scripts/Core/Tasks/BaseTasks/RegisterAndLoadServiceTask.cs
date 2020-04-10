using System.Collections;
using System.Collections.Generic;
using Core.Services;
using KlimLib.TaskQueueLib;
using UnityDI;
using UnityEngine;

namespace Core.Initialization.Base {
    public class RegisterAndLoadServiceTask<T> : AutoCompletedTask where T : class, new() {
        protected override void AutoCompletedRun() {
            var container = ContainerHolder.Container;
            var serviceInstance = new T();
            container.RegisterInstance(serviceInstance);
            container.BuildUp(serviceInstance.GetType(), serviceInstance);
            (serviceInstance as ILoadableService)?.Load();
        }
    }
}
