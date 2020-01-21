using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Character.Movement
{
    public class GroundSensor : MonoBehaviour, IGroundSensor
    {
        [SerializeField]
        private float _Radius;
        public float Radius => _Radius;
        public  bool IsGrounded { get; private set; }
        public float DistanseToGround { get; private set; }

        private ContactFilter2D _Filter;
        private readonly List<Collider2D> _Intersections = new List<Collider2D>();

        private void Awake()
        {
            _Filter = new ContactFilter2D { useLayerMask = true, layerMask = Layers.Masks.Walkable };
        }

        private void Update()
        {
            Physics2D.OverlapCircle(transform.position, _Radius, _Filter, _Intersections);
            IsGrounded = _Intersections.Any();

            var hit = Physics2D.Raycast(transform.position, Vector2.down);
            DistanseToGround = hit.transform == null ? float.MaxValue : Vector2.Distance(hit.point, transform.position);
        }

        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded ? Color.green : Color.red;
            Handles.DrawWireDisc(transform.position, transform.forward, _Radius);
            Handles.color = DistanseToGround > 5f ? Color.green : Color.red;
            Handles.DrawLine(transform.position, transform.position - Vector3.up * DistanseToGround);
        }
    }
}