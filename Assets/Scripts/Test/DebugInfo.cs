using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DebugInfo : MonoBehaviour {
        public Text MinFPSText;
        public Text FPSText;
        public Text RigidbodiesCountText;
        public float UpdatesPerSec;

        private int _MinFrameRate = int.MaxValue;
        private int _MaxRigidbodies;

        private void Start() {
            StartCoroutine(UpdateRoutine());
        }

        private IEnumerator UpdateRoutine() {
            while (true) {
                yield return new WaitForSecondsRealtime(1 / UpdatesPerSec);
                var frameRate = 1 / Time.deltaTime;
                var intFramerate = (int) frameRate;
                if (frameRate < _MinFrameRate)
                    _MinFrameRate = intFramerate;
                MinFPSText.text = $"MIN FPS: {_MinFrameRate}";
                FPSText.text = $"FPS: {intFramerate}";
                RigidbodiesCountText.text = $"RIGIDBODIES COUNT: {Object.FindObjectsOfType<Rigidbody2D>().Length}";
            }
        }
    }
}
