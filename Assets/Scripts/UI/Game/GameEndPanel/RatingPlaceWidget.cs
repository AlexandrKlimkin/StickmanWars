using Core.Services;
using Core.Services.Game;
using Game.Match;
using KlimLib.ResourceLoader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class RatingPlaceWidget : MonoBehaviour {

        public Text RatingNumberText;
        public Image PreviewImage;
        public Transform KillsPreviewContainer;
        public Image KillPreviewPrefab;
        public Text CharacterNameText;
        public List<Color> PlaceColors;

        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly BattleStatisticsService _BattleStatisticsService;

        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
        }

        public void DisplayStats(byte playerId, int place) {
            var playerData = _MatchService.GetPlayerData(playerId);
            RatingNumberText.text = place.ToString() + ".";
            RatingNumberText.color = PlaceColors[place - 1];
            var characterData = CharacterConfig.Instance.GetCharacterData(playerData.CharacterId);
            var playerCharacterPreviewPath = characterData.AvatarPath;
            PreviewImage.sprite = _ResourceLoader.LoadResource<Sprite>(playerCharacterPreviewPath);

            var killsData = _BattleStatisticsService.KillsDict[playerId];
            for(int i = 0; i < KillsPreviewContainer.childCount; i++) {
                var child = KillsPreviewContainer.GetChild(i);
                Destroy(child.gameObject);
            }
            foreach(var killData in killsData) {
                var killPreview = Instantiate(KillPreviewPrefab, KillsPreviewContainer);
                var killedId = killData.KilledPlayerId;
                var killedCharacterId = _MatchService.GetPlayerData(killedId).CharacterId;
                var killedCharacterData = CharacterConfig.Instance.GetCharacterData(killedCharacterId);
                var killedCharacterPreviewPath = killedCharacterData.AvatarPath;
                KillPreviewPrefab.sprite = _ResourceLoader.LoadResource<Sprite>(killedCharacterPreviewPath);
            }
            CharacterNameText.text = characterData.Name;
        }
    }
}