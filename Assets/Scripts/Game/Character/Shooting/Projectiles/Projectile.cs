using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Projectile<D> : VisualEffect where D : ProjectileDataBase {
        public List<string> HitEffectNames;
        public List<string> DintEffectNames;
        public D Data { get; private set; }
        public bool Initialized { get; private set; }
        public float NormalizedLifeTime => (Time.time - Data.BirthTime) / Data.LifeTime;

        public abstract void Simulate(float time);

        protected bool _Hit;

        public virtual void Setup(D data) {
            Data = data;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            _Hit = false;
            Initialize();
        }

        protected virtual void Initialize() {
            Initialized = true;
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
        }

        protected virtual void PerformHit(IDamageable damageable, bool killProjectile = true) {
            if (killProjectile)
                KillProjectile();
            _Hit = true;
            Data.Damage.Receiver = damageable;
            ApplyDamage(damageable, Data.Damage);
            HitEffect();
            PlayRandomEffect(DintEffectNames);
        }

        private void HitEffect() {
            var hitEffect = PlayRandomEffect(HitEffectNames);
            hitEffect.transform.position = transform.position;
            hitEffect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        }

        protected virtual VisualEffect DintEffect(Vector3 position, Vector2 normal) {
            var dintEffect = PlayRandomEffect(DintEffectNames);
            if (dintEffect == null)
                return null;
            dintEffect.transform.position = position;
            dintEffect.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * normal);
            return dintEffect;
        }

        private VisualEffect PlayRandomEffect(List<string> list) {
            VisualEffect effect = null;
            if (list != null && list.Count > 0) {
                var randIndex = Random.Range(0, HitEffectNames.Count);
                effect = GetEffect<VisualEffect>(list[randIndex]);
                effect.Play();
            }
            return effect;
        }

        protected virtual void ApplyDamage(IDamageable damageable, Damage dmg) {
            damageable?.ApplyDamage(Data.Damage);
        }
    }
}