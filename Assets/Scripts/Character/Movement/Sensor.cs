using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Character.Movement
{
    public class Sensor : MonoBehaviour, ISensor
    {
        [SerializeField]
        private float _Radius;
        [SerializeField]
        private Vector3 _Direction;

        public float Radius => _Radius;
        public  bool IsTouching { get; private set; }
        public float Distanse { get; private set; }

        private ContactFilter2D _Filter;
        private readonly List<Collider2D> _Intersections = new List<Collider2D>();

        private void Awake()
        {
            _Filter = new ContactFilter2D { useLayerMask = true, layerMask = Layers.Masks.Walkable };
        }

        private void Update()
        {
            Physics2D.OverlapCircle(transform.position, _Radius, _Filter, _Intersections);
            IsTouching = _Intersections.Any();

            var hit = Physics2D.Raycast(transform.position, _Direction, 1000, Layers.Masks.NoCharacter);
            Distanse = hit.transform == null ? float.MaxValue : Vector2.Distance(hit.point, transform.position);
        }

        private void OnDrawGizmos()
        {
            Handles.color = IsTouching ? Color.green : Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _Radius);
            Handles.color = Distanse > 25f ? Color.green : Color.red;
            Handles.DrawLine(transform.position, transform.position + _Direction * Distanse);
        }
    }
}