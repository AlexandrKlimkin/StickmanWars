using Character.Health;
using UnityEngine;

public class DisableOnDeath : MonoBehaviour {
    private HealthDamageable HealthDamageable;

    private void Awake() {
        HealthDamageable = GetComponentInParent<HealthDamageable>();
        HealthDamageable.OnDeath += DisableThisOnDeath;
    }

    private void DisableThisOnDeath() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        HealthDamageable.OnDeath -= DisableThisOnDeath;
    }
}