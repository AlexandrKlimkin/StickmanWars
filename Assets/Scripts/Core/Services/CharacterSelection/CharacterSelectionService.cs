using Core.Services.Game;
using KlimLib.SignalBus;
using Tools.Services;
using UnityDI;

namespace Core.Services.MapSelection {
    public class CharacterSelectionService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;

        public void Load() { }
        public void Unload() { }

        public void SelectCharacter() {

        }
    }
}
