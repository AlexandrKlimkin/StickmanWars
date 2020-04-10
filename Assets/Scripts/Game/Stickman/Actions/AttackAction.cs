using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools.Unity;

namespace Character.MuscleSystem {
    [Serializable]
    public class AttackAction : MuscleAction {

        private List<Muscle> _ArmUp;
        private List<Muscle> _ArmDown;
        private Muscle _Chest;

        private Coroutine _HitCoroutine;

        [Header("Settings")]
        public float HandForce;
        public float HitForce;
        public float CdTime = 0.2f;

        private float _CdTime = 0;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _ArmDown = muscles.Where(_ => _.MuscleType == MuscleType.ArmDown).ToList();
            _ArmUp = muscles.Where(_=> _.MuscleType == MuscleType.ArmUp).ToList();
            _Chest = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Chest);
        }

        [Header("Debug")]
        [SerializeField]
        private int _Arm = 0;

        public override void UpdateAction(params float[] parameters) {
            if (_CdTime > Time.time)
                return;
            _Arm = _Arm == 0 ? 1 : 0;
            var direction = parameters[0];
            var arm = _ArmDown[_Arm];
            var point = _Chest.Rigidbody.position + Vector2.right;
            var dir = point - _Chest.Rigidbody.position;
            var force = dir * direction * HandForce;
            arm.AddForce(force);
            //if(_HitCoroutine != null)
            //    UnityEventProvider.Instance.StopCoroutine(_HitCoroutine);
            //_HitCoroutine = UnityEventProvider.Instance.StartCoroutine(ApplyForceRoutine(_Arm));
            _CdTime = Time.time + CdTime;
        }

        private IEnumerator ApplyForceRoutine(int arm) {
            _ArmDown[arm].BoneCollider.DamageableCollisionEnter += OnHitDamageable;
            yield return new WaitForFixedUpdate();
            _ArmDown[arm].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
        }

        private void OnHitDamageable(Collision2D collision) {
            _ArmDown[_Arm].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            //if(_HitCoroutine != null)
            //    UnityEventProvider.Instance.StopCoroutine(_HitCoroutine);
            var contact = collision.contacts[0]; //ToDo
            collision.rigidbody.AddForceAtPosition(-contact.normal * HitForce, contact.point);
        }
    }
}
