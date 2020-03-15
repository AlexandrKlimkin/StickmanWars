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

        public Vector3 Position => transform.position;
        public Vector3 Velocity => Vector3.zero;
        public float Direction => 0f;

        private void Update()
        {
            if (Triggers.Any(_ => _.ContainsUnit()))
            {
                if (!GameCameraBehaviour.Instance.Targets.Contains(this)) {
                    GameCameraBehaviour.Instance.Targets.Add(this);
                }
            }
            else
            {
                if (GameCameraBehaviour.Instance.Targets.Contains(this)) {
                    GameCameraBehaviour.Instance.Targets.Remove(this);
                }
            }
        }
    }
}