using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;
using InputSystem;
using Stickman.Movement;
using Stickman.Shooting;

namespace Stickman.Controllers {
    public class PlayerController : MonoBehaviour {

        public int Id;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private InputKit _InputKit;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
            _WeaponController = GetComponent<WeaponController>();
        }

        private void Start() {
            _InputKit = InputConfig.Instance.GetSettings(Id);
        }

        public void Update() {
            var hor = Input.GetAxis(_InputKit.Horizontal);
            var vert = Input.GetAxis(_InputKit.Vertical);
            _MovementController.SetHorizontal(hor);
            if (Input.GetKeyDown(_InputKit.Attack1))
            {
                _WeaponController.Fire();
            }
            //if (Input.GetKeyDown(_InputKit.Attack2)) {
            //    _MuscleController.AttackLeg();
            //}
            if (Input.GetKeyDown(_InputKit.Jump))
            {
                _MovementController.Jump();
            }
        }
    }
}
