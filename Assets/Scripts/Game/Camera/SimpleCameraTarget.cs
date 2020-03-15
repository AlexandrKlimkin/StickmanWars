using UnityEngine;

namespace Rendering
{
    public class SimpleCameraTarget : MonoBehaviour, ICameraTarget
    {
        public Vector3 Position => transform.position;
        public Vector3 Velocity => Vector3.zero;
        public float Direction => 0f;

        private void Start()
        {
            GameCameraBehaviour.Instance.Targets.Add(this);
        }

        private void OnDestroy()
        {
            GameCameraBehaviour.Instance?.Targets.Remove(this);
        }
    }
}