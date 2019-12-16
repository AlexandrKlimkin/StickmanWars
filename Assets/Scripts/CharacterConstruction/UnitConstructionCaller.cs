using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterConstruction
{
    public class UnitConstructionCaller : MonoBehaviour
    {
        [Button] public bool Create;

        public void OnCreate()
        {
            var constructor = new HumanoidConstructor();
            var unit = constructor.ConstructUnit("White_Punk");
        }
    }
}
