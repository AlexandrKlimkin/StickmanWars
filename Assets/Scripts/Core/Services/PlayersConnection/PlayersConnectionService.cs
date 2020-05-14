using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Services;
using Core.Services.Controllers;
using Game.Match;
using InputSystem;
using KlimLib.SignalBus;
using Tools.Services;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class PlayersConnectionService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly UnityEventProvider _EventProvider;
        [Dependency]
        private readonly ControllersStatusService _ControllersStatusService;
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;
        [Dependency]
        private readonly PlayersSpawnSettings _SpawnSettings;

        private InputConfig _InputConfig => InputConfig.Instance;
        private byte _AllocatedId;

        private Dictionary<int, PlayerData> _DeviceLocalPlayerDict = new Dictionary<int, PlayerData>();

        private bool _PlayersLimitReached => _MatchData.Players.Count >= MatchService.MaxPlayerCount;

        public const int MaxPlayerCount = 4;

        public void Load() {
            _EventProvider.OnUpdate += ProcessPlayersConnect;
            _SignalBus.Subscribe<GamepadStatusChangedSignal>(OnGamepadStatusChanged, this);
        }

        public void Unload() {
            _EventProvider.OnUpdate -= ProcessPlayersConnect;
            _SignalBus.UnSubscribeFromAll(this);
        }

        public int? GetDeviceIndex(byte playerId) {
            return _DeviceLocalPlayerDict.FirstOrDefault(_ => _.Value.PlayerId == playerId).Key;
        }

        public bool PlayerConnected(byte playerId, out int? deviceIndex) {
            deviceIndex = GetDeviceIndex(playerId);
            return deviceIndex != null;
        }

        private void OnGamepadStatusChanged(GamepadStatusChangedSignal signal) { }

        private void ProcessPlayersConnect() {
            if (_PlayersLimitReached)
                return;
            foreach (var input in _InputConfig.InputKitsDict) {
                TryToAddPlayer(input.Value.Select, input.Key);
            }
        }

        private void TryToAddPlayer(KeyCode keyCode, int deviceId) {
            if (!Input.GetKeyDown(keyCode))
                return;
            if (_DeviceLocalPlayerDict.ContainsKey(deviceId))
                return;
            if (_PlayersLimitReached)
                return;
            var id = AllocatePlayerId();
            var player = new PlayerData(id, id.ToString(), false, id, null);
            _DeviceLocalPlayerDict.Add(deviceId, player);
            _MatchService.AddPlayer(player);
            Debug.Log($"player {player.PlayerId} connected");
            _SignalBus.FireSignal(new PlayerConnectedSignal(player, true, deviceId));
        }

        private byte AllocatePlayerId() {
            var id = _AllocatedId;
            _AllocatedId++;
            return id;
        }
    }
}