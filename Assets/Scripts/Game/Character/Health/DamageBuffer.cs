using KlimLib.SignalBus;
using System.Collections.Generic;
using System.Linq;
using UnityDI;
using UnityEngine;

namespace Character.Health {
    public class DamageBuffer : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public float SummaryBufferedDamage => _SummaryBufferedDamageDict.Sum(_ => _.Value);

        private Queue<DamageBufferData> _Buffer = new Queue<DamageBufferData>();
        private Dictionary<byte, float> _SummaryBufferedDamageDict = new Dictionary<byte, float>();
        private float _SafeTime;
        private IDamageable _Damageable;

        public void Initialize(IDamageable damageable, float safeTime) {
            ContainerHolder.Container.BuildUp(this);
            _SafeTime = safeTime;
            _Damageable = damageable;
            _SignalBus.Subscribe<ApplyDamageSignal>(OnApplyDamageSignal, this);
        }

        private void OnDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
        }

        private void AddToBuffer(DamageBufferData damageBufferData) {
            _Buffer.Enqueue(damageBufferData);
            var instigastorId = damageBufferData.InstigatorId.Value;
            if (_SummaryBufferedDamageDict.ContainsKey(instigastorId))
                _SummaryBufferedDamageDict[instigastorId] += damageBufferData.DamageAmount;
            else
                _SummaryBufferedDamageDict.Add(instigastorId, damageBufferData.DamageAmount);
        }

        private void Update() {
            while (_Buffer.Count > 0 && _Buffer.Peek().Time + _SafeTime < Time.time) {
                var damageData = _Buffer.Dequeue();
                var instigastorId = damageData.InstigatorId.Value;
                if (_SummaryBufferedDamageDict.ContainsKey(instigastorId)) {
                    _SummaryBufferedDamageDict[instigastorId] -= damageData.DamageAmount;
                    if (Mathf.Abs(_SummaryBufferedDamageDict[instigastorId]) < 0.01f) // float error
                        _SummaryBufferedDamageDict.Remove(instigastorId);
                }
            }
        }

        public byte? TopBufferedDamager() {
            if (_SummaryBufferedDamageDict.Count == 0)
                return null;
            var first = _SummaryBufferedDamageDict.First();
            var topDamager = first.Key;
            float topDamage = first.Value;
            foreach (var summaryDamage in _SummaryBufferedDamageDict) {
                if (summaryDamage.Value > topDamage) {
                    topDamage = summaryDamage.Value;
                    topDamager = summaryDamage.Key;
                }
            }
            return topDamager;
        }

        public float SummaryBufferedDamageOnTime(float sec) {
            var currentTime = Time.time;
            return _Buffer.Where(_ => _.Time + sec >= currentTime).Sum(_ => _.DamageAmount);
        }

        private void OnApplyDamageSignal(ApplyDamageSignal signal) {
            if (signal.Damage.Receiver != _Damageable)
                return;
            if(signal.Damage.InstigatorId.HasValue  )
                AddToBuffer(new DamageBufferData() { InstigatorId = signal.Damage.InstigatorId, DamageAmount = signal.Damage.Amount, Time = Time.time });
        }

    }
}
