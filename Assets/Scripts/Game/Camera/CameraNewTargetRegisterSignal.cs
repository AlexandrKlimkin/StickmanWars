using System.Collections;
using System.Collections.Generic;
using Game.CameraTools;
using UnityEngine;

public struct CameraNewTargetRegisterSignal {
    public ICameraTarget NewCameraTarget;

    public CameraNewTargetRegisterSignal(ICameraTarget newCameraTarget) {
        this.NewCameraTarget = newCameraTarget;
    }
}
