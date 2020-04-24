using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class UnitTrigger : MonoBehaviour
    {
        private readonly List<Unit> _UnitsInscide = new List<Unit>();
        public IReadOnlyList<Unit> UnitsInside => _UnitsInscide;

        public bool ContainsUnit()
        {
            return _UnitsInscide.Count > 0;
        }

        public bool ContainsUnit(Unit unit)
        {
            return _UnitsInscide.Contains(unit);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            var unit = col.gameObject.GetComponent<Unit>();
            if (!unit || _UnitsInscide.Contains(unit))
                return;
            OnUnitEnterTheTrigger(unit);
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            var unit = col.gameObject.GetComponent<Unit>();
            if (!unit || !_UnitsInscide.Contains(unit))
                return;
            OnUnitExitTheTrigger(unit);
        }

        public virtual void OnUnitEnterTheTrigger(Unit unit) {
            _UnitsInscide.Add(unit);
        }

        public virtual void OnUnitExitTheTrigger(Unit unit) {
            _UnitsInscide.Remove(unit);
        }
    }
}