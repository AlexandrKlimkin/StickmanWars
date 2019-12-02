using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Stickman.MuscleSystem {
    [Serializable]
    public class AttackLegAction : MuscleAction {

        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;
        private Muscle _Chest;

        [Header("Settings")]
        public float LegForce;
        public float HitForce;

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
            _LegDown[_Leg].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            _Leg = _Leg == 0 ? 1 : 0;
            var direction = parameters[0];
            var leg = _LegDown[_Leg];
            var point = _Chest.Rigidbody.position + Vector2.right + Vector2.up * 0.5f;
            var dir = point - _Chest.Rigidbody.position;
            var force = new Vector2(dir.x * direction, dir.y) * LegForce;
            _LegDown[_Leg].BoneCollider.DamageableCollisionEnter += OnHitDamageable;
            leg.AddForce(force);
        }

        private void OnHitDamageable(Collision2D collision) {
            _LegDown[_Leg].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            var contact = collision.contacts[0]; //ToDo
            collision.rigidbody.AddForceAtPosition(-contact.normal * HitForce, contact.point);
        }
    }
}
