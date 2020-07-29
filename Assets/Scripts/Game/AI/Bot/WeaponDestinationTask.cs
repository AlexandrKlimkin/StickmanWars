using Character.Shooting;
using System.Linq;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI {
    public class WeaponDestinationTask : UnitTask {
        private MovementData _MovementData;

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run() {
            if(WeaponController.HasMainWeapon)
                return TaskStatus.Failure;
            var weapon = FindClosestWeapon();
            if (weapon == null)
                return TaskStatus.Failure;
            _MovementData.TargetPos = weapon.transform.position;
            _MovementData.DestinationType = DestinationType.Weapon;
            return TaskStatus.Success;
        }

        private Weapon FindClosestWeapon() {
            var weapons = WeaponsInfoContainer.AllWeapons.Where(_ => _.ItemType == ItemType.Weapon && _.PickableItem.Owner == null  && !_.WeaponView.FallingDown).ToList();
            if (weapons.Count == 0)
                return null;
            var closestWeapon = weapons.First();
            float closestMagnitude = float.PositiveInfinity; 
            foreach(var weapon in weapons) {
                var magnitude = (weapon.transform.position - CharacterUnit.transform.position).sqrMagnitude;
                if (magnitude < closestMagnitude) {
                    closestMagnitude = magnitude;
                    closestWeapon = weapon;
                }
            }
            return closestWeapon;
        }
    }
}
