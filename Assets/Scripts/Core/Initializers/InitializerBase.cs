using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KlimLib.TaskQueueLib;
using UnityEngine;

namespace Core.Initialization {
    public abstract class InitializerBase : MonoBehaviour {
        [NonSerialized]
        public bool Complete;

        protected abstract List<Task> SpecialTasks { get; }

        protected static bool _InitializationRequested;
        protected bool _WasInitialized;

        private void Awake() {
            Initialize();
        }

        private void OnEnable() {
            Initialize();
        }

        private void Initialize() {
            if (_InitializationRequested)
                return;
            _WasInitialized = true;
            _InitializationRequested = true;
            InitializationParameters.BaseTasks
                .Concat(SpecialTasks)
                .ToList()
                .RunTasksListAsQueue(ServicesInitializationComplete, ServicesInitializationFailed, null);
        }

        protected virtual void ServicesInitializationComplete() {
            Debug.Log($"services initialization complete");
            Complete = true;
        }

        protected virtual void ServicesInitializationFailed(Task task, Exception exception) {
            Debug.LogError($"initialization task failed: {task} exception:{exception}");
        }
    }
}