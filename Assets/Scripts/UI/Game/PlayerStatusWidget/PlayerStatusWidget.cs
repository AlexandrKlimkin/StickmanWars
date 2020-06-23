using Character.Health;
using Core.Services;
using Core.Services.Game;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class PlayerStatusWidget : MonoBehaviour {
        public Text KillsCountText;
        public Transform LifesContainer;
        public Image Avatar;
        public Image HealthImage;

        public Color HasLifeColor1;
        public Color HasLifeColor2;

        public Color NoLifeColor1;
        public Color NoLifeColor2;

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly BattleStatisticsService _BattleStatisticsService;

        private IPlayerLifesCounter PlayerLifesCounter {
            get {
                if (_PlayerLifesCounter == null)
                    _PlayerLifesCounter = ContainerHolder.Container.Resolve<IPlayerLifesCounter>();
                return _PlayerLifesCounter;
            }
        }
        private IPlayerLifesCounter _PlayerLifesCounter;

        private byte? _AssignedPlayerId;
        private Dictionary<int, List<Image>> _LifesDict = new Dictionary<int, List<Image>>();
        private bool _Initialized;

        private void Initialize() {
            if (_Initialized)
                return;
            _Initialized = true;
            ContainerHolder.Container.BuildUp(this);
            CacheLifes();
        }

        private void CacheLifes() {
            for(int i = 0; i < LifesContainer.childCount; i++) {
                var child = LifesContainer.GetChild(i);
                _LifesDict.Add(i, new List<Image>());
                child.gameObject.GetComponentsInChildren(_LifesDict[i]);
            }
        }

        public void AssignToPlayer(byte playerId) {
            Initialize();
            _AssignedPlayerId = playerId;
            _SignalBus.UnSubscribe<CharacterSpawnedSignal>(this);
            _SignalBus.Subscribe<CharacterSpawnedSignal>(OnCharacterSpawned, this);
            _SignalBus.UnSubscribe<CharacterDeathSignal>(this);
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);

        }

        private void OnCharacterSpawned(CharacterSpawnedSignal signal) {
            var unit = signal.Unit;
            if (_AssignedPlayerId != unit.OwnerId)
                return;
            signal.Unit.OnApplyDamage += UpdateAfterDamage;
            RefreshAvatar(unit.CharacterId);
            RefreshLifesCount(PlayerLifesCounter.PlayersLifesDict[unit.OwnerId]);
            RefreshKillsCount(0);
            RefreshHealthAmount(unit.NormilizedHealth);
        }

        private void UpdateAfterDamage(Damage dmg) {
            RefreshHealthAmount(dmg.Receiver.NormilizedHealth);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            var playerId = signal.Damage.Receiver.OwnerId.Value;
            if (playerId == _AssignedPlayerId) {
                RefreshLifesCount(PlayerLifesCounter.PlayersLifesDict[playerId]);
            }
            if(_AssignedPlayerId == signal.Damage.InstigatorId) {
                KillsCountText.text = _BattleStatisticsService.KillsDict[_AssignedPlayerId.Value].Count.ToString();
            }
        }

        private void RefreshAvatar(string characterId) {
            Avatar.sprite = _ResourceLoader.LoadResource<Sprite>(CharacterConfig.Instance.GetCharacterData(characterId).AvatarPath);
        }

        private void RefreshLifesCount(int lifes) {
            foreach(var life in _LifesDict) {
                life.Value[0].color = life.Key < lifes ? HasLifeColor1 : NoLifeColor1;
                life.Value[1].color = life.Key < lifes ? HasLifeColor2 : NoLifeColor2;
            }
        }

        private void RefreshKillsCount(int kills) {
            KillsCountText.text = kills.ToString();
        }

        private void RefreshHealthAmount(float normilizedHealth) {
            var oldColor = HealthImage.color;
            HealthImage.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1 - normilizedHealth);
        }

        private void OnDestroy() {
            _SignalBus.UnSubscribeFromAll(this);
        }

    }
}