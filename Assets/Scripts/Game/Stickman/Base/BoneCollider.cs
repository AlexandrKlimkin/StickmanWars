using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.MuscleSystem {
    public class BoneCollider : MonoBehaviour {
        public event Action GroundCollisionEnter;
        public event Action GroundCollisionStay;
        public event Action<Collision2D> DamageableCollisionEnter;
        public event Action<Collision2D> DamageableCollisionStay;

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "ground") {
                GroundCollisionEnter?.Invoke();
            }
            if (Layers.Masks.Damageable == (Layers.Masks.Damageable | (1 << collision.gameObject.layer))) {
                DamageableCollisionEnter?.Invoke(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision) {
            if (collision.gameObject.tag == "ground") {
                GroundCollisionStay?.Invoke();
            }
            if (Layers.Masks.Damageable == (Layers.Masks.Damageable | (1 << collision.gameObject.layer))) {
                DamageableCollisionStay?.Invoke(collision);
            }
        }
    }
}
