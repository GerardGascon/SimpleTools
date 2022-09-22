using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace SimpleTools.Timer{
	public class Timer : MonoBehaviour{

		float elapsedTime;
		public float ElapsedTime { get { return elapsedTime; } }
		bool isPaused;
		public bool IsPaused { get { return isPaused; } }
		TimeSpan timePlaying;
		public TimeSpan TimePlaying { get { return timePlaying; } }
		TMP_Text timer;
		public TMP_Text TimerText { get { return timer; } }
		TimerType timerType;
		public TimerType TimerType { get { return timerType; } }
		TimerUpdate timerUpdate;
		public TimerUpdate TimerUpdate { get { return timerUpdate; } }

		float defaultTime;

		/// <summary>
		/// Setup the timer
		/// </summary>
		public void Setup(float elapsedTime, bool isPaused, TimeSpan timePlaying, TMP_Text timer, TimerType timerType, TimerUpdate timerUpdate, string text){
			this.elapsedTime = defaultTime = elapsedTime;
			this.isPaused = isPaused;
			this.timePlaying = timePlaying;
			this.timer = timer;
			this.timerType = timerType;
			this.timerUpdate = timerUpdate;
			timer.text = text;
		}

		IEnumerator UpdateTimer(){
			while (!isPaused){
				if(timerType == TimerType.Clock){
					timer.text = DateTime.Now.ToString("HH:mm:ss");
				}else{
					switch (timerType){
						case TimerType.Countdown:
							elapsedTime -= timerUpdate == TimerUpdate.UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
							if(elapsedTime < 0f){
								elapsedTime = 0f;
								isPaused = true;
							}
							break;
						case TimerType.Stopwatch:
							elapsedTime += timerUpdate == TimerUpdate.UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
							break;
					}
					timePlaying = TimeSpan.FromSeconds(elapsedTime);
					timer.text = timePlaying.ToString("m':'ss'.'ff");
				}
				yield return null;
			}
		}

		/// <summary>
		/// Play or resume the timer
		/// </summary>
		public void Play(){
			isPaused = false;
			StartCoroutine(UpdateTimer());
		}

		/// <summary>
		/// Pause the timer
		/// </summary>
		public void Stop(){
			isPaused = true;
		}

		/// <summary>
		/// Pause and sets the time to the defaultOne
		/// </summary>
		public void ResetTimer(){
			isPaused = true;
			elapsedTime = defaultTime;
			timePlaying = TimeSpan.FromSeconds(elapsedTime);
			timer.text = timePlaying.ToString("m':'ss'.'ff");
		}

		/// <summary>
		/// Restarts the timer
		/// </summary>
		public void Restart(){
			isPaused = false;
			elapsedTime = defaultTime;
			timePlaying = TimeSpan.FromSeconds(elapsedTime);
			timer.text = timePlaying.ToString("m':'ss'.'ff");
			StopAllCoroutines();
			StartCoroutine(UpdateTimer());
		}
	}
}
