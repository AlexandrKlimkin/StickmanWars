﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health {
    public class Damage {
        public float Amount;
        public byte? InstigatorId;
        public IDamageable Receiver;
        public bool InstantKill;

        public Damage(byte? instigator, IDamageable receiver, float amount, bool instantKill = false) {
            this.InstigatorId = instigator;
            this.Receiver = receiver;
            this.Amount = amount;
            this.InstantKill = instantKill;
        }

    }

}
