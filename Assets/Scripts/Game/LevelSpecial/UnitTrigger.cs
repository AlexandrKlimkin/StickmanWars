using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class UnitTrigger : MonoBehaviour
    {
        private readonly List<Unit> _UnitsInscide = new List<Unit>();

        public bool ContainsUnit()
        {
            return _UnitsInscide.Count > 0;
        }

        public bool ContainsUnit(Unit unit)
        {
            return _UnitsInscide.Contains(unit);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var unit = col.gameObject.GetComponent<Unit>();
            if (unit && !_UnitsInscide.Contains(unit))
            {
                _UnitsInscide.Add(unit);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            var unit = col.gameObject.GetComponent<Unit>();
            if (unit && _UnitsInscide.Contains(unit)) {
                _UnitsInscide.Remove(unit);
            }
        }
    }
}