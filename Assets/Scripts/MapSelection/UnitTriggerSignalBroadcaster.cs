using KlimLib.SignalBus;
using UnityDI;

namespace Game.LevelSpecial {
    public abstract class UnitTriggerSignalBroadcaster<T> : UnitTrigger {
        [Dependency]
        protected readonly SignalBus _SignalBus;

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected override void OnUnitEnterTheTrigger(Unit unit) {
            base.OnUnitEnterTheTrigger(unit);
            _SignalBus.FireSignal(CreateSignal(unit, true));
        }

        protected override void OnUnitExitTheTrigger(Unit unit) {
            base.OnUnitExitTheTrigger(unit);
            _SignalBus.FireSignal(CreateSignal(unit, false));
        }

        protected abstract T CreateSignal(Unit unit, bool enter);
    }
}
