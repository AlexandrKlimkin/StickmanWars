using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character.Health;
using UnityEngine;

namespace Character.Shooting {
    public class GrenadeProjectile : ThrowingProjectile {
        public float Timer;

        public override void Setup(ThrowingProjectileData data) {
            base.Setup(data);
            StartCoroutine(ExplosionRoutine());
        }

        private IEnumerator ExplosionRoutine() {
            yield return new WaitForSeconds(Timer);
            PerformHit(null, true);
        }
    }
}
