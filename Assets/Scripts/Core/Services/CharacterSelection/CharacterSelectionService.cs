using System.Linq;
using Core.Services.Game;
using Game.Match;
using KlimLib.SignalBus;
using MapSelection;
using Tools.Services;
using UnityDI;
using UnityEngine;

namespace Core.Services.MapSelection {
    public class CharacterSelectionService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly PlayersConnectionService _PlayersConnectionService;

        public void Load() {
            _SignalBus.Subscribe<PlayerConnectedSignal>(OnPlayerConnected, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public void SelectCharacter(byte playerId, string characterId) {
            var player = _MatchService.GetPlayerData(playerId);
            player.SelectCharacter(characterId);
            var deviceId = _PlayersConnectionService.GetDeviceIndex(playerId).Value;
            var spawnPoint = _PlayersSpawnSettings.PlayerSpawnPoints[player.PlayerId].Point;
            _CharacterCreationService.CreateCharacter(player.CharacterId, player.PlayerId, true, deviceId, spawnPoint.position);
            Debug.Log($"player {player.PlayerId} charcacter {player.CharacterId} spawned");
        }


        private void OnPlayerConnected(PlayerConnectedSignal signal) {

        }


    }
}
