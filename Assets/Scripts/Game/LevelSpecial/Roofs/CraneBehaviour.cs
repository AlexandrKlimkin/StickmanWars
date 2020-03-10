using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial.Roofs
{
    public class CraneBehaviour : MonoBehaviour
    {
        public PathFollowPhysicsObject Platform;
        public SimpleRotationController BigGear;
        public SimpleRotationController SmallGear;

        private void Update()
        {
            if (Platform.IsMoving)
            {
                var clockwise = Platform.CurrentFollowPointIndex == 0;
                BigGear.Rotate(!clockwise);
                SmallGear.Rotate(clockwise);
            }
            else
            {
                BigGear.Stop();
                SmallGear.Stop();
            }
        }
    }
}