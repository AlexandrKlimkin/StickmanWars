using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour {
        public Animator Animator;
        public Transform IkTransform;
        public float Speed = 1f;
        public float GroundAcceleration = 1f;
        public float AirAcceleration = 1f;
        public float JumpForce = 1000f;
        public float WallJumpForce = 1000f;
        public float WallSlideSpeed;
        //public float TestForce = 1000f;

        public Sensor GroundSensor;
        public List<Sensor> RightSensors;
        public List<Sensor> LeftSensors;

        public Rigidbody2D Rigidbody { get; private set; }
        public int Direction { get; private set; } = 1;

        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();
        private float _Horizontal;
        private float _LastY;
        private bool _FallingDown = false;

        private bool WallSliding => !GroundSensor.IsTouching && (LeftSensors.Any(_=>_.IsTouching) || RightSensors.Any(_ => _.IsTouching));
        private bool LefTouch => LeftSensors.Any(_ => _.IsTouching);
        private bool RightTouch => RightSensors.Any(_ => _.IsTouching);

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            GetComponentsInChildren(_SimpleCcds);
            _LastY = transform.position.y;
        }

        private void Update() {
            _FallingDown = transform.position.y < _LastY && !GroundSensor.IsTouching;
            _LastY = transform.position.y;
            UpdateAnimator();

            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    Rigidbody.AddForce(new Vector2(TestForce, 0));
            //}
        }

        private void FixedUpdate()
        {
            SetDirection();
            var targetXVelocity = 0f;
            var xVelocity = Rigidbody.velocity.x;
            if (_Horizontal > 0)
                targetXVelocity = Speed;
            if (_Horizontal < 0)
                targetXVelocity = -Speed;
            var acceleration = GroundSensor.IsTouching ? GroundAcceleration : AirAcceleration;
            xVelocity = Mathf.Lerp(xVelocity, targetXVelocity, Time.fixedDeltaTime * acceleration);
            Rigidbody.velocity = new Vector2(xVelocity, Rigidbody.velocity.y);

            if (WallSliding)
            {
                if(Rigidbody.velocity.y < -WallSlideSpeed)
                    Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, -WallSlideSpeed);
            }
        }

        private void UpdateAnimator()
        {
            Animator.SetFloat("Horizontal", Mathf.Abs(_Horizontal));
            Animator.SetBool("Grounded", GroundSensor.IsTouching);
            Animator.SetFloat("DistanseToGround", GroundSensor.Distanse);
            Animator.SetBool("FallingDown", _FallingDown);
            Animator.SetBool("WallSliding", WallSliding);
        }

        public void SetHorizontal(float hor) {
            _Horizontal = hor;
            Mathf.Clamp(_Horizontal, -1f, 1f);
        }

        private void SetDirection()
        {
            if (_Horizontal != 0)
            {
                var newDir = _Horizontal > 0 ? 1 : -1;
                if (LefTouch)
                    newDir = -1;
                else if (RightTouch)
                    newDir = 1;
                if (Direction != newDir)
                    ChangeDirection(newDir);
            }
        }

        private void ChangeDirection(int newDir) {
            Direction = newDir;
            var localScale = transform.localScale;
            IkTransform.localScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            _SimpleCcds.ForEach(_ => _.ReflectNodes());
        }

        public void Jump() {
            if (GroundSensor.IsTouching)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, JumpForce);
                return;
            }
            if (RightTouch)
            {
                var vector = new Vector2(-1, 1).normalized;
                Rigidbody.velocity = vector * WallJumpForce;
            }
            if (LefTouch)
            {
                var vector = new Vector2(1, 1).normalized;
                Rigidbody.velocity = vector * WallJumpForce;
            }
        }

        public void JumpOffTheWall() { }
    }
}