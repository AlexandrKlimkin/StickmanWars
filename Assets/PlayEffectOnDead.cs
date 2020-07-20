using Core.Audio;
using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityDI;
using UnityEngine;

public class PlayEffectOnDead : MonoBehaviour {
    [Dependency]
    private readonly AudioService _AudioService;

    public string EffectOnKillName;

    private SimpleDamageable _SimpleDamageable;

    private void Awake() {
        _SimpleDamageable = GetComponent<SimpleDamageable>();
        this._SimpleDamageable.OnKill += this._SimpleDamageable_OnKill;
    }

    private void _SimpleDamageable_OnKill(SimpleDamageable dmgbl, Character.Health.Damage dmg) {
        ContainerHolder.Container.BuildUp(this);
        var effect = VisualEffect.GetEffect<VisualEffect>(EffectOnKillName);
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.Play();
    }

    private void OnDestroy() {
        if (this._SimpleDamageable) {
            this._SimpleDamageable.OnKill -= this._SimpleDamageable_OnKill;
        }
    }
}
