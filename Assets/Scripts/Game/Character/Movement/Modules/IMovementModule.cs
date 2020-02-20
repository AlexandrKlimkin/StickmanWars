using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementModule
{
    void Initialize(Blackboard bb);
    void Start();
    void Update();
    void FixedUpdate();
}