using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        var damageable = collider.GetComponentInParent<IDamageable>();
        if(damageable == null)
            return;
        damageable.Kill(new Damage(null, damageable, float.MaxValue));
    }
}
