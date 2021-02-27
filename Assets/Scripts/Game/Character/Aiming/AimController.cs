using Character.Movement;
using UnityEngine;
using Character.Shooting;
using Core.Services.Game;
using KlimLib.ResourceLoader;
using UnityDI;

namespace Character.Control {
    public class AimController : MonoBehaviour {
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;

        public PlayerActions PlayerActions;

        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private IAimProvider _AimProvider;
        private Camera _Camera;

        private AimViewBase _AimView;

        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            _WeaponController = GetComponent<WeaponController>();
            _MovementController = GetComponent<MovementController>();
        }

        private void Start() {
            _Camera = Camera.main;
            _AimProvider = PlayerActions.Device == null
                ? (IAimProvider)new MouseAimProvider(_Camera, transform)
                : new JoystickAimProvider(_WeaponController.NearArmShoulder, _MovementController, PlayerActions);
            _WeaponController.OnWeaponEquiped += OnWeaponEquiped;
            _WeaponController.OnWeaponThrowed += OnWeaponThrowed;
        }

        private void LateUpdate() {
            if (_WeaponController.HasMainWeapon)
                _WeaponController.SetAimPosition(_AimProvider.AimPoint);
        }

        private void OnWeaponEquiped(Weapon weapon) {
            var aimViewProvider = weapon as IAimViewProvider;
            if (aimViewProvider == null)
                return;
            if(aimViewProvider.AimPrefab == null)
                return;
            _AimView = Instantiate(aimViewProvider.AimPrefab, transform);
            _AimView.transform.localPosition = Vector3.zero;
            _AimView.transform.localRotation = Quaternion.identity;
            _AimView.Setup(_AimProvider, aimViewProvider.AimPositionPoint);
        }

        private void OnWeaponThrowed(Weapon weapon) {
            if(_AimView != null)
                Destroy(_AimView.gameObject);
        }

        private void OnDestroy() {
            _WeaponController.OnWeaponEquiped -= OnWeaponEquiped;
            _WeaponController.OnWeaponThrowed -= OnWeaponThrowed;
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying)
                return;
            Gizmos.color = _AimProvider is MouseAimProvider ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(_AimProvider.AimPoint, 1f);
            Gizmos.DrawLine(_AimProvider.AimPoint, transform.position + new Vector3(0, 13f, 0));
        }
    }
}
