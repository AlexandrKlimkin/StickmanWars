using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class SimpleRotationController : MonoBehaviour
    {
        public Transform Target;
        public float AngleSpeed;

        public bool IsRotating { get; private set; }
        public bool Clockwise { get; private set; }

        private void Update()
        {
            if(!Target)
                return;
            if(!IsRotating)
                return;
            var side = Clockwise ? -1 : 1;
            var delta = AngleSpeed * side * Time.deltaTime;
            Target.Rotate(Vector3.forward, delta);
        }

        public void Rotate(bool clockwise)
        {
            IsRotating = true;
            Clockwise = clockwise;
        }

        public void Stop()
        {
            IsRotating = false;
        }
    }
}