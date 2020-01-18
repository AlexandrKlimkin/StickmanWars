using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Stickman.Movement
{
    public class GroundSensor : MonoBehaviour, IGroundSensor
    {
        [SerializeField]
        private float _Radius;
        public float Radius => _Radius;
        bool IGroundSensor.IsGrounded => _IsGrounded;
        private bool _IsGrounded;

        private ContactFilter2D _Filter;
        private readonly List<Collider2D> _Intersections = new List<Collider2D>();

        private void Awake()
        {
            _Filter = new ContactFilter2D { useLayerMask = true, layerMask = Layers.Masks.Walkable };
        }

        private void Update()
        {
            Physics2D.OverlapCircle(transform.position, _Radius, _Filter, _Intersections);
            _IsGrounded = _Intersections.Any();
        }

        private void OnDrawGizmos()
        {
            Handles.color = _IsGrounded ? Color.green : Color.red;
            Handles.DrawWireDisc(transform.position, transform.forward, _Radius);
        }
    }
}