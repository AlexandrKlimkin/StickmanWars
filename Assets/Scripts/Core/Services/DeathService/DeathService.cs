using Character.Health;
using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;

namespace Core.Services.Game {
    public class DeathService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;

        public void Load() {
            _SignalBus.Subscribe<CharacterSpawnedSignal>(OnCharacterSpawned, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void Kill(Damage dmg) {
            dmg.Receiver.Kill();
            _SignalBus?.FireSignal(new CharacterDeathSignal(dmg));
        }

        private void OnCharacterSpawned(CharacterSpawnedSignal signal) {
            signal.Unit.OnApplyDeathDamage += Kill;
        }
    }
}
