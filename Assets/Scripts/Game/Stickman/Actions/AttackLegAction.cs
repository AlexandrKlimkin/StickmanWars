using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools.Unity;

namespace Character.MuscleSystem {
    [Serializable]
    public class AttackLegAction : MuscleAction {

        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;
        private Muscle _Chest;

        private Coroutine _HitCoroutine;

        [Header("Settings")]
        public float LegForce;
        public float HitForce;
        public float CdTime = 0.2f;
        public float DisableHitMusclesTime;
        public AnimationCurve DisableHitMusclesCurve;

        private float _CdTime = 0;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _LegDown = muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
            _LegUp = muscles.Where(_=> _.MuscleType == MuscleType.LegUp).ToList();
            _Chest = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Chest);
        }

        [Header("Debug")]
        [SerializeField]
        private int _Leg = 0;
        
        public override void UpdateAction(params float[] parameters) {
            if (_CdTime > Time.time)
                return;
            _Leg = _Leg == 0 ? 1 : 0;
            var direction = parameters[0];
            var leg = _LegDown[_Leg];
            var point = _Chest.Rigidbody.position + Vector2.right + Vector2.up * 0.5f;
            var dir = point - _Chest.Rigidbody.position;
            var force = new Vector2(dir.x * direction, dir.y) * LegForce;
            leg.AddForce(force);
            if (_HitCoroutine != null)
                UnityEventProvider.Instance.StopCoroutine(_HitCoroutine);
            _HitCoroutine = UnityEventProvider.Instance.StartCoroutine(ApplyForceRoutine(_Leg));
            _CdTime = Time.time + CdTime;
        }

        private IEnumerator ApplyForceRoutine(int leg) {
            _LegDown[leg].BoneCollider.DamageableCollisionEnter += OnHitDamageable;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            _LegDown[leg].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
        }

        private void OnHitDamageable(Collision2D collision) {
            _LegDown[_Leg].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            if (_HitCoroutine != null)
                UnityEventProvider.Instance.StopCoroutine(_HitCoroutine);
            var contact = collision.contacts[0]; //ToDo
            collision.rigidbody.AddForceAtPosition(-contact.normal * HitForce, contact.point);
            collision.gameObject.GetComponentInParent<MuscleController>()?.DisableMuscleForce(DisableHitMusclesTime, DisableHitMusclesCurve);
        }
    }
}
