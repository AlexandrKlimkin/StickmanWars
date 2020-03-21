using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateParticlesOnStart : MonoBehaviour
{
    public float simulationTime;
    private ParticleSystem _PS;
    void Start()
    {
        _PS = GetComponent<ParticleSystem>();
        _PS.Simulate(simulationTime);
        _PS.Play();
    }
}
