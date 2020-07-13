using Character.Health;
using Character.Shooting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.CloseCombat {
    public class MeleeAttack : Weapon {
        public override ItemType ItemType => ItemType.MeleeAttack;

        public override WeaponInputProcessor InputProcessor => _SingleAttackProcessor ?? (_SingleAttackProcessor = new SingleShotProcessor(this));
        private SingleShotProcessor _SingleAttackProcessor;

        private Coroutine _MeleeAttackRoutine = null;

        public bool Attacking { get; private set; }

        public Vector2[] RecoilForce;
        public Collider2D DamageTrigger;
        public CharacterUnit Owner;

        private float _AttackTimer = 0;
        private void Update() {
            Attacking = _AttackTimer > 0;
            _AttackTimer -= Time.deltaTime;
            if (_AttackTimer < 0)
                _AttackTimer = 0;
        }

        public override void PerformShot() {
            _AttackTimer += 0.25f;
            if (_AttackTimer > 0.5f)
                _AttackTimer = 0.5f;
        }

        public override void PickUp(CharacterUnit unit) {
            base.PickUp(unit);
            Owner = unit;
        }

        public override void ThrowOut(CharacterUnit owner, Vector2? throwForce = null, float? angularVel = null) {
            base.ThrowOut(owner, throwForce, angularVel);
            Owner = null;
        }

        public virtual void AddForce1() {
            AddForce(RecoilForce[0]);
        }

        public virtual void AddForce2() {
            AddForce(RecoilForce[1]);
        }

        private void AddForce(Vector2 force) {
            if (Owner != null)
                Owner.Rigidbody2D.AddForce(new Vector2(Owner.MovementController.Direction * force.x, force.y));
        }

        private ContactFilter2D _Filter = new ContactFilter2D { useTriggers = false, useLayerMask = false };
        public void TakeDamage() {
            List<Collider2D> hits = new List<Collider2D>();
            var resultsAmount = DamageTrigger.OverlapCollider(_Filter, hits);
            var dmgbls = new List<IDamageable>();
            var rbs = new List<Rigidbody2D>();
            if(hits != null && hits.Count > 0) {
                foreach(var hit in hits) {
                    var rb = hit.attachedRigidbody;
                    if(rb != null) { //ToDo: no rb in dmageable??
                        if (!rbs.Contains(rb) && rb != Owner.Rigidbody2D) {
                            rbs.Add(rb);
                        }
                        var dmgbl = rb.GetComponent<IDamageable>();
                        if (!dmgbls.Contains(dmgbl) && dmgbl != Owner)
                            dmgbls.Add(dmgbl);
                    }
                }
            }
            if(rbs.Count > 0) {
                foreach(var rb in rbs) {
                    rbs.ForEach(_ => _.AddForce(Vector2.right * Owner.MovementController.Direction * Stats.HitForce));
                }
            }
            if (dmgbls.Count > 0) {
                dmgbls.ForEach(_ => _.ApplyDamage(new Damage(Owner?.OwnerId, _, Stats.Damage)));

            }
        }

        private IEnumerator WaitForSecondsMeleeAttack(float sec) {
            Attacking = true;
            yield return new WaitForSeconds(sec);
            Attacking = false;
        }
    }
}
