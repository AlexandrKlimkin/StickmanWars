using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InputSystem {
    [Serializable]
    public class InputKit {
        public int Id;
        public string Horizontal;
        public string Vertical;
        public KeyCode Attack1;
        public KeyCode Attack2;
        public KeyCode Jump;
    }
}
