using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Services;
using Core.Services.Controllers;
using Game.Match;
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
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;

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

        public int GetDeviceIndex(byte playerId) {
            return _DeviceLocalPlayerDict.First(_ => _.Value.PlayerId == playerId).Key;
        }

        private void OnGamepadStatusChanged(GamepadStatusChangedSignal signal) { }

        private void ProcessPlayersConnect() {
            if (_PlayersLimitReached)
                return;
            TryToAddPlayer(KeyCode.Space, 0);
            TryToAddPlayer(KeyCode.Joystick1Button0, 1);
            TryToAddPlayer(KeyCode.Joystick2Button0, 2);
            TryToAddPlayer(KeyCode.Joystick3Button0, 3);
            TryToAddPlayer(KeyCode.Joystick4Button0, 4);
        }

        private void TryToAddPlayer(KeyCode keyCode, int deviceId) {
            if (!Input.GetKeyDown(keyCode))
                return;
            if (_DeviceLocalPlayerDict.ContainsKey(deviceId))
                return;
            if (_PlayersLimitReached)
                return;
            var id = AllocateId();
            var player = new PlayerData(id, id.ToString(), false, id, "Yuri");
            _DeviceLocalPlayerDict.Add(deviceId, player);
            _MatchService.AddPlayer(player);
            _SignalBus.FireSignal(new PlayerConnectedSignal(player, true, deviceId));
            var spawnPoint = _PlayersSpawnSettings.PlayerSpawnPoints[player.PlayerId].Point;
            _CharacterCreationService.CreateCharacter(player.CharacterId, player.PlayerId, true, deviceId, spawnPoint.position);
            Debug.LogError($"player {player.Nickname} spawned");
        }

        private byte AllocateId() {
            var id = _AllocatedId;
            _AllocatedId++;
            return id;
        }
    }
}