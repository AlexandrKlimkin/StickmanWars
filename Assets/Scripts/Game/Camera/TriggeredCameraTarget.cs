using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Game.LevelSpecial;
using UnityEditor;
using UnityEngine;

namespace Rendering
{
    public class TriggeredCameraTarget : MonoBehaviour, ICameraTarget
    {
        public List<UnitTrigger> Triggers;
        public float TriggerOutTime;

        public Vector3 Position => transform.position;
        public Vector3 Velocity => Vector3.zero;
        public float Direction => 0f;

        private float _TriggerOutTimer = 0f;

        private void Update()
        {
            if (Triggers.Any(_ => _.ContainsUnit()))
            {
                if (!GameCameraBehaviour.Instance.Targets.Contains(this)) {
                    GameCameraBehaviour.Instance.Targets.Add(this);
                }
                _TriggerOutTimer = 0f;
            }
            else
            {
                if (GameCameraBehaviour.Instance.Targets.Contains(this)) {
                    if (_TriggerOutTimer >= TriggerOutTime)
                    {
                        GameCameraBehaviour.Instance.Targets.Remove(this);
                    }
                    else
                    {
                        _TriggerOutTimer += Time.deltaTime;
                    }
                }
            }
        }
    }
}