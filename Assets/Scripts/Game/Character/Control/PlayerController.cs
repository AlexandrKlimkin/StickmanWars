using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using Character.MuscleSystem;
using Character.Shooting;
using UnityEngine;
using InputSystemSpace;
using Core.Services.Game;

namespace Character.Control {
    public class PlayerController : MonoBehaviour {

        public int Id;
        //public const float PressTime2HighJump = 0.12f;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private InputKit _InputKit;
        private IAimProvider _AimProvider;

        private Camera _Camera;
        private bool _IsJumping;
        private bool _WallJump;
        private bool _IsWallJumping;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
            _WeaponController = GetComponent<WeaponController>();
        }

        private void Start() {
            //_InputKit = InputConfig.Instance.GetSettings(Id);
            _Camera = Camera.main;
            _AimProvider = _InputKit.Id == 0
                ? (IAimProvider) new MouseAim(_Camera)
                : new JoystickAim(_WeaponController.NearArmShoulder, _MovementController, _InputKit.HorizontalRight, _InputKit.VerticalRight);
        }

        public void Update() {
            if (GameManagerService.GameInProgress && !GameManagerService.MatchStarted)
                return;
            Move();
            Jump();
            Attack();
            ThrowWeapon();
            ThrowVehicle();
        }

        private void Move() {
             var hor = Input.GetAxis(_InputKit.Horizontal);
            var vert = Input.GetAxis(_InputKit.Vertical);
            _MovementController.SetHorizontal(hor);
            _MovementController.SetVertical(vert);
        }

        private void Jump() {
            if (Input.GetKeyDown(_InputKit.Jump)) {
                var fallDown = _MovementController.FallDownPlatform();
                if (!fallDown) {
                    _IsJumping = _MovementController.Jump();
                    if (!_IsJumping) {
                        _IsJumping = _MovementController.WallJump();
                        _WallJump = _IsJumping;
                    }
                    _MovementController.PressJump();
                }
            }

            if (Input.GetKey(_InputKit.Jump)) {
                _MovementController.ProcessHoldJump();
            }

            if (Input.GetKeyUp(_InputKit.Jump)) {
                _IsJumping = false;
                _WallJump = false;
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