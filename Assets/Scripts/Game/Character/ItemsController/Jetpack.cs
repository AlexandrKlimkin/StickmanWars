using Character.Shooting;
using Tools.VisualEffects;
using UnityEngine;

namespace Items.Jetpack {
    public class Jetpack : Weapon {
        [SerializeField]
        private string _RocketEffectName;
        [SerializeField]
        private Transform _RocketEffectTransform;
        public override ItemType ItemType => ItemType.Vehicle;
        public override WeaponReactionType WeaponReaction => WeaponReactionType.Jump;
        private AutoFireProcessor _AutoFireProcessor;
        private AttachedParticleEffect _RocketEffect;

        protected override void Start() {
            base.Start();
            InputProcessor.OnProcessHold += OnProcessHold;
            InputProcessor.OnProcessRelease += OnProcessRelease;
        }

        public override void PerformShot() {
            base.PerformShot();
            AddRecoil(Vector2.up);
        }

        private void AddRecoil(Vector2 direction) {
            PickableItem.Owner.Rigidbody2D.AddForce(direction * Stats.RecoilForce);
        }

        private void OnProcessHold(int magazine) {
            if (_RocketEffect == null) {
                _RocketEffect = VisualEffect.GetEffect<AttachedParticleEffect>(_RocketEffectName);
                _RocketEffect.SetTarget(_RocketEffectTransform);
                _RocketEffect.Play();
            }
            _RocketEffect.gameObject.SetActive(magazine > 0);
        }

        private void OnProcessRelease(int magazine) {
            _RocketEffect.gameObject.SetActive(false);
        }

        public override void ThrowOut(CharacterUnit owner, Vector2? throwForce = null, float? angularVel = null) {
            base.ThrowOut(owner, throwForce, angularVel);
            _RocketEffect.gameObject.SetActive(false);
        }

        protected void OnDestroy() {
            _AutoFireProcessor.OnProcessHold -= OnProcessHold;
            _AutoFireProcessor.OnProcessRelease -= OnProcessRelease;
        }

    }
}