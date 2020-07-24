using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public interface IMovementModule
{
    void Initialize(Blackboard bb);
    void Start();
    void Update();
    void LateUpdate();
    void FixedUpdate();
}