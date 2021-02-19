using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Control
{
    public interface IAimProvider
    {
        Vector2 AimPoint { get; }
    }
}
