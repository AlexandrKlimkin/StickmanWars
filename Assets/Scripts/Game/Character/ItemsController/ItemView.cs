using System.Collections;
using System.Collections.Generic;
using Game.Physics;
using UnityEngine;

namespace Items
{
    public class ItemView : MonoBehaviour
    {
        public GameObject CollidersContainer;

        private List<Collider2D> _Colliders = new List<Collider2D>();
        private float _StartXScaleSign;

        public Rigidbody2D Rigidbody { get; private set; }
        public Levitation Levitation { get; private set; }

        public bool FallingDown { get; private set; }

        protected virtual void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            Levitation = GetComponent<Levitation>();
            CollidersContainer.GetComponentsInChildren(_Colliders);
        }

        protected virtual void Start() {
            _StartXScaleSign = Mathf.Sign(transform.lossyScale.x);
        }

        public virtual void PickUp(Transform place) {
            Rigidbody.simulated = false;
            CollidersContainer.SetActive(false);
            transform.SetParent(place);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _StartXScaleSign, transform.localScale.y, transform.localScale.z);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            StopAllCoroutines();
            Levitation.SetActive(false);
        }

        public virtual void ThrowOut(GameObject thrower) {
            Rigidbody.simulated = true;
            CollidersContainer.SetActive(true);
            transform.SetParent(null);
            StopAllCoroutines();
            StartCoroutine(IgnorCollisionForTimeRoutine(thrower, 1f));
        }

        public void MakeFallingDown() {
            _Colliders.ForEach(_ => _.gameObject.layer = LayerMask.NameToLayer(Layers.Names.FallingDownObject));
            FallingDown = true;
        }

        public void IgnoreCollisionForTime(GameObject ignoredObject, float time) {
            StartCoroutine(IgnorCollisionForTimeRoutine(ignoredObject, time));
        }

        private IEnumerator IgnorCollisionForTimeRoutine(GameObject thrower, float time) {
            var throwerColliders = thrower.GetComponentsInChildren<Collider2D>();
            foreach (var col in _Colliders) {
                foreach (var throwerCol in throwerColliders) {
                    Physics2D.IgnoreCollision(throwerCol, col);
                }
            }
            yield return new WaitForSeconds(time);
            foreach (var col in _Colliders) {
                if (col) {
                    foreach (var throwerCol in throwerColliders) {
                        if (throwerCol) {
                            Physics2D.IgnoreCollision(throwerCol, col, false);
                        }
                    }
                }
            }
        }
    }
}