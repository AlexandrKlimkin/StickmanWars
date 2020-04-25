using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
    public class DestroyOnAwake : MonoBehaviour {
        private void Awake() {
            Destroy(this.gameObject);
        }
    }
}
