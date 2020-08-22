using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private GameObject _water;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _endPosition;

    private Vector2 _startPosition;
    private float _progress;
    private void Start()
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(_startPosition, _endPosition, _progress);
        _progress += _speed;
    }

}
