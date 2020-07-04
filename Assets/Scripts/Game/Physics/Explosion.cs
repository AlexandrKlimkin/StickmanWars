using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using Character.Health;
using UnityEditor;
using UnityEngine;

namespace Game.Physics {
    public class Explosion : MonoBehaviour {
        [Button("Play")]
        public bool PlayButton;
        public bool PlayOnStart;
        public bool PlayOnEnable;
        public LayerMask Layers;
        public float Radius;
        public float MaxForce;
        public float MaxDamage;
        public AnimationCurve StrenghtCurve;

        private void OnEnable() {
            if (PlayOnEnable)
                Play();
        }

        protected virtual void Start() {
            if(PlayOnStart)
                Play();
        }

        public virtual void Play() {
            var colliders = Physics2D.OverlapCircleAll(transform.position.ToVector2(), Radius, Layers);
            var rigidbodies = new List<Rigidbody2D>();
            var damageables = new List<PartData>();
            foreach (var col in colliders) {
                if(col.attachedRigidbody == null)
                    continue;
                if(rigidbodies.Contains(col.attachedRigidbody))
                    continue;
                rigidbodies.Add(col.attachedRigidbody);
            }
            foreach (var rb in rigidbodies) {
                var closestPoint = rb.ClosestPoint(transform.position);
                var vector = closestPoint - transform.position.ToVector2();
                if (vector == Vector2.zero)
                    vector = rb.worldCenterOfMass - transform.position.ToVector2();
                if (vector == Vector2.zero)
                    continue;
                var dist = vector.magnitude;
                var normilizedVector = vector / dist;
                var percentForce = StrenghtCurve.Evaluate(dist / Radius);
                var totalForce = percentForce * MaxForce;
                //Debug.LogError(totalForce);
                var levitation = rb.GetComponent<Levitation>();
                if (levitation != null)
                    levitation.DisableOnTime(6f);
                rb.AddForceAtPosition(totalForce * normilizedVector, closestPoint);
                var damageable = rb.GetComponent<IDamageable>();
                damageables.Add(new PartData { Damageable = damageable, Damage = percentForce * MaxDamage });
            }
            StartCoroutine(ApplyDamageAfteFixedUpdate(damageables));
        }

        private struct PartData {
            public IDamageable Damageable;
            public float Damage;
        }

        private IEnumerator ApplyDamageAfteFixedUpdate(List<PartData> parts) {
            yield return new WaitForFixedUpdate();
            parts.ForEach(_=>_.Damageable.ApplyDamage(new Damage(null, _.Damageable, _.Damage)));
        }


        protected virtual void OnDrawGizmos() {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }
    }
}