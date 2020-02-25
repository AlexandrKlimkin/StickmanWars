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
        private bool _Raycast;
        [SerializeField]
        private Vector3 _Direction;
        [SerializeField]
        private LayerMask _LayerMask;

        public float Radius => _Radius;
        public  bool IsTouching { get; private set; }
        public float Distanse { get; private set; }
        public List<Collider2D> TouchedColliders;

        private ContactFilter2D _Filter;

        private void Awake()
        {
            _Filter = new ContactFilter2D { useLayerMask = true, layerMask = _LayerMask, useTriggers = true};
        }

        private void Update()
        {
            Physics2D.OverlapCircle(transform.position, _Radius, _Filter, TouchedColliders);
            IsTouching = TouchedColliders.Any();
            if (_Raycast)
            {
                var hit = Physics2D.Raycast(transform.position, _Direction, 1000, _LayerMask);
                Distanse = hit.transform == null ? float.MaxValue : Vector2.Distance(hit.point, transform.position);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.color = IsTouching ? Color.green : Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _Radius);
            //Handles.color = Distanse > 25f ? Color.green : Color.red;
            //Handles.DrawLine(transform.position, transform.position + _Direction * Distanse);
#endif
        }
    }
}