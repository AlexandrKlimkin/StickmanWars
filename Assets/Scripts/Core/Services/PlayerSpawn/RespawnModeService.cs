using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Game.Match;
using KlimLib.SignalBus;
using Tools.Services;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class RespawnModeService : ILoadableService, IUnloadableService {
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
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly GameManagerService _GameManager;

        private Dictionary<byte, int> _PlayersLifesDict;

        private int PlayersAlive => AlivePlayers.Count;

        private List<byte> AlivePlayers => _PlayersLifesDict.Where(_ => _.Value > 0).Select(_=>_.Key).ToList();

        public const int PlayerLifes = 2;

        public void Load() {
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);
            InitializeNewMatch();
            SpawnAllAtTheBegining();
        }

        public void Unload() {
            _SignalBus.UnSubscribe<CharacterDeathSignal>(this);
        }

        public void InitializeNewMatch() {
            _PlayersLifesDict = new Dictionary<byte, int>();
            foreach (var player in _MatchData.Players) {
                _PlayersLifesDict.Add(player.PlayerId, PlayerLifes);
                Debug.LogError(this.GetHashCode());
            }
        }

        private void SpawnAllAtTheBegining() {
            var availablePoints = _PlayersSpawnSettings.PlayerSpawnPoints.ToList();
            foreach (var player in _MatchData.Players) {
                var randomPointIndex = Random.Range(0, availablePoints.Count);
                var point = availablePoints[randomPointIndex];
                availablePoints.Remove(point);
                _CharacterCreationService.CreateCharacter(player.CharacterId, player.PlayerId, true, _PlayersConnectionService.GetDeviceIndex(player.PlayerId).Value, point.Point.position);
            }
        }

        private void SpawnPlayerCharacter(PlayerData playerData, int pointIndex) {
            var deviceIndex = _PlayersConnectionService.GetDeviceIndex(playerData.PlayerId).Value;
            var pos = _PlayersSpawnSettings.PlayerSpawnPoints[pointIndex].Point.position;
            _CharacterCreationService.CreateCharacter(playerData.CharacterId, playerData.PlayerId, true, deviceIndex, pos);
        }

        private void SpawnPlayerCharacter(byte playerId, int pointIndex) {
            var playerData = _MatchService.GetPlayerData(playerId);
            SpawnPlayerCharacter(playerData, pointIndex);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            Debug.LogError(this.GetHashCode());
            _PlayersLifesDict[signal.PlayerId]--;
            if (_PlayersLifesDict[signal.PlayerId] > 0) {
                var respawnPoint = GetRandomCheckpointIndex();
                SpawnPlayerCharacter(signal.PlayerId, respawnPoint);
            }
            else {
                PlayerDefeated(signal.PlayerId);
            }
        }

        private void PlayerDefeated(byte playerId) {
            Debug.LogError($"Player {playerId} defeated.");
            if (PlayersAlive <= 1) {
                EndMatch();
            }
        }

        private void EndMatch() {
            _SignalBus.FireSignal(new MatchEndSignal());
            _GameManager.EndGame();
            Debug.LogError(PlayersAlive > 0 ? $"Match end. Player {AlivePlayers.First()} win!" : $"Match end.");
        }

        private int GetRandomCheckpointIndex() {
            return Random.Range(0, _PlayersSpawnSettings.PlayerSpawnPoints.Count);
        }
    }
}