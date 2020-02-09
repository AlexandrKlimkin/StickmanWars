﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour {
        public Animator Animator;
        public Transform IkTransform;
        public Transform PuppetTransform;
        public float Speed = 1f;
        public float GroundAcceleration = 1f;
        public float AirAcceleration = 1f;
        public float NormalGravityScale = 1f;
        public float FallGravityScale = 5f;
        public float WallSlideSpeed;
        [Header("Jumping")]
        public float JumpSpeed = 1000f;
        public float WallJumpSpeed = 1000f;
        public float LowJumpTime = 0.5f;
        public float HighJumpAddTime = 0.5f;

        public List<Sensor> GroundSensors;
        public Sensor MainGroundSensor;
        public List<Sensor> RightSensors;
        public List<Sensor> LeftSensors;

        public Rigidbody2D Rigidbody { get; private set; }
        public int Direction { get; private set; } = 1;

        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();
        private float _Horizontal;
        private float _LastY;

        private bool FallingDown => transform.position.y < _LastY && !IsMainGrounded;
        private bool WallSliding => !IsMainGrounded && (LeftSensors.Any(_=>_.IsTouching) || RightSensors.Any(_ => _.IsTouching));
        private bool LefTouch => LeftSensors.Any(_ => _.IsTouching);
        private bool RightTouch => RightSensors.Any(_ => _.IsTouching);
        private bool IsGrounded => GroundSensors.Any(_ => _.IsTouching) && !WallSliding;
        private bool IsMainGrounded => MainGroundSensor.IsTouching && MainGroundSensor.Distanse < 1f;
        private float _TimeSinceMainGrounded;
        private float MinDistanceToGround => GroundSensors.Min(_ => _.Distanse);

        private float _JumpTimer;

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            GetComponentsInChildren(_SimpleCcds);
            _LastY = transform.position.y;
        }

        private void Update() {
            _LastY = transform.position.y;
            UpdateAnimator();
            Rigidbody.gravityScale = Rigidbody.velocity.y < 0 ? FallGravityScale : NormalGravityScale;
            if (IsMainGrounded)
                _TimeSinceMainGrounded = 0f;
            else
                _TimeSinceMainGrounded += Time.deltaTime;
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
            var acceleration = IsMainGrounded ? GroundAcceleration : AirAcceleration;
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
            Animator.SetBool("Grounded", IsGrounded);
            Animator.SetFloat("DistanseToGround", MinDistanceToGround);
            Animator.SetBool("FallingDown", FallingDown);
            Animator.SetBool("WallSliding", WallSliding);
            Animator.SetFloat("Speed", Mathf.Abs(Rigidbody.velocity.x / 50f));
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
            var newLocalScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            //transform.localScale = newLocalScale;
            IkTransform.localScale = newLocalScale;
            PuppetTransform.localScale = newLocalScale;
            _SimpleCcds.ForEach(_ => _.ReflectNodes());
        }

        public bool Jump() {
            if (IsGrounded && _TimeSinceMainGrounded < 0.3f)
            {
                _JumpTimer = LowJumpTime;
                StopCoroutine(JumpRoutine());
                StartCoroutine(JumpRoutine());
                return true;
            }
            return false;
        }

        public void ContinueJump()
        {
            if(_JumpTimer != 0)
                _JumpTimer += HighJumpAddTime;
        }

        public bool WallJump()
        {
            if (RightTouch)
            {
                var vector = new Vector2(-1, 1).normalized;
                Rigidbody.velocity = vector * WallJumpSpeed;
                return true;
            }
            if (LefTouch)
            {
                var vector = new Vector2(1, 1).normalized;
                Rigidbody.velocity = vector * WallJumpSpeed;
                return true;
            }
            return false;
        }

        public void ContinueWallJump()
        {
            if(_JumpTimer != 0)
                _JumpTimer += HighJumpAddTime;
        }

        private IEnumerator JumpRoutine()
        {
            while (_JumpTimer > 0)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, JumpSpeed);
                _JumpTimer -= Time.deltaTime;
                yield return null;
            }
            _JumpTimer = 0;
        }
    }
}