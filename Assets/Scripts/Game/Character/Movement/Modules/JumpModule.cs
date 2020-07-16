using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityDI;
using UnityEngine;

namespace Character.Movement.Modules {
    public class JumpModule : MovementModule {

        [Dependency]
        private readonly AudioService _AudioService;

        private JumpParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;
        private JumpData _JumpData;
        private WalkData _WalkData;

        private float _JumpTimer;

        public JumpModule(JumpParameters parameters) {
            _Parameters = parameters;
        }

        public override void Start() {
            ContainerHolder.Container.BuildUp(this);
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
            _JumpData = BB.Get<JumpData>();
            _WalkData = BB.Get<WalkData>();
        }

        public override void LateUpdate() {
            _JumpData.Jump = false;
        }

        public bool Jump(MonoBehaviour behaviour) {
            if (_GroundedData.Grounded && _GroundedData.TimeSinceMainGrounded < 0.3f) {
                _JumpTimer = _Parameters.LowJumpTime;
                behaviour.StopCoroutine(JumpRoutine());
                behaviour.StartCoroutine(JumpRoutine());
                SpawnJumpEffects();
                PlayAudioEffect();

                return true;
            }
            return false;
        }

        private void SpawnJumpEffects() {
            if (_Parameters.JumpEffectTransformPoints.IsNullOrEmpty() || _Parameters.JumpEffectNames.IsNullOrEmpty())
                return;
            foreach (var point in _Parameters.JumpEffectTransformPoints) {
                var randIndex = UnityEngine.Random.Range(0, _Parameters.JumpEffectNames.Count);
                var effect = VisualEffect.GetEffect<ParticleEffect>(_Parameters.JumpEffectNames[randIndex]);
                effect.transform.position = point.transform.position;
                effect.transform.rotation = Quaternion.identity;
                effect.transform.localScale = new Vector3(Mathf.Abs(effect.transform.localScale.x) * _WalkData.Direction, effect.transform.localScale.y, effect.transform.localScale.z);
                effect.Play();
            }
        }

        private void PlayAudioEffect() {
            if (_Parameters.JumpAudioEffectNames.IsNullOrEmpty())
                return;
            var randIndex = UnityEngine.Random.Range(0, _Parameters.JumpAudioEffectNames.Count);
            _AudioService.PlaySound3D(_Parameters.JumpAudioEffectNames[randIndex], false, false, CommonData.MovementController.transform.position);
        }

        private IEnumerator JumpRoutine() {
            var gravityScale = CommonData.ObjRigidbody.gravityScale;
            while (_JumpTimer > 0) {
                CommonData.ObjRigidbody.velocity = new Vector2(CommonData.ObjRigidbody.velocity.x, _Parameters.JumpSpeed);
                //CommonData.ObjRigidbody.gravityScale = 0;
                _JumpData.LastJumpTime = Time.time;
                _JumpTimer -= Time.deltaTime;
                yield return null;
            }
            CommonData.ObjRigidbody.gravityScale = gravityScale;
            _JumpTimer = 0;
        }

        public void ContinueJump() {
            if (_JumpTimer != 0)
                _JumpTimer += _Parameters.HighJumpAddTime;
        }

        public bool WallJump() {
            if (_WallSlideData.RightTouch) {
                var vector = new Vector2(-1, 0.9f).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                _JumpData.LastWallJumpTime = Time.time;
                return true;
            }
            if (_WallSlideData.LeftTouch) {
                var vector = new Vector2(1, 0.9f).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                _JumpData.LastWallJumpTime = Time.time;
                return true;
            }
            return false;
        }

        public void ContinueWallJump() {
            if (_JumpTimer != 0)
                _JumpTimer += _Parameters.HighJumpAddTime;
        }
    }

    [Serializable]
    public class JumpParameters {
        public float JumpSpeed;
        public float WallJumpSpeed;
        public float LowJumpTime;
        public float HighJumpAddTime;
        public List<Transform> JumpEffectTransformPoints;
        public List<string> JumpEffectNames;
        public List<string> JumpAudioEffectNames;
    }
}