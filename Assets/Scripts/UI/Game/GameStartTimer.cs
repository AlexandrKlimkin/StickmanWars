using System.Collections;
using System.Collections.Generic;
using Game.Match;
using KlimLib.SignalBus;
using UnityEngine;
using UnityDI;
using Core.Audio;

namespace UI.Game {
    public class GameStartTimer : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly AudioService _AudioService;

        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<AnimationObjectNameSignal>(ActivateNumber, this);
            _SignalBus.Subscribe<MatchReadySignal>(OnMatchReady, this);
        }

        private void OnMatchReady(MatchReadySignal signal) {
            StartCoroutine(WaitRoutine());
        }

        private IEnumerator WaitRoutine() {
            yield return new WaitForSeconds(1);
            PlayBeep(0);
        }

        private void ActivateNumber(AnimationObjectNameSignal signal) {
            var index = signal.Index + 1;
            if(transform.childCount <= index)
                return;
            PlayBeep(index);
        }

        private void PlayBeep(int index) {
            var isLast = transform.childCount - 1 == index;
            var animName = isLast ? "FightAppear" : "NumberAppear";
            transform.GetChild(index).gameObject.GetComponent<Animator>().Play(animName);
            var soundName = isLast ? "CountDownBeepLast" : "CountDownBeep";
            _AudioService.PlaySound2D(soundName, false, false);
        }

        private void OnDestroy() {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}
