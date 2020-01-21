using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health
{
    public class Damage
    {
        public float Amount;
        public Unit Instigator; //Todo: Id

        public Damage(Unit instigator, float amount)
        {
            this.Instigator = instigator;
            this.Amount = amount;
        }
    }
}
