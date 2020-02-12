using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _LineRenderer;
    [SerializeField]
    private Transform[] _Points;

    private void Update()
    {
        if(_Points == null || _LineRenderer == null)
            return;
        _LineRenderer.SetPositions(_Points.Select(_=>_.position).ToArray());
    }
}
