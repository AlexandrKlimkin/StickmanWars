using Game.Match;
using KlimLib.SignalBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerController : MonoBehaviour
{
    [SerializeField] private GameTimer _timer;

    [Dependency]
    private readonly SignalBus _SignalBus;

    private void Awake()
    {
        ContainerHolder.Container.BuildUp(this);
        _SignalBus.Subscribe<MatchEndSignal>(OnMatchEnd, this);
    }

    private void Start()
    {
        StartTimer();
    }

    private void OnMatchEnd(MatchEndSignal signal)
    {
        StopTimer();
    }
    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(_timer.Timer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        _SignalBus.UnSubscribe<MatchEndSignal>(this);
    }
}
