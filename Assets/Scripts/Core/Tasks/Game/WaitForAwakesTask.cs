using System.Collections;
using System.Collections.Generic;
using KlimLib.TaskQueueLib;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Initialization.Game {
    public class WaitForAwakesTask : Task {
        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        public override void Run() {
            _EventProvider.StartCoroutine(WaitRoutine());
        }

        private IEnumerator WaitRoutine() {
            yield return null;
            OnComplete.Invoke(this);
        }
    }
}
