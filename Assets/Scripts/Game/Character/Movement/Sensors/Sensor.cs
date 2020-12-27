using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Character.Movement {
    public class Sensor : MonoBehaviour, ISensor {
        [SerializeField]
        private float _Radius;
        [SerializeField]
        private bool _Raycast;
        [SerializeField]
        private Vector3 _Direction;
        [SerializeField]
        private LayerMask _LayerMask;
        [SerializeField]
        private bool _InteractTriggers;
        [SerializeField]
        private bool _InteractEffectorColliders;

        [SerializeField]
        private List<Collider2D> _ExtentionColliderList;
        public List<Collider2D> ExtentionColliderList {
            get {
                return _ExtentionColliderList;
            }
            private set {
                _ExtentionColliderList = value;
            }
        }

        public float Radius => _Radius;
        public bool IsTouching { get; private set; }
        public bool TouchingEffector { get; private set; }
        public float Distanse { get; private set; }
        public List<Collider2D> TouchedColliders;
        public List<Collider2D> TouchedEffectors;

        private ContactFilter2D _Filter;

        private void Awake() {
            _Filter = new ContactFilter2D { useLayerMask = true, layerMask = _LayerMask, useTriggers = _InteractTriggers };
        }

        private void Update() {
            Physics2D.OverlapCircle(transform.position, _Radius, _Filter, TouchedColliders);
            if(ExtentionColliderList != null)
                TouchedColliders.RemoveAll(_ => ExtentionColliderList.Contains(_));
            if (_InteractEffectorColliders) {
                IsTouching = TouchedColliders.Any();
                TouchedEffectors = TouchedColliders.Where(_ => _.usedByEffector).ToList();
                TouchingEffector = TouchedEffectors.Any();
            } else {
                IsTouching = TouchedColliders.Any(_ => _.usedByEffector == false);
                TouchedEffectors = null;
                TouchingEffector = false;
            }
            if (_Raycast) {
                var hit = Physics2D.Raycast(transform.position, _Direction, 1000, _LayerMask);
                Distanse = hit.transform == null ? float.MaxValue : Vector2.Distance(hit.point, transform.position);
            }
        }

        public void AddExtentionCollider(Collider2D collider) {
            if (ExtentionColliderList == null)
                ExtentionColliderList = new List<Collider2D>();
            ExtentionColliderList.Add(collider);
        }

        private void OnDrawGizmos() {
#if UNITY_EDITOR
            Handles.color = IsTouching ? Color.green : Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _Radius);
            //Handles.color = Distanse > 25f ? Color.green : Color.red;
            //Handles.DrawLine(transform.position, transform.position + _Direction * Distanse);
#endif
        }
    }
}