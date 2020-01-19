using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform NearArmTransform;

    private Camera _Camera;

    private void Start()
    {
        _Camera = Camera.main;
    }

    private void Update()
    {
        NearArmTransform.position = _Camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
