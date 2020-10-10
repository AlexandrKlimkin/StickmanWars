
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float seconds, minutes, hours;
    [SerializeField] private Text timerDisplay;

    public IEnumerator Timer()
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
