using Game.Match;
using KlimLib.SignalBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float seconds, minutes, hours;
    [SerializeField] private Text timerDisplay;

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

    private void OnMatchEnd (MatchEndSignal signal)
    {
        StopTimer();
    }
    public void StartTimer ()
    {
        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        _SignalBus.UnSubscribe<MatchEndSignal>(this);
    }


    IEnumerator Timer()
    {
        while (true)
        {
            seconds += Time.unscaledDeltaTime;

            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes == 60)
                {
                    minutes = 0;
                    hours++;
                }
            }

            if (minutes != 0 && hours == 0)
            {
                timerDisplay.text = Math.Round(minutes) + "м " + Math.Round(seconds) + " c";
            }
            else if (hours != 0)
            {
                timerDisplay.text = Math.Round(hours) + "ч " + Math.Round(minutes) + "м";
            }
            else
            {
                timerDisplay.text = Math.Round(seconds) + "с";
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
