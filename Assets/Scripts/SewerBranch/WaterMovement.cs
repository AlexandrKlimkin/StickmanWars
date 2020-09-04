using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private GameObject _Water;
    [SerializeField] private float _Speed;
    [SerializeField] private Vector2 _EndPosition;

    private Vector2 _StartPosition;
    private float _Progress;
    private void Start()
    {
        _StartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(_StartPosition, _EndPosition, _Progress);
        _Progress += _Speed;
    }

}
