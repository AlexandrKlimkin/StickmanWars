using Character.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour {
    public HealthDamageable HealthDamageable;

    private void Awake() {
        if(HealthDamageable == null)
            HealthDamageable = GetComponentInParent<HealthDamageable>();
        HealthDamageable.OnDeath += DestroyThisOnDeath;
    }

    private void OnValidate() {
        if (HealthDamageable == null)
            HealthDamageable = GetComponentInParent<HealthDamageable>();
    }

    private void DestroyThisOnDeath() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        HealthDamageable.OnDeath -= DestroyThisOnDeath;
    }
}
