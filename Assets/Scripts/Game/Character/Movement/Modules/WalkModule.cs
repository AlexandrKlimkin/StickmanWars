using Assets.Scripts.Tools;
using Core.Audio;
using System;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Character.Movement.Modules {
    public class WalkModule : MovementModule {
        [Dependency]
        private readonly AudioService _AudioService;

        public float Direction => _WalkData.Direction;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;
        private WalkData _WalkData;

        private WalkParameters _Parameters;
        private float _TargetXVelocity = 0f;
        public float Horizontal => _WalkData.Horizontal;
        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();

        public WalkModule(WalkParameters parameters) : base() {
            this._Parameters = parameters;
        }

        public override void Start() {
            ContainerHolder.Container.BuildUp(this);
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _WalkData = BB.Get<WalkData>();
            CommonData.ObjTransform.GetComponentsInChildren(_SimpleCcds);
            _WalkData.Direction = 1;
        }

        public override void FixedUpdate() {
            //var xVelocity = CommonData.ObjRigidbody.velocity.x;
            var acceleration = _GroundedData.MainGrounded ? _Parameters.GroundAcceleration : _Parameters.AirAcceleration;
            //xVelocity = Mathf.Lerp(xVelocity, _TargetXVelocity, Time.fixedDeltaTime * acceleration);
            //CommonData.ObjRigidbody.velocity = new Vector2(xVelocity, CommonData.ObjRigidbody.velocity.y);

            var force = new Vector2(_TargetXVelocity * acceleration * Time.fixedDeltaTime, 0);
            CommonData.ObjRigidbody.AddForce(force, ForceMode2D.Impulse);
            UpdateSlowDownForce();
        }

        private void UpdateSlowDownForce() {
            var velocity = CommonData.ObjRigidbody.velocity;
            var targetSpeed = _Parameters.Speed;
            var directionalSpeed = Vector2.Dot(CommonData.ObjTransform.right * Mathf.Abs(_TargetXVelocity), velocity);
            var relativeTargetSpeed = targetSpeed / _Parameters.Speed;
            var relativeCurrentSpeed = directionalSpeed / _Parameters.Speed;
            var relativeSpeedDelta = relativeTargetSpeed - relativeCurrentSpeed;
            var slowdownCoeff = Mathf.Clamp01(-relativeSpeedDelta);
            //if (this.GetComponentInParent<VehicleController>().IsLocalPlayerVehicle)
            //    Debug.LogError(slowdownCoeff);
            CommonData.ObjRigidbody.AddForce(CommonData.ObjTransform.right.ToVector2() * Mathf.Sign(_TargetXVelocity) * -slowdownCoeff * 10, ForceMode2D.Impulse);
        }

        public override void Update() {
            SetDirection();
            _TargetXVelocity = 0f;
            if (_WalkData.Horizontal > 0.15f) {
                _TargetXVelocity = _Parameters.Speed;
                ProcessRunSound(true);
            } else if (_WalkData.Horizontal < -0.15f) {
                _TargetXVelocity = -_Parameters.Speed;
                ProcessRunSound(true);
            } else {
                _WalkData.Horizontal = 0;
                ProcessRunSound(false);
            }
            if (CommonData.WeaponController.MeleeAttacking) {
                _TargetXVelocity = 0;
            }
        }

        private AudioEffect _RunSoundEffect;
        private void ProcessRunSound(bool moving) {
            if (string.IsNullOrEmpty(_Parameters.RunSoundEffectName) || !_GroundedData.Grounded || !moving) {
                StopIfHasEffect();
                return;
            }
            if (_RunSoundEffect == null)
                _RunSoundEffect = _AudioService.PlaySound3D(_Parameters.RunSoundEffectName, false, false, CommonData.MovementController.transform.position);
            else
                _RunSoundEffect.transform.position = CommonData.ObjTransform.position;
        }

        private void StopIfHasEffect() {
            if (_RunSoundEffect != null) {
                _RunSoundEffect.Stop(false);
                _RunSoundEffect = null;
            }
        }

        public void SetHorizontal(float hor) {
            _WalkData.Horizontal = hor;
            Mathf.Clamp(_WalkData.Horizontal, -1f, 1f);
        }

        private void SetDirection() {
            if (_WalkData.Horizontal == 0)
                return;
            var newDir = _WalkData.Horizontal > 0 ? 1 : -1;
            CommonData.MovementController.ChangeDirection(newDir);
        }
    }

    [Serializable]
    public class WalkParameters {
        public float Speed = 1f;
        public float GroundAcceleration = 1f;
        public float AirAcceleration = 1f;
        public Transform IkTransform;
        public string RunSoundEffectName;
    }
}