using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterConstruction {
    public abstract class UnitCostructor
    {
        public abstract Unit ConstructUnit(string unitId, out Vector3 downOffset);
    }
}
