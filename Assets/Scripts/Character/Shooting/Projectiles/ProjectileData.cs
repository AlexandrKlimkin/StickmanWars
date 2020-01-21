using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public class ProjectileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float LifeTime;
        public float BirthTime;
        public float Speed;
        public Damage Damage;
    }
}
