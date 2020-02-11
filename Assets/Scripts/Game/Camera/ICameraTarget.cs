using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    public interface ICameraTarget
    {
        Vector3 Position { get; }
        Vector3 Velocity { get; }
        float Direction { get; }
    }
}
