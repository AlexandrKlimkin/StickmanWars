using System;

namespace KlimLib.SignalBus {
    public class FiredSignalState {
        public Action OnComplete = null;
        public int CurrentFireCount;
    }
}