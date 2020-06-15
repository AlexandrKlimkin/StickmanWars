using Game.Match;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class PlayerInfoWidget : MonoBehaviour {

        [SerializeField]
        private Text _KillsCount;
        [SerializeField]
        private Image _AvatarImage;
        [SerializeField]
        private Image _HealthImage;

        [Dependency]
        private readonly SignalBus _SignalBus;

        private void Start() {
            
        }

        //public void ConnectToPlayer(PlayerData player) {
        //    _AvatarImage = 
        //}

    }
}