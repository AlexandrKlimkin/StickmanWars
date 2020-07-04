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

        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public void Kill(Damage dmg) {

        }
    }
}
