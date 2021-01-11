using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InputSystemSpace {
    [Serializable]
    public class InputKit {
        public int Id;
        public string Horizontal;
        public string Vertical;

        public string HorizontalRight;
        public string VerticalRight;

        public KeyCode Attack1;
        public KeyCode Attack2;
        public KeyCode Jump;
        public KeyCode ThrowOutWeapon;
        public KeyCode ThrowOutVehicle;
        public KeyCode Select;
        public KeyCode Back;
    }
}
