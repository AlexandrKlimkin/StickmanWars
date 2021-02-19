using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using Character.MuscleSystem;
using Character.Shooting;
using UnityEngine;
using Core.Services.Game;
using InControl;

namespace Character.Control {
    public class PlayerController : MonoBehaviour {
        public int Id;
        //public InputDevice InputDevice;
        public PlayerActions PlayerActions;
        //public const float PressTime2HighJump = 0.12f;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
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
            _Camera = Camera.main;
            _AimProvider = PlayerActions.Device == null
                ? (IAimProvider)new MouseAimProvider(_Camera, transform)
                : new JoystickAimProvider(_WeaponController.NearArmShoulder, _MovementController, PlayerActions);
            //_AimProvider = new JoystickAim(_WeaponController.NearArmShoulder, _MovementController, PlayerActions);
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
            var hor = PlayerActions.Move.Value.x;
            var vert = PlayerActions.Move.Value.y;
            _MovementController.SetHorizontal(hor);
            _MovementController.SetVertical(vert);
        }

        private void Jump() {
            if (PlayerActions.Jump.WasPressed) {
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

            if (PlayerActions.Jump) {
                _MovementController.ProcessHoldJump();
            }

            if (PlayerActions.Jump.WasReleased) {
                _IsJumping = false;
                _WallJump = false;
                _MovementController.ReleaseJump();
            }
        }

        private void Attack() {
            if (PlayerActions.Fire.WasPressed) {
                _WeaponController.PressFire();
            }
            if (PlayerActions.Fire) {
                _WeaponController.HoldFire();
            }
            if (PlayerActions.Fire.WasReleased) {
                _WeaponController.ReleaseFire();
            }
        }

        private void ThrowWeapon() {
            if (PlayerActions.ThrowOutWeapon.WasPressed) {
                _WeaponController.ThrowOutMainWeapon();
            }
        }

        private void ThrowVehicle() {
            if (PlayerActions.ThrowOutVehicle.WasPressed) {
                _WeaponController.ThrowOutVehicle();
            }
        }

        public void LateUpdate() {
            if (_WeaponController.HasMainWeapon)
                _WeaponController.SetAimPosition(_AimProvider.AimPoint);
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying)
                return;
            Gizmos.color = _AimProvider is MouseAimProvider ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(_AimProvider.AimPoint, 1f);
            Gizmos.DrawLine(_AimProvider.AimPoint, transform.position + new Vector3(0,13f,0));
        }
    }
}