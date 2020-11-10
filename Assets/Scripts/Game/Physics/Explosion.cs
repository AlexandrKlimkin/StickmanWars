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
        public float MaxVelocityMagnitude;
        public string CameraShakePresetName;
        public List<ParticleSystem> VFXEffects;
        public List<string> AudioEffectNames;

        private bool _BuiltUp;

        private void OnEnable() {
            if (PlayOnEnable)
                Play();
        }

        protected virtual void Start() {
            if(PlayOnStart)
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
            if (!_BuiltUp) {
                ContainerHolder.Container.BuildUp(this);
                _BuiltUp = true;
            }
            var colliders = Physics2D.OverlapCircleAll(transform.position.ToVector2(), Radius, Layers);
            var rigidbodies = new List<Rigidbody2D>();
            var damageables = new List<PartData>();
            var speedLimitsRbs = new List<Rigidbody2D>();
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

                var velMagnitude = rb.velocity.magnitude;
                if (velMagnitude > MaxVelocityMagnitude)
                    speedLimitsRbs.Add(rb);
            }
            StartCoroutine(ApplyDamageAfteFixedUpdate(damageables));
            StartCoroutine(LimitVelocityAfterFixedUpdate(speedLimitsRbs));
            if(ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(CameraShakePresetName))
                ProCamera2DShake.Instance.Shake(CameraShakePresetName);
            PlayEffect();
            PlaySound();
        }

        private struct PartData {
            public IDamageable Damageable;
            public float Damage;
        }

        private IEnumerator ApplyDamageAfteFixedUpdate(List<PartData> parts) {
            yield return new WaitForFixedUpdate();
            parts.ForEach(_=>_.Damageable.ApplyDamage(new Damage(null, _.Damageable, _.Damage)));
        }

        private IEnumerator LimitVelocityAfterFixedUpdate(List<Rigidbody2D> rbs) {
            yield return new WaitForFixedUpdate();
            rbs.ForEach(_ => {
                if (_)
                    _.velocity = _.velocity.normalized * MaxVelocityMagnitude;
            });
        }

        protected virtual void OnDrawGizmos() {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }
    }
}