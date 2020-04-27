using KlimLib.SignalBus;
using UnityDI;

namespace Game.LevelSpecial {
    public abstract class UnitTriggerSignalBroadcaster<T> : UnitTrigger {
        [Dependency]
        protected readonly SignalBus _SignalBus;

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected override void OnUnitEnterTheTrigger(CharacterUnit characterUnit) {
            base.OnUnitEnterTheTrigger(characterUnit);
            _SignalBus.FireSignal(CreateSignal(characterUnit, true));
        }

        protected override void OnUnitExitTheTrigger(CharacterUnit characterUnit) {
            base.OnUnitExitTheTrigger(characterUnit);
            _SignalBus.FireSignal(CreateSignal(characterUnit, false));
        }

        protected abstract T CreateSignal(CharacterUnit characterUnit, bool enter);
    }
}
