using Assets.Scripts.Tools;
using Character.Shooting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI {
    public class ShootTask : UnitTask {
        private bool _Pressed;
        private bool _Released;
        private Transform _Target;
        private bool _TargetIsVisible;
        private float _TargetVisibleTimer;
        private float _WeaponHoldTimer;
        private int _CharacterLayer = LayerMask.NameToLayer(Layers.Names.Character);
        private float _TargetVisibleTime;


        private ContactFilter2D _ContactViewFilter = new ContactFilter2D() {
            useTriggers = false,
            useLayerMask = true,
            layerMask = Layers.Masks.NoCharacter,
        };

        public ShootTask(float TargetVisibleTime) {
            this._TargetVisibleTime = TargetVisibleTime;
        }

        public override void Init() {
            base.Init();
            UpdatedTask = true;
        }

        public override TaskStatus Run() {
            if (WeaponController.MainWeapon == null) {
                _Pressed = false;
                _WeaponHoldTimer = 0;
                return TaskStatus.Failure;
            }
            FindTarget();
            ProcessVisible();
            Fire();
            return TaskStatus.Running;
        }

        protected override void LateUpdate() {
            base.LateUpdate();
            Aim();
        }

        private void FindTarget() {
            if (CharacterUnit.Characters.Count < 2) {
                _Target = null;
                return;
            }
            CharacterUnit closest = null;
            float distToClosest = float.MaxValue;
            foreach (var unit in CharacterUnit.Characters) {
                if (unit == CharacterUnit)
                    continue;
                var dist = (unit.transform.position - CharacterUnit.transform.position).sqrMagnitude;
                if (dist < distToClosest) {
                    distToClosest = dist;
                    closest = unit;
                }
            }
            _Target = closest.transform;
        }

        private void ProcessVisible() {
            if (!_Target) {
                _TargetIsVisible = false;
                _TargetVisibleTimer = 0f;
                return;
            }
            var armPos = WeaponController.NearArmWeaponTransform.position;
            var hits = Physics2D.LinecastAll(armPos, _Target.position.ToVector2() + new Vector2(0, 14f), Layers.Masks.BotVisionMask);
            _TargetIsVisible = true;
            foreach (var hit in hits) {
                if (!hit.collider.isTrigger) {
                    _TargetIsVisible = false;
                    _TargetVisibleTimer = 0f;
                }
            }
            if (_TargetIsVisible)
                _TargetVisibleTimer += Time.deltaTime;
        }

        private void Fire() {
            var processorType = WeaponController.MainWeapon.InputProcessor.GetType();

            if (_TargetIsVisible && _TargetVisibleTimer > _TargetVisibleTime) {
                if (!_Pressed)
                    WeaponController.PressFire();
                WeaponController.HoldFire();
                _WeaponHoldTimer += Time.deltaTime;
                _Pressed = true;
            }
            if (processorType == typeof(SingleShotProcessor)) {
                _Pressed = false;
            } else if (processorType == typeof(FireForceProcessor)) {
                if(_WeaponHoldTimer > 1f) {
                    WeaponController.ReleaseFire();
                    _WeaponHoldTimer = 0f;
                }
            }
        }

        private void Aim() {
            if (_Target) {
                CharacterUnit.WeaponController.SetAimPosition(_Target.position.ToVector2() + new Vector2(0, 14f));
            } else {

            }
        }
    }
}
