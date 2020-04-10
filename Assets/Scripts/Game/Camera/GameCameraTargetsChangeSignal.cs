using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraTools {
    public struct GameCameraTargetsChangeSignal {
        public List<(ICameraTarget, bool)> Pairs;

        public GameCameraTargetsChangeSignal(List<(ICameraTarget, bool)> pairs) {
            this.Pairs = pairs;
        }

        public GameCameraTargetsChangeSignal(ICameraTarget cameraTarget, bool add) {
            this.Pairs = new List<(ICameraTarget, bool)>{new ValueTuple<ICameraTarget, bool>(cameraTarget, add)};
        }
    }
}
