using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Projectile<D> : VisualEffect where D : ProjectileDataBase {
        public List<string> HitEffectNames;
        public D Data { get; private set; }
        public bool Initialized { get; private set; }
        public float NormalizedLifeTime => (Time.time - Data.BirthTime) / Data.LifeTime;

        public abstract void Simulate(float time);

        protected bool _Hit;

        public Transform TrailTransformOverride;
        public string TrailName;

        protected AttachedParticleEffect _Trail;

        public virtual void Setup(D data) {
            Data = data;
            transform.position = data.Position;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            transform.rotation = data.Rotation;
            _Hit = false;
            Initialize();
        }

        protected virtual void Initialize() {
            Initialized = true;
            AttachTrail();
        }

        protected override IEnumerator PlayTask() {
            if (!Initialized)
                yield break;
            while (true) {
                Simulate(Time.deltaTime);
                if (NormalizedLifeTime >= 1) {
                    KillProjectile();
                }
                yield return null;
            }
        }

        protected virtual void KillProjectile() {
            this.gameObject.SetActive(false);
            Initialized = false;
            DetachTrail();
        }

        protected virtual void PerformHit(IDamageable damageable, bool killProjectile = true) {
            damageable?.Collider?.attachedRigidbody?.AddForceAtPosition(new Vector2(transform.forward.x, transform.forward.y) * Data.Force, transform.position);

            if (killProjectile)
                KillProjectile();
            _Hit = true;
            Data.Damage.Receiver = damageable;
            ApplyDamage(damageable, Data.Damage);
            PlayHitEffect();
        }

        protected virtual void PlayHitEffect() {
            if (HitEffectNames != null && HitEffectNames.Count > 0) {
                var randIndex = Random.Range(0, HitEffectNames.Count);
                var effect = GetEffect<ParticleEffect>(HitEffectNames[randIndex]);
                effect.transform.position = transform.position;
                effect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
                effect.Play();
            }
        }

        protected virtual void AttachTrail() {
            if (string.IsNullOrEmpty(TrailName))
                return;
            _Trail = GetEffect<AttachedParticleEffect>(TrailName);
            _Trail.gameObject.SetActive(true);
            var target = TrailTransformOverride ?? this.transform;
            _Trail.SetTarget(target);
            _Trail.Play();
        }

        protected virtual void DetachTrail() {
            if (_Trail == null)
                return;
            //_Trail.SetTarget(null);
            _Trail = null;
        }

        protected virtual void ApplyDamage(IDamageable damageable, Damage dmg) {
            damageable?.ApplyDamage(Data.Damage);
        }
    }
}