using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character.Health;
using Items;
using UnityEngine;

namespace Character.Shooting {
    public class GrenadeProjectile : ThrowingProjectile {
        private HealthDamageable _HealthDamageable;
        public float Timer;
        public ThrowingWeapon ThrowingWeapon { get; private set; }

        protected override void Awake() {
            base.Awake();
            _RB = GetComponent<Rigidbody2D>();
            _HealthDamageable = GetComponent<HealthDamageable>();
            ThrowingWeapon = GetComponent<ThrowingWeapon>();
        }

        private void Start() {
            _HealthDamageable.OnDeath += PerformHit;
        }


        public override void Setup(ThrowingProjectileData data) {
            base.Setup(data);
            StartCoroutine(ExplosionRoutine());
        }

        //public override void Play() {
        //    base.Play();
        //}

        private void PerformHit() {
            Setup(ThrowingWeapon.GetProjectileData());
            PerformHit(null, true);
        }

        private IEnumerator ExplosionRoutine() {
            yield return new WaitForSeconds(Timer);
            PerformHit();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            _HealthDamageable.OnDeath -= PerformHit;
        }
    }
}
