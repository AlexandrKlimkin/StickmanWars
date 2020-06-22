using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        var damageable = collider.GetComponent<IDamageable>();
        if(damageable == null)
            return;
        damageable.ApplyDamage(new Damage(null, damageable, float.MaxValue));
    }
}
