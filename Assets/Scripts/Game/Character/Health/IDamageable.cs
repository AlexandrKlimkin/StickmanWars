using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health {
    public interface IDamageable {
        byte? OwnerId { get; }
        float MaxHealth { get; }
        float Health { get; set; }
        float NormilizedHealth { get; }
        bool Dead { get; set; }
        Collider2D Collider { get; set; }
        void ApplyDamage(Damage damage);
        void Kill(Damage damage);
    }
}
