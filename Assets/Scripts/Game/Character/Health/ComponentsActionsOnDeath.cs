using Character.Health;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsActionsOnDeath : MonoBehaviour {

    public List<ComponentAction> ComponentActions;
    private HealthDamageable HealthDamageable;

    private void Awake() {
        HealthDamageable = GetComponentInParent<HealthDamageable>();
        HealthDamageable.OnDeath += ComponentActionsOnDeath;
    }

    private void ComponentActionsOnDeath() {
        foreach(var componentAction in ComponentActions) {
            componentAction.Component.enabled = componentAction.Enable;
        }
    }

    private void OnDestroy() {
        HealthDamageable.OnDeath -= ComponentActionsOnDeath;
    }
}

[Serializable]
public class ComponentAction {
    public Behaviour Component;
    public bool Enable = true;
}
