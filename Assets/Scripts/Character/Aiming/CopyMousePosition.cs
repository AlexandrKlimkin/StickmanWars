using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMousePosition : MonoBehaviour
{
    public Transform Transform;

    private void LateUpdate()
    {
        Transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
