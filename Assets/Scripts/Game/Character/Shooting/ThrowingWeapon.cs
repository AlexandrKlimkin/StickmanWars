﻿using Assets.Scripts.Tools;
using Character.Shooting.Character.Shooting;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Character.Shooting {
    public class ThrowingWeapon : LongRangeWeapon<ThrowingProjectile, ThrowingProjectileData> {
        public override WeaponInputProcessor InputProcessor => _FireForceProcessor ?? (_FireForceProcessor = new FireForceProcessor(this));
        private FireForceProcessor _FireForceProcessor;

        private float _startTime;
        private bool _throw = false;
        private Vector2 _dir;
        [SerializeField] private GrenadeProjectile _grenadeScript;
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
            PickableItem.Owner.WeaponController.ThrowOutMainWeapon(data.StartVelocity, -720f);
            _throw = true;
            projectile.Play();
        }

        public void FixedUpdate()
        { 
            if (_throw == true)
            {
             _rb2d.AddForce(_dir.normalized * (_startTime * 500f), ForceMode2D.Impulse);
                _throw = false;
                _startTime = 0;
            }
            
        }
        public void Update()
        {
            if (PickableItem.Owner != null && Input.GetMouseButton(0))
            {              
                _startTime += Time.deltaTime; 
                if (_startTime >= _grenadeScript.Timer)
                {
                    Debug.Log(_startTime);
                    PerformShot();
                }
            }
        }
    }
}
