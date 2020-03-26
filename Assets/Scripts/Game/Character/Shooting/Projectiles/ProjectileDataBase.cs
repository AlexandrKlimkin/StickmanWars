using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public class ProjectileDataBase
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float LifeTime;
        public float BirthTime;
        public Damage Damage;
    }
}
