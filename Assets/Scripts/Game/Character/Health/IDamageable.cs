using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health
{
    public interface IDamageable
    {
        Collider2D Collider { get; set; }
        void ApplyDamage(Damage damage);
    }
}
