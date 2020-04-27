using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelSpecial {
    public class UnitTrigger : MonoBehaviour {
        private readonly List<CharacterUnit> _UnitsInscide = new List<CharacterUnit>();
        public IReadOnlyList<CharacterUnit> UnitsInside => _UnitsInscide;

        public bool ContainsUnit() {
            return _UnitsInscide.Count > 0;
        }

        public bool ContainsUnit(CharacterUnit characterUnit) {
            return _UnitsInscide.Contains(characterUnit);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<CharacterUnit>();
            if (!unit || _UnitsInscide.Contains(unit))
                return;
            OnUnitEnterTheTrigger(unit);
        }

        protected virtual void OnTriggerExit2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<CharacterUnit>();
            if (!unit || !_UnitsInscide.Contains(unit))
                return;
            OnUnitExitTheTrigger(unit);
        }

        protected virtual void OnUnitEnterTheTrigger(CharacterUnit characterUnit) {
            _UnitsInscide.Add(characterUnit);
        }

        protected virtual void OnUnitExitTheTrigger(CharacterUnit characterUnit) {
            _UnitsInscide.Remove(characterUnit);
        }
    }
}