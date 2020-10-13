using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.BehaviourTree;

namespace Game.AI {
    public class CheckBurstDamageTask : UnitTask {

        private float _BurstTime;
        private float _BurstDamageValue;
        private bool _PercentBurst;


        public CheckBurstDamageTask(float burstTime, float burstDamageValue, bool percentBurst) {
            this._BurstTime = burstTime;
            this._BurstDamageValue = burstDamageValue;
            this._PercentBurst = percentBurst;
        }

        public override TaskStatus Run() {
            if (CharacterUnit.DamageBuffer == null)
                return TaskStatus.Failure;
            var burstDmg = CharacterUnit.DamageBuffer.SummaryBufferedDamageOnTime(_BurstTime);
            if (_PercentBurst) {
                return burstDmg / CharacterUnit.MaxHealth * 100 > _BurstDamageValue ? TaskStatus.Success : TaskStatus.Failure;
            } else {
                return burstDmg > _BurstDamageValue ? TaskStatus.Success : TaskStatus.Failure;
            }
        }
    }
}
