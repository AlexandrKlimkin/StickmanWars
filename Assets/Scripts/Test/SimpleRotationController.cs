using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotationController : MonoBehaviour {
    public float xRotSpeed;
    public float yRotSpeed;
    public float zRotSpeed;

    private bool _Rotate;
    public bool Rotate {
        get {
            return _Rotate;
        }
        set {
            if (!value && _Rotate != value)
                ResetRotation();
            _Rotate = value;
        }
    }

    private Quaternion _StartRotation;

    private void Start() {
        _StartRotation = transform.localRotation;
    }

    private void Update() {
        if(Rotate)
            transform.Rotate(xRotSpeed * Time.deltaTime, yRotSpeed * Time.deltaTime, zRotSpeed * Time.deltaTime, Space.Self);
    }

    private void ResetRotation() {
        StopAllCoroutines();
        StartCoroutine(ResetRotationRoutine());
    }

    private IEnumerator ResetRotationRoutine() {
        while (Quaternion.Angle(transform.localRotation, _StartRotation) > 1f) {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _StartRotation, Time.deltaTime * 5f);
            yield return null;
        }
        transform.localRotation = _StartRotation;
    }
}
