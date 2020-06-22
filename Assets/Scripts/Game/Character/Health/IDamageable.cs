using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health {
    public interface IDamageable {
        byte? OwnerId { get; }
        float Health { get; }
        float NormilizedHealth { get; }
        Collider2D Collider { get; set; }
        void ApplyDamage(Damage damage);
        void Kill();
    }
}
