using System.Linq;
using Game.Match;
using Tools.Services;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class CharacterSpawnService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly PlayersConnectionService _PlayersConnectionService;
        [Dependency]
        private readonly MatchService _MatchService;

        public void Load() {
            SpawnAllAtTheBegining();
        }

        public void Unload() {

        }

        private void SpawnAllAtTheBegining() {
            var availablePoints = _PlayersSpawnSettings.PlayerSpawnPoints.ToList();
            foreach (var player in _MatchData.Players) {
                var randomPointIndex = Random.Range(0, availablePoints.Count);
                var point = availablePoints[randomPointIndex];
                availablePoints.Remove(point);
                _CharacterCreationService.CreateCharacter(player.CharacterId, true, _PlayersConnectionService.GetDeviceIndex(player.PlayerId), point.Point.position);
            }
        }

        private void SpawnPlSpawnPlayerCharacter(PlayerData playerData, int pointIndex) {
            var deviceIndex = _PlayersConnectionService.GetDeviceIndex(playerData.PlayerId);
            var pos = _PlayersSpawnSettings.PlayerSpawnPoints[pointIndex].Point.position;
            _CharacterCreationService.CreateCharacter(playerData.CharacterId, true, deviceIndex, pos);
        }

        private void SpawnPlayerCharacter(byte playerId, int pointIndex) {
            var playerData = _MatchService.GetPlayerData(playerId);
            SpawnPlSpawnPlayerCharacter(playerData, pointIndex);
        }

        private int GetRandomCheckpointIndex() {
            return Random.Range(0, _PlayersSpawnSettings.PlayerSpawnPoints.Count);
        }
    }
}