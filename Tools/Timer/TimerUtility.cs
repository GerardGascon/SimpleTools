using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace SimpleTools.Timer{
	public static class TimerUtility {
		/// <summary>
		/// Setup the timer
		/// </summary>
		/// <param name="container">TMPro object that will contain the timer</param>
		/// <param name="timerType">What type of timer will it be (Countdown, Stopwatch, Clock)</param>
		/// <param name="countdownTime">The time that will have in case it is a countdown timer</param>
		/// <returns></returns>
		public static Timer SetupTimer(this TMP_Text container, TimerType timerType, TimerUpdate timerUpdate, float countdownTime = 60f){
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
			t.Setup(elapsedTime, true, timePlaying, container, timerType, timerUpdate, text);

			return t;
		}
	}
}
