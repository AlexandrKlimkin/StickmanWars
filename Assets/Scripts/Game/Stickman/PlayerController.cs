using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using Character.MuscleSystem;
using Character.Shooting;
using UnityEngine;
using InputSystem;

namespace Character.Controllers {
    public class PlayerController : MonoBehaviour {

        public int Id;
        public float PressTime2HighJump;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private InputKit _InputKit;

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
        }

        public void Update() {
            var hor = Input.GetAxis(_InputKit.Horizontal);
            var vert = Input.GetAxis(_InputKit.Vertical);
            _MovementController.SetHorizontal(hor);
            if (Input.GetKeyDown(_InputKit.Attack1))
            {
                _WeaponController.Fire();
            }
            if (Input.GetKeyDown(_InputKit.Jump))
             {
                _IsJumping = _MovementController.Jump();
                if (!_IsJumping)
                {
                    _IsJumping = _MovementController.WallJump();
                    _WallJump = _IsJumping;
                }
                if(_IsJumping)
                    _JumpTimer = 0;
            }
            if (Input.GetKey(_InputKit.Jump))
            {
                if (_IsJumping)
                {
                    _JumpTimer += Time.deltaTime;
                    if (_JumpTimer > PressTime2HighJump)
                    {
                        _MovementController.ContinueJump();
                        _IsJumping = false;
                        _WallJump = false;
                        _JumpTimer = 0;
                    }
                }
            }
            if (Input.GetKeyUp(_InputKit.Jump))
            {
                _IsJumping = false;
                _WallJump = false;
                _JumpTimer = 0;
            }
        }

        public void LateUpdate() {
            if (_InputKit.Id == 1)
                _WeaponController.SetWeaponedHandPosition(_Camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
