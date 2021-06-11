using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace SimpleTools.Timer{
    public static class TimerUtility {
        public static Timer SetupTimer(this TMP_Text container, TimerType timerType, float countdownTime = 60f){
            Timer t = container.gameObject.AddComponent<Timer>();
            float elapsedTime = 0f;
            string text = string.Empty;
            TimeSpan timePlaying = TimeSpan.Zero;
			switch (timerType){
                case TimerType.Countdown:
                    elapsedTime = countdownTime;
                    timePlaying = TimeSpan.FromSeconds(elapsedTime);
                    text = timePlaying.ToString("m':'ss'.'ff");
                    break;
                case TimerType.Clock:
                    text = DateTime.Now.ToString("HH:mm:ss");
                    break;
            }
            t.Setup(elapsedTime, true, timePlaying, container, timerType, text);

            return t;
        }
    }
}
