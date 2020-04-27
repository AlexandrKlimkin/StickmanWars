using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterConstruction {
    public abstract class UnitCostructor
    {
        public abstract CharacterUnit ConstructUnit(string unitId);
    }
}
