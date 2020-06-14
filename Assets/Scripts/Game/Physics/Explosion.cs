using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEditor;
using UnityEngine;

namespace Game.Physics {
    public class Explosion : MonoBehaviour {
        [Button("Play")]
        public bool PlayButton;
        public bool PlayOnStart;
        public LayerMask Layers;
        public float Radius;
        public float MaxForce;
        public AnimationCurve StrenghtCurve;

        protected virtual void Start() {
            if(PlayOnStart)
                Play();
        }

        public virtual void Play() {
            var colliders = Physics2D.OverlapCircleAll(transform.position.ToVector2(), Radius, Layers);
            var rigidbodies = new List<Rigidbody2D>();
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
                var dist = vector.magnitude;
                var normilizedVector = vector / dist;
                var percentForce = StrenghtCurve.Evaluate(dist / Radius);
                var totalForce = percentForce * MaxForce;
                Debug.LogError(totalForce);
                var levitation = rb.GetComponent<Levitation>();
                if (levitation != null)
                    levitation.DisableOnTime(6f);
                rb.AddForceAtPosition(totalForce * normilizedVector, closestPoint);
            }
        }

        protected virtual void OnDrawGizmos() {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }
    }
}