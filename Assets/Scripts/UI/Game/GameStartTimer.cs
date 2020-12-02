using System.Collections;
using System.Collections.Generic;
using Game.Match;
using KlimLib.SignalBus;
using UnityEngine;
using UnityDI;

namespace UI.Game {
    public class GameStartTimer : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<AnimationObjectNameSignal>(ActivateNumber, this);
            _SignalBus.Subscribe<MatchStartSignal>(OnGameStart, this);
        }

        private void OnGameStart(MatchStartSignal signal) {
            StartCoroutine(WaitRoutine());
        }

        private IEnumerator WaitRoutine() {
            yield return new WaitForSeconds(1);
            transform.GetChild(0).gameObject.GetComponent<Animator>().Play("NumberAppear");
        }

        private void ActivateNumber(AnimationObjectNameSignal signal) {
            var index = signal.Index + 1;
            if(transform.childCount <= index)
                return;
            var animName = transform.childCount - 1 == index ? "FightAppear" : "NumberAppear";
            transform.GetChild(index).gameObject.GetComponent<Animator>().Play(animName);
        }

        private void OnDestroy() {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}
