using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Cutscenes {
    public abstract class SignalActivator<T> : MonoBehaviour where T : ActivationSignal {
        private SignalBus _SignalBus;

        //protected void OnEnable() {
        //    if (_SignalBus == null)
        //        ContainerHolder.Container.BuildUp(this);
        //    _SignalBus.FireSignal(GetSignal(true));
        //}

        //protected void OnDisable() {
        //    if (_SignalBus == null)
        //        ContainerHolder.Container.BuildUp(this);
        //    _SignalBus.FireSignal(GetSignal(false));
        //}

        //protected virtual T GetSignal(bool active) {
        //    var signal = new T(active);
        //    return signal;
        //}

    }
}
