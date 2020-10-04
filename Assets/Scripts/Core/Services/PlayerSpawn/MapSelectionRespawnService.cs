using Character.Health;
using Core.Services.Game;
using Game.Match;
using KlimLib.SignalBus;
using UnityDI;

namespace Core.Services.MapSelection {
    public class MapSelectionRespawnService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;

        public void Load() {
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            var playerId = signal.Damage.Receiver.OwnerId.Value;
            RespawnCharacter(playerId);
        }

        private void RespawnCharacter(byte id) {

        }
    }
}
