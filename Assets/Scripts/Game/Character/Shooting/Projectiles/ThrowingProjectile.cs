using Character.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.Shooting {
    public class ThrowingProjectile : Projectile<ThrowingProjectileData> {
        [SerializeField]
        protected Rigidbody2D _RB;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        protected virtual void Awake() {
            _RB = GetComponent<Rigidbody2D>();
        }

        public override void Setup(ThrowingProjectileData data) {
            base.Setup(data);
            _RB.velocity = Vector2.zero;
        }

        public override void Simulate(float time) {

        }

        //protected virtual void OnCollisionEnter2D(Collision2D collision) {
        //    if (!_Hit && collision.collider.gameObject != null && !_HitPerformed) {
        //        PerformHit(collision.collider.gameObject.GetComponent<IDamageable>(), false);
        //        _HitPerformed = true;
        //    }
        //}
    }
}
