using Game.Match;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEditor.Animations;
using UnityEngine;

namespace MapSelection {
    public class TubeBehaviour : MonoBehaviour {
        public Animator Animator;

        [Dependency]
        private readonly SignalBus _SignalBus;
        private MatchData _Matchdata;
        private bool _CanLoad;

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<MatchDataCreatedSignal>(OnMatchDataCreated, this);
        }

        private void Update() {
            if (_Matchdata?.Players == null)
                return;
            var matchDataPlayers = _Matchdata.Players.Count;
            var enoughPlayersToPlay = matchDataPlayers > 0 && CharacterUnit.Characters.Count == matchDataPlayers;
            Animator.SetBool("Show", enoughPlayersToPlay);
        }

        private void OnMatchDataCreated(MatchDataCreatedSignal signal) {
            _Matchdata = signal.MatchData;
        }
    }
}
