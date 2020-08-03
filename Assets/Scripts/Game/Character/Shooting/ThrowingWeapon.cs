using Assets.Scripts.Tools;
using Character.Shooting.Character.Shooting;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Shooting {
    public class ThrowingWeapon : LongRangeWeapon<ThrowingProjectile, ThrowingProjectileData> {
        public override WeaponInputProcessor InputProcessor => _FireForceProcessor ?? (_FireForceProcessor = new FireForceProcessor(this));
        private FireForceProcessor _FireForceProcessor;

        private float _startTime;
        private bool _throw = false;
        private Vector2 _dir;
        [SerializeField] private Image _throwingSlider;
        [SerializeField] private WeaponConfig _weaponConfigScript;
        [SerializeField] private Rigidbody2D _rb2d;
        public bool CanBePicked { get; private set; }

        protected override bool UseThrowForce => false;

        public override ThrowingProjectileData GetProjectileData() {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = transform.position;
            data.Rotation = transform.rotation;
            return data;
        }

        public override ThrowingProjectile GetProjectile() {
            return GetComponent<ThrowingProjectile>();
        }

        public override void PerformShot() {
            base.PerformShot();
            var projectile = GetProjectile();
            var data = GetProjectileData();
            projectile.Setup(data);
            _dir = PickableItem.Owner.WeaponController.AimPosition;
            _throwingSlider.fillAmount = 0;
            PickableItem.Owner.WeaponController.ThrowOutMainWeapon(data.StartVelocity, -720f);
            _throw = true;
            projectile.Play();
        }

        public void FixedUpdate()
        { 
            if (_throw == true)
            {
                _rb2d.AddForce(_dir.normalized * (_startTime * 700f), ForceMode2D.Impulse);
                _throw = false;
                _startTime = 0;
            }
            
        }
        public void Update()
        {
            if (PickableItem.Owner != null && Input.GetMouseButton(0))
            {
                {
                     GameObject slider = GameObject.FindGameObjectWithTag("ThrowSlider");
                     _throwingSlider = slider.gameObject.GetComponent<Image>();
                }
                _startTime += Time.deltaTime;
                _throwingSlider.fillAmount += 1.0f / _weaponConfigScript.MaxForceTime * Time.deltaTime;
                if (_startTime >= _weaponConfigScript.MaxForceTime)
                {
                    Debug.Log(_startTime);
                    PerformShot();
                }
            }
        }
    }
}
