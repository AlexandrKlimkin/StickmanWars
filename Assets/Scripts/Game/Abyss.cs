using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class Abyss : MonoBehaviour {
    public LayerMask LayerMask;
    private void OnTriggerEnter2D(Collider2D collider) {
        if (!Layers.Masks.LayerInMask(LayerMask, collider.gameObject.layer))
            return;
        var damageable = collider.GetComponentInParent<IDamageable>();
        damageable?.Kill(new Damage(null, damageable, float.MaxValue));
    }
}
