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
        public PlayerActions PlayerActions;
        private WeaponController _WeaponController;
        private MovementController _MovementController;

        private bool _IsJumping;
        private bool _WallJump;
        private bool _IsWallJumping;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
            _WeaponController = GetComponent<WeaponController>();
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
    }
}