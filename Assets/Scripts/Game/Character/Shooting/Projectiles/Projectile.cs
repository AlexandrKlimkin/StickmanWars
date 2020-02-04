using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class Projectile<D> : VisualEffect where D : ProjectileData
    {
        public D Data { get; private set; }
        public bool Initialized { get; private set; }
        public float NormalizedLifeTime => (Time.time - Data.BirthTime) / Data.LifeTime;

        public abstract void Simulate(float time);

        public virtual void Setup(D data)
        {
            Data = data;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            Initialize();
        }

        protected virtual void Initialize()
        {
            Initialized = true;
        }
        
        protected override IEnumerator PlayTask()
        {
            if (!Initialized)
                yield break;
            while (true)
            {
                Simulate(Time.deltaTime);
                if (NormalizedLifeTime >= 1)
                {
                    KillProjectile();
                }
                yield return null;
            }
        }

        protected virtual void KillProjectile()
        {
            this.gameObject.SetActive(false);
            Initialized = false;
        }

        protected virtual void PerformHit(IDamageable damageable)
        {
            KillProjectile();
            Data.Damage.Force = transform.right;
            damageable?.ApplyDamage(Data.Damage);
        }
    }
}