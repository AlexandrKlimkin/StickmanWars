using Character.Control;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;

public class AimView : AimViewBase {
    [SerializeField]
    private LineRenderer _LineRenderer;
    [SerializeField]
    private SpriteRenderer _AimSprite;
    [SerializeField]
    private Vector2 _AimPosLimits;
    [SerializeField]
    private float _LineLength;

    private void Update() {
        if(_Provider == null || !_AimStartTransform) {
            _LineRenderer.gameObject.SetActive(false);
            _AimSprite.gameObject.SetActive(false);
            return;
        }
        _LineRenderer.gameObject.SetActive(true);
        _AimSprite.gameObject.SetActive(false);
        //_AimSprite.gameObject.SetActive(true);

        var firstpoint = _AimStartTransform.position.ToVector2();



        //var aimVector = _Provider.AimPoint - firstpoint;
        //var lineVector = aimVector.normalized * _LineLength;
        //aimVector = Vector2.ClampMagnitude(aimVector, _AimPosLimits.y);
        var secondPoint = firstpoint + _AimStartTransform.forward.ToVector2() * _LineLength;
        //_AimSprite.transform.position = firstpoint + aimVector;
        var positions = new Vector3[] {
            firstpoint,
            secondPoint,
        };
        _LineRenderer.SetPositions(positions);
    }

}