using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class Projectile<D> : MonoBehaviour where D : ProjectileData
    {
        public D Data { get; private set; }
        public bool Initialized { get; private set; }
        public float NormalizedLifeTime => (Time.time - Data.BirthTime) / Data.LifeTime;

        public abstract void Simulate(float time);

        public virtual void PerformShot(D data)
        {
            Data = data;
            Initialized = true;
            Setup(data);
        }

        protected virtual void Setup(D data)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }

        protected virtual void Update()
        {
            if (!Initialized)
                return;
            Simulate(Time.deltaTime);
            if (NormalizedLifeTime >= 1)
            {
                KillProjectile();
                return;
            }
        }

        protected virtual void KillProjectile()
        {
            Destroy(this.gameObject); //ToDo: Pooling
            Initialized = false;
        }

        protected virtual void PerformHit(IDamageable damageable)
        {
            KillProjectile();
            damageable?.ApplyDamage(Data.Damage);
        }
    }
}