using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Character.Control;
using Core.Services;
using Core.Services.Controllers;
using Core.Services.SceneManagement;
using Game.Match;
using InControl;
using KlimLib.SignalBus;
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
        [Dependency]
        private readonly SceneManagerService _SceneManagerService;

        private byte _AllocatedId;

        private Dictionary<PlayerActions, PlayerData> _DeviceLocalPlayerDict = new Dictionary<PlayerActions, PlayerData>();

        private bool _PlayersLimitReached => _MatchData.Players.Count >= MatchService.MaxPlayerCount;

        public const int MaxPlayerCount = 4;

        public int MaxBotsCount { get; set; } = 0;// MaxPlayerCount - 1;

        PlayerActions _KeyboardListener;
        PlayerActions _JoystickListener;

        public void Load() {
            if (!_SceneManagerService.IsGameScene)
                _EventProvider.OnUpdate += ProcessPlayersConnect;

            _KeyboardListener = PlayerActions.CreateWithKeyboardBindings();
            _JoystickListener = PlayerActions.CreateWithJoystickBindings();
            //_SignalBus.Subscribe<GamepadStatusChangedSignal>(OnGamepadStatusChanged, this);
            //_SignalBus.Subscribe<MatchStartSignal>(OnMatchStart, this);

            foreach (var player in _MatchData.Players) {
                var unit = CharacterUnit.Characters.FirstOrDefault(_ => _.OwnerId == player.PlayerId);
                if(unit == null)
                    continue;
                var playerController = unit.GetComponent<PlayerController>();
                FillDeviceIndexForce(playerController.PlayerActions, player);
                _SignalBus.FireSignal(new PlayerConnectedSignal(player, true, playerController.PlayerActions));
            }
        }

        public void Unload() {
            _EventProvider.OnUpdate -= ProcessPlayersConnect;
            _SignalBus.UnSubscribeFromAll(this);
        }

        public PlayerActions GetPlayerActions(byte playerId) {
            return _DeviceLocalPlayerDict.FirstOrDefault(_ => _.Value.PlayerId == playerId).Key;
        }

        public bool PlayerConnected(byte playerId, out PlayerActions playerActions) {
            playerActions = GetPlayerActions(playerId);
            return playerActions != null;
        }

        private void FillDeviceIndexForce(PlayerActions playerActions, PlayerData player) {
            _DeviceLocalPlayerDict.Add(playerActions, player);
        }

        private void ProcessPlayersConnect() {
            if (_PlayersLimitReached)
                return;
            //Debug.LogError(InputManager.Devices.ToStringInternal());
            //TryToAddPlayer(InputManager.ActiveDevice);
            if (JoinButtonWasPressedOnListener(_JoystickListener)) {
                var inputDevice = InputManager.ActiveDevice;

                if (ThereIsNoPlayerUsingJoystick(inputDevice)) {
                    CreatePlayer(inputDevice);
                }
            }

            if (JoinButtonWasPressedOnListener(_KeyboardListener)) {
                if (ThereIsNoPlayerUsingKeyboard()) {
                    CreatePlayer(null);
                }
            }
        }


        bool JoinButtonWasPressedOnListener(PlayerActions actions) {
            return actions.Confirm.WasPressed;
        }

        private PlayerData FindPlayerUsingJoystick(InputDevice inputDevice) {
            foreach (var pair in _DeviceLocalPlayerDict) {
                var player = pair.Value;
                if (pair.Key.Device == inputDevice) {
                    return player;
                }
            }
            return null;
        }

        private bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice) {
            return FindPlayerUsingJoystick(inputDevice) == null;
        }


        private PlayerData FindPlayerUsingKeyboard() {
            foreach (var pair in _DeviceLocalPlayerDict) {
                var player = pair.Value;
                if (pair.Key == _KeyboardListener) {
                    return player;
                }
            }
            return null;
        }


        private bool ThereIsNoPlayerUsingKeyboard() {
            return FindPlayerUsingKeyboard() == null;
        }

        private void CreatePlayer(InputDevice inputDevice) {
            if (_PlayersLimitReached)
                return;
            if (_DeviceLocalPlayerDict.Any(_ => _.Key.Device == inputDevice))
                return;
            var id = AllocatePlayerId();
            var player = new PlayerData(id, id.ToString(), false, id, null);
            PlayerActions actions;
            if (inputDevice == null) {
                actions = _KeyboardListener;
            } else {
                actions = PlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;
            }
            _DeviceLocalPlayerDict.Add(actions, player);
            _MatchService.AddPlayer(player);
            Debug.Log($"player {player.PlayerId} connected");
            _SignalBus.FireSignal(new PlayerConnectedSignal(player, true, actions));
        }


        //private void TryToAddPlayer(InputDevice inputDevice) {
        //    if (!inputDevice.AnyButtonIsPressed)
        //        return;
        //    if (_DeviceLocalPlayerDict.Any(_=>_.Key.Device == inputDevice))
        //        return;
        //    if (_PlayersLimitReached)
        //        return;
        //    var id = AllocatePlayerId();
        //    var player = new PlayerData(id, id.ToString(), false, id, null);
        //    //var playerActions = PlayerActions.CreateWithDefaultBindings();
        //    //playerActions.Device = inputDevice;
        //    _DeviceLocalPlayerDict.Add(playerActions, player);
        //    _MatchService.AddPlayer(player);
        //    Debug.Log($"player {player.PlayerId} connected");
        //    _SignalBus.FireSignal(new PlayerConnectedSignal(player, true, playerActions));
        //}

        public void AddBots() {
            var playersDontPlay = 4 - _MatchData.Players.Count;
            var botsNeedToSpawn = Mathf.Min(playersDontPlay, MaxBotsCount);
            byte maxIndex = _MatchData.Players.Count > 0 ? _MatchData.Players.Max(_ => _.PlayerId) : (byte)0;
            maxIndex++;
            for (byte index = maxIndex; index < maxIndex + botsNeedToSpawn; index++) {
                var player = new PlayerData(AllocatePlayerId(), index.ToString(), true, index, "Robot");
                _MatchService.AddPlayer(player);
            }
        }

        private byte AllocatePlayerId() {
            var id = _AllocatedId;
            _AllocatedId++;
            return id;
        }
    }
}