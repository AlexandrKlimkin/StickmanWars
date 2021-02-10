using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health {
    public class Damage {
        public float Amount;
        public byte? InstigatorId;
        public IDamageable Receiver;
        public bool InstantKill;
        public Vector2? DamagePos;
        public Vector2? DamageForce;

        public Damage(byte? instigator, IDamageable receiver, float amount, bool instantKill = false) {
            this.InstigatorId = instigator;
            this.Receiver = receiver;
            this.Amount = amount;
            this.InstantKill = instantKill;
        }

        public Damage(byte? instigator, IDamageable receiver, float amount, Vector2 DamagePos, Vector2 DamageForce, bool instantKill = false) {
            this.InstigatorId = instigator;
            this.Receiver = receiver;
            this.Amount = amount;
            this.InstantKill = instantKill;
            this.DamagePos = DamagePos;
            this.DamageForce = DamageForce;
        }

    }

}
