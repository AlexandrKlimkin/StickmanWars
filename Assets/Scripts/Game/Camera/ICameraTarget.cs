using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraTools
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        Vector3 Velocity { get; }
        float Direction { get; }
    }
}
