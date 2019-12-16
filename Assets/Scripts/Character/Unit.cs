using Character.MuscleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public MuscleController Controller { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<MuscleController>();
    }
}
