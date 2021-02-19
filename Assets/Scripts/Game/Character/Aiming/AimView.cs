using Character.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimView : MonoBehaviour {
    [SerializeField]
    private LineRenderer _LineRenderer;
    [SerializeField]
    private SpriteRenderer _AimSprite;
    [SerializeField]
    private Vector2 _LineSizeLimits;

    private IAimProvider _Provider;
    private Transform _AimStartTransform;

    private void Update() {
        if(_Provider == null || !_AimStartTransform) {
            _LineRenderer.gameObject.SetActive(false);
            _AimSprite.gameObject.SetActive(false);
            return;
        }
        _AimSprite.transform.position = _Provider.AimPoint;

    }

    public void SetProvider(IAimProvider provider, Transform aimStartTransform) {
        _Provider = provider;
    }
}