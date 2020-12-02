using System;
using UnityEngine;

namespace KlimLib.Timers {
    public class TimeSpanTimerController : TimerController {
        [SerializeField]
        private string _FormatString;

        protected override string GetTimeText(float time) {
            var ticks = (long)time * TimeSpan.TicksPerSecond;
            var timeSpan = new TimeSpan(ticks);
            return timeSpan.ToString(_FormatString);
        }
    }
}
