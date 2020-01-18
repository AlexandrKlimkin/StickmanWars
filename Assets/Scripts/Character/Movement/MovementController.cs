using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Movement {
    public class MovementController : MonoBehaviour {
        public Animator Animator;
        public float Speed = 1f;
        public float JumpForce = 1000f;

        public Rigidbody2D Rigidbody { get; private set; }
        public IGroundSensor GroundSensor { get; private set; }

        public int Direction { get; private set; } = 1;

        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();
        private float _Horizontal;

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            GroundSensor = GetComponentInChildren<IGroundSensor>();
            GetComponentsInChildren(_SimpleCcds);
        }

        private void Update() {
            Animator.SetFloat("Horizontal", Mathf.Abs(_Horizontal));
        }

        private void FixedUpdate() {
            if (_Horizontal != 0)
            {
                var newDir = _Horizontal > 0 ? 1 : -1;
                if (Direction != newDir)
                    ChangeDirection(newDir);
                var delta = Speed * Direction * Time.fixedDeltaTime;
                Rigidbody.position += new Vector2(delta, 0);
            }
        }

        public void SetHorizontal(float hor) {
            _Horizontal = hor;
            Mathf.Clamp(_Horizontal, -1f, 1f);
        }

        private void ChangeDirection(int newDir) {
            Direction = newDir;
            var localScale = transform.localScale;
            transform.localScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            _SimpleCcds.ForEach(_ => _.ReflectNodes());
        }

        public void Jump() {
            if (!GroundSensor.IsGrounded)
                return;
            Rigidbody.AddForce(new Vector2(0, JumpForce));
        }

    }
}