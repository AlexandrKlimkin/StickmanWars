using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health
{
    public interface IDamageable
    {
        void ApplyDamage(Damage damage);
    }
}
