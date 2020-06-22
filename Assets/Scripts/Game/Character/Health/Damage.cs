using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health {
    public class Damage {
        public float Amount;
        public byte? InstigatorId;
        public IDamageable Receiver;

        public Damage(byte? instigator, IDamageable receiver, float amount) {
            this.InstigatorId = instigator;
            this.Receiver = receiver;
            this.Amount = amount;
        }
    }

}
