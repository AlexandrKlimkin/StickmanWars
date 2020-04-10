using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraTools {
    public class CameraBounds : MonoBehaviour
    {
        [SerializeField] private Vector2 _Position;
        [SerializeField] private Vector2 _Size;

        public Rect Rect { get; private set; }

        private void Start()
        {
            Rect = new Rect(_Position.x - _Size.x / 2, _Position.y - _Size.y / 2, _Size.x, _Size.y);
        }

        private void OnDrawGizmos()
        {
            Rect = new Rect(_Position.x - _Size.x / 2, _Position.y - _Size.y / 2, _Size.x, _Size.y);
            Gizmos.color = Color.magenta;
            MyGizmos.DrawRect(Rect);
        }
    }
}
