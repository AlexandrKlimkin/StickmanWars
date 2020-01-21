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
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private InputKit _InputKit;

        private Camera _Camera;

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
            //if (Input.GetKeyDown(_InputKit.Attack2)) {
            //    _MuscleController.AttackLeg();
            //}
            if (Input.GetKeyDown(_InputKit.Jump))
            {
                _MovementController.Jump();
            }
        }

        public void LateUpdate()
        {
            if(_InputKit.Id == 1)
                _WeaponController.SetWeaponedHandPosition(_Camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
