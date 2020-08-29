using Character.Health;
using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Character.Health {
    public class HealthDamageable : MonoBehaviour, IDamageable {
        [Dependency]
        private readonly SignalBus _SignalBus;

        [SerializeField]
        private float _MaxHealth;
        public byte? OwnerId => null;
        public float MaxHealth => _MaxHealth;
        public float Health { get; set; }
        public float NormilizedHealth => Health / MaxHealth;
        public event Action OnDeath;
        public bool DestroyOnDeath = true;

        public bool Dead { get; set; }
        public Collider2D Collider { get; set; }

        private void Awake() {
            Collider = GetComponentInChildren<Collider2D>();
            Health = MaxHealth;
        }

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
        }

        public void ApplyDamage(Damage damage) {
            _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        }

        public void Kill(Damage damage) {
            if(DestroyOnDeath)
                Destroy(gameObject);
            OnDeath?.Invoke();
        }
    }
}
