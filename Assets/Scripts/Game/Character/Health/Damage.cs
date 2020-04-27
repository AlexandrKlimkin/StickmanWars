using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Health
{
    public class Damage
    {
        public float Amount;
        public CharacterUnit Instigator; //Todo: Id
        public Vector2 Force;

        public Damage(CharacterUnit instigator, float amount)
        {
            this.Instigator = instigator;
            this.Amount = amount;
        }
    }
}
