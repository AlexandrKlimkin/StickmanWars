using Game.Match;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityDI;
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
            var realPlayers = _Matchdata.Players.Where(_ => !_.IsBot).Count();
            var enoughPlayersToPlay = realPlayers > 0 && CharacterUnit.Characters.Count == realPlayers;
            Animator.SetBool("Show", enoughPlayersToPlay);
        }

        private void OnMatchDataCreated(MatchDataCreatedSignal signal) {
            _Matchdata = signal.MatchData;
        }
    }
}