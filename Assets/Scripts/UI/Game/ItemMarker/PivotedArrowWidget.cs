using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotedArrowWidget : MonoBehaviour {
    [SerializeField]
    private RectTransform _Contents;

    public void SetRotation(float angle) {
        _Contents.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
