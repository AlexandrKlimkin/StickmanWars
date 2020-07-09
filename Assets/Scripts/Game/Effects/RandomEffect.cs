using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.VisualEffects;
using UnityEngine;

namespace Tools.VisualEffects {
    public class RandomEffect : ParticleEffect {
        private List<ParticleEffect> _Effects;

        private void Start() {
            GetComponentsInChildren(_Effects);
        }

        protected override IEnumerator PlayTask() { yield break; }

        protected override void OnParticleEffectStarted() {
            var rand = UnityEngine.Random.Range(0, _Effects.Count);
            var activeEffect = _Effects[rand];
            _Effects.ForEach(_ => _.gameObject.SetActive(_ == activeEffect));
        }
    }
}
