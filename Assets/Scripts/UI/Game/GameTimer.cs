using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private int seconds, minutes, hours;
    [SerializeField] private Text timerDisplay;

    private void Start()
    {
        StartTimer();
    }
    public void StartTimer ()
    {
        StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        StopCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {            
      
            seconds++;
            if (seconds == 60)
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
                timerDisplay.text = minutes + "м " + seconds + " c";
            }
            else if (hours != 0)
            {
                timerDisplay.text = hours + "ч " + minutes + "м";
            }
            else
            {
                timerDisplay.text = seconds + "с";
            }

            yield return new WaitForSeconds(1);
        }
    }
}
