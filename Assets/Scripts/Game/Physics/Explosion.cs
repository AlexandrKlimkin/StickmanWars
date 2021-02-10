using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using Character.Health;
using Com.LuisPedroFonseca.ProCamera2D;
using Core.Audio;
using UnityDI;
using UnityEditor;
using UnityEngine;

namespace Game.Physics {
    public class Explosion : MonoBehaviour {
        [Dependency]
        private readonly AudioService _AudioService;

        [Button("Play")]
        public bool PlayButton;
        public bool PlayOnStart;
        public bool PlayOnEnable;
        public float ExplosionDelay;
        public LayerMask Layers;
        public float Radius;
        public float MaxForce;
        public float MaxDamage;
        public AnimationCurve StrenghtCurve;
        public string CameraShakePresetName;
        public List<ParticleSystem> VFXEffects;
        public List<string> AudioEffectNames;

        private bool _BuiltUp;

        private void OnEnable() {
            if (PlayOnEnable)
                Play();
        }

        protected virtual void Start() {
            if (PlayOnStart)
                StartCoroutine(ExplosionRoutine());
        }

        private IEnumerator ExplosionRoutine() {
            yield return new WaitForSeconds(ExplosionDelay);
            Play();
        }

        private void PlaySound() {
            if (AudioEffectNames == null || AudioEffectNames.Count == 0)
                return;
            _AudioService.PlaySound3D(AudioEffectNames[UnityEngine.Random.Range(0, AudioEffectNames.Count)], false, false, transform.position);
        }

        private void PlayEffect() {
            VFXEffects?.ForEach(_ => _.Play());
        }

        public virtual void Play() {
            Debug.LogError(transform.position);
            if (!_BuiltUp) {
                ContainerHolder.Container.BuildUp(this);
                _BuiltUp = true;
            }
            var colliders = Physics2D.OverlapCircleAll(transform.position.ToVector2(), Radius, Layers);
            var rigidbodies = new List<Rigidbody2D>();
            var damageables = new List<PartData>();
            foreach (var col in colliders) {
                if (col.attachedRigidbody == null)
                    continue;
                if (rigidbodies.Contains(col.attachedRigidbody))
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
                rb.AddForceAtPosition(totalForce * normilizedVector, closestPoint);
                var damageable = rb.GetComponent<IDamageable>();
                damageables.Add(new PartData { Damageable = damageable, Damage = new Damage(null, damageable, percentForce * MaxDamage, closestPoint, totalForce * normilizedVector * 10f) });
                var velMagnitude = rb.velocity.magnitude;
            }
            StartCoroutine(ApplyDamageRoutine(damageables));
            if (ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(CameraShakePresetName))
                ProCamera2DShake.Instance.Shake(CameraShakePresetName);
            PlayEffect();
            PlaySound();
        }

        private struct PartData {
            public IDamageable Damageable;
            public Damage Damage;
        }

        private IEnumerator ApplyDamageRoutine(List<PartData> parts) {
            yield return null;
            parts.ForEach(_ => {
                if(_.Damageable != null)
                    _.Damageable.ApplyDamage(_.Damage);
            });
        }

        protected virtual void OnDrawGizmos() {
            //Handles.color = Color.red;
            //Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }
    }
}