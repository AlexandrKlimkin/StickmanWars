using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using Character.MuscleSystem;
using Character.Shooting;
using UnityEngine;
using InputSystem;

namespace Character.Control {
    public class PlayerController : MonoBehaviour {

        public int Id;
        public const float PressTime2HighJump = 0.12f;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private InputKit _InputKit;
        private IAimProvider _AimProvider;

        private Camera _Camera;
        private bool _IsJumping;
        private bool _WallJump;
        private bool _IsWallJumping;
        private float _JumpTimer;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
            _WeaponController = GetComponent<WeaponController>();
        }

        private void Start() {
            _InputKit = InputConfig.Instance.GetSettings(Id);
            _Camera = Camera.main;
            _AimProvider = _InputKit.Id == 0
                ? (IAimProvider) new MouseAim(_Camera)
                : new JoystickAim(_WeaponController.NearArmShoulder, _MovementController, _InputKit.HorizontalRight, _InputKit.VerticalRight);
        }

        public void Update() {
            Move();
            Jump();
            Attack();
            ThrowWeapon();
            ThrowVehicle();
        }

        private void Move() {
            var hor = Input.GetAxis(_InputKit.Horizontal);
            _MovementController.SetHorizontal(hor);
        }

        private void Jump() {
            if (Input.GetKeyDown(_InputKit.Jump)) {
                _IsJumping = _MovementController.Jump();
                if (!_IsJumping) {
                    _IsJumping = _MovementController.WallJump();
                    _WallJump = _IsJumping;
                }

                if (_IsJumping)
                    _JumpTimer = 0;
                _MovementController.PressJump();
            }

            if (Input.GetKey(_InputKit.Jump)) {
                if (_IsJumping) {
                    _JumpTimer += Time.deltaTime;
                    if (_JumpTimer > PressTime2HighJump) {
                        _MovementController.ContinueJump();
                        _IsJumping = false;
                        _WallJump = false;
                        _JumpTimer = 0;
                    }
                }
                //else {
                //    _IsJumping = _MovementController.Jump();
                //    if (!_IsJumping) {
                //        _IsJumping = _MovementController.WallJump();
                //        _WallJump = _IsJumping;
                //    }
                //    if (_IsJumping)
                //        _JumpTimer = 0;
                //}
                _MovementController.HoldJump();
            }

            if (Input.GetKeyUp(_InputKit.Jump)) {
                _IsJumping = false;
                _WallJump = false;
                _JumpTimer = 0;
                _MovementController.ReleaseJump();
            }
        }

        private void Attack() {
            if (Input.GetKeyDown(_InputKit.Attack1))
            {
                _WeaponController.PressFire();
            }
            if (Input.GetKey(_InputKit.Attack1)) {
                _WeaponController.HoldFire();
            }
            if (Input.GetKeyUp(_InputKit.Attack1)) {
                _WeaponController.ReleaseFire();
            }
        }

        private void ThrowWeapon() {
            if (Input.GetKeyDown(_InputKit.ThrowOutWeapon)) {
                _WeaponController.ThrowOutMainWeapon();
            }
        }

        private void ThrowVehicle()
        {
            if (Input.GetKeyDown(_InputKit.ThrowOutVehicle)) {
                _WeaponController.ThrowOutVehicle();
            }
        }

        public void LateUpdate() {
            if (_WeaponController.HasMainWeapon)
                _WeaponController.SetAimPosition(_AimProvider.AimPoint);
        }

        private void OnDrawGizmosSelected() {
            if (!Application.isPlaying)
                return;
            Gizmos.color = _AimProvider is MouseAim ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(_AimProvider.AimPoint, 1f);
            Gizmos.DrawLine(_AimProvider.AimPoint, _WeaponController.NearArmShoulder.position);
        }
    }
}