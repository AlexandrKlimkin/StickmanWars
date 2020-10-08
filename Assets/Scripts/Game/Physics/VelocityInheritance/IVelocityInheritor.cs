using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVelocityInheritor {
    Rigidbody2D AttachedToRB { get; set; }
    bool CanDetach { get; set; }
}