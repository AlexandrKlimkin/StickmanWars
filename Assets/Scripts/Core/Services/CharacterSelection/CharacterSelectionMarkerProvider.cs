using System;
using System.Collections;
using Core.Services;
using Core.Services.Game;
using InputSystem;
using KlimLib.SignalBus;
using UI.Markers;
using UnityDI;
using UnityEngine;

namespace MapSelection.UI {
    public class CharacterSelectionMarkerProvider : MarkerProvider<CharacterSelectionMarkerWidget, CharacterSelectionMarkerData> {

        [Dependency]
        private readonly SignalBus _SignalBus;
        private CharacterSelectionService CharacterSelectionService => _CharacterSelectionService ?? (_CharacterSelectionService = ContainerHolder.Container.Resolve<CharacterSelectionService>());
        private CharacterSelectionService _CharacterSelectionService;
        private PlayersConnectionService PlayersConnectionService => _PlayersConnectionService ?? (_PlayersConnectionService = ContainerHolder.Container.Resolve<PlayersConnectionService>());
        private PlayersConnectionService _PlayersConnectionService;

        public byte PlayerId;

        private InputKit _InputKit;
        private CharacterConfig _CharacterConfig;
        private PlayerConnectedSignal _CachedPlayerConnected;
        private bool _PlayerConnected = false;
        private bool _CharacterSelected = false;
        private int _LeafIndex;
        private bool _HasHorizontal;
        private bool _LastFrameHasHorizontal;
        private bool _Leaf;
        private bool _FirstTimeUpdated = false;
        private bool _AllowSpawn = false;
        private bool _ChangePreview => _Leaf || !_FirstTimeUpdated;

        private float _Horizontal = 0f;

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<PlayerConnectedSignal>(OnPlayerConnected, this);
            _CharacterConfig = CharacterConfig.Instance;
        }

        private void OnPlayerConnected(PlayerConnectedSignal signal) {
            var deviceIndex = PlayersConnectionService.GetDeviceIndex(PlayerId);
            if(deviceIndex == null)
                return;
            if (signal.PlayerData.PlayerId != PlayerId)
                return;
            _InputKit = InputConfig.Instance.GetSettings(deviceIndex.Value);
            _PlayerConnected = true;
            StartCoroutine(AllowSpawnRoutine());
            _CachedPlayerConnected = signal;
        }

        private IEnumerator AllowSpawnRoutine() {
            yield return null;
            _AllowSpawn = true;
        }

        private void OnDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
        }

        private float _SrollBeginTime = 0.7f;
        private float _HasHorizontalTimer;
        private bool _IsScrolling;

        private float _ScrollTime = 0.25f;
        private float _ScrollTimer;

        private void Update() {
            if (!_PlayerConnected)
                return;
            if (!_AllowSpawn)
                return;
            if (_CharacterSelected)
                return;
            if (Input.GetKeyDown(_InputKit.Select)) {
                _CharacterSelected = true;
                CharacterSelectionService.SelectCharacter(_CachedPlayerConnected.PlayerData.PlayerId, _CharacterConfig.AvailableCharacters[_LeafIndex].Id);
            }
            _Horizontal = Input.GetAxis(_InputKit.Horizontal);
            _HasHorizontal = Mathf.Abs(_Horizontal) > 0.1f;
            _Leaf = _HasHorizontal && !_LastFrameHasHorizontal;
            var leafWithTimer = false;
            int dir = _Horizontal > 0 ? 1 : -1;
            if (_HasHorizontal) {
                if (_HasHorizontalTimer >= _SrollBeginTime) {
                    if (_ScrollTimer >= _ScrollTime) {
                        Leaf(dir);
                        leafWithTimer = true;
                        _ScrollTimer = 0f;
                    }
                }
                _ScrollTimer += Time.deltaTime;
                _HasHorizontalTimer += Time.deltaTime;;
                _FirstTimeUpdated = true;

                if (_Leaf) {
                    Leaf(dir);
                    leafWithTimer = false;
                }
                _Leaf = leafWithTimer || _Leaf;
            } else {
                _ScrollTimer = 0;
                _HasHorizontalTimer = 0;
            }
            _LastFrameHasHorizontal = _HasHorizontal;
        }

        private void Leaf(int count) {
            _LeafIndex += count;
            var charactersCount = _CharacterConfig.AvailableCharacters.Count;
            if (_LeafIndex >= charactersCount) {
                _LeafIndex = _LeafIndex % charactersCount;
            } else if (_LeafIndex < 0) {
                _LeafIndex = charactersCount + _LeafIndex;
            }
        }

        public override bool GetVisibility() {
            return !_CharacterSelected;
        }

        protected override void RefreshData(CharacterSelectionMarkerData data) {
            base.RefreshData(data);
            data.PlayerConnected = _PlayerConnected;
            data.ChangePreview = _ChangePreview;
            data.PreviewPath = _CharacterConfig?.AvailableCharacters[_LeafIndex].AvatarPath;
            data.Right = _Horizontal > 0;
            data.Left = _Horizontal < 0;
        }
    }
}