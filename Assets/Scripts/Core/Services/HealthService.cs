using Core.Services;
using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Character.Health {
    public class HealthService : ILoadableService, IUnloadableService {
        [Dependency]
        private SignalBus _SignalBus;

        public void Load() {
            _SignalBus.Subscribe<ApplyDamageSignal>(ProcessDamageTake, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public void ProcessDamageTake(ApplyDamageSignal signal) {
            var damage = signal.Damage;
            if (damage.Receiver.Dead)
                return;
            if (damage.InstigatorId != null && damage.InstigatorId == damage.Receiver.OwnerId)
                return;
            damage.Receiver.Health -= damage.Amount;
            damage.Receiver.Health = Mathf.Clamp(damage.Receiver.Health, 0, damage.Receiver.MaxHealth);
            if(damage.Receiver.Health <= 0) {
                //damage.Receiver.Dead = true;
                damage.Receiver.Kill(damage);
                _SignalBus?.FireSignal(new DamageableDeathSignal(damage));
            }
        }
    }
}
