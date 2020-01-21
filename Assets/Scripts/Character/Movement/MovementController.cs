using System.Collections.Generic;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour {
        public Animator Animator;
        public float Speed = 1f;
        public float JumpForce = 1000f;

        public Rigidbody2D Rigidbody { get; private set; }
        public IGroundSensor GroundSensor { get; private set; }

        public int Direction { get; private set; } = 1;

        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();
        private float _Horizontal;
        private float _LastY;
        private bool _FallingDown = false;

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            GroundSensor = GetComponentInChildren<IGroundSensor>();
            GetComponentsInChildren(_SimpleCcds);
            _LastY = transform.position.y;
        }

        private void Update() {
            _FallingDown = transform.position.y < _LastY && !GroundSensor.IsGrounded;
            _LastY = transform.position.y;
            UpdateAnimator();
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

        private void UpdateAnimator()
        {
            Animator.SetFloat("Horizontal", Mathf.Abs(_Horizontal));
            Animator.SetBool("Grounded", GroundSensor.IsGrounded);
            Animator.SetFloat("DistanseToGround", GroundSensor.DistanseToGround);
            Animator.SetBool("FallingDown", _FallingDown);
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