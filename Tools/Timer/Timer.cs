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

		public void Setup(float elapsedTime, bool isPaused, TimeSpan timePlaying, TMP_Text timer, TimerType timerType, string text){
			this.elapsedTime = elapsedTime;
			this.isPaused = isPaused;
			this.timePlaying = timePlaying;
			this.timer = timer;
			this.timerType = timerType;
			timer.text = text;
		}

		IEnumerator UpdateTimer(){
			while (!isPaused){
				if(timerType == TimerType.Clock){
					timer.text = DateTime.Now.ToString("HH:mm:ss");
				}else{
					switch (timerType){
						case TimerType.Countdown:
							elapsedTime -= Time.deltaTime;
							if(elapsedTime < 0f){
								elapsedTime = 0f;
								isPaused = true;
							}
							break;
						case TimerType.Stopwatch:
							elapsedTime += Time.deltaTime;
							break;
					}
					timePlaying = TimeSpan.FromSeconds(elapsedTime);
					timer.text = timePlaying.ToString("m':'ss'.'ff");
				}
				yield return null;
			}
		}

		public void Play(){
			isPaused = false;
			StartCoroutine(UpdateTimer());
		}

		public void Stop(){
			isPaused = true;
		}

		public void ResetTimer(){
			isPaused = true;
			elapsedTime = 0f;
			timePlaying = TimeSpan.FromSeconds(elapsedTime);
			timer.text = timePlaying.ToString("m':'ss'.'ff");
		}

		public void Restart(){
			isPaused = false;
			elapsedTime = 0f;
			timePlaying = TimeSpan.FromSeconds(elapsedTime);
			timer.text = timePlaying.ToString("m':'ss'.'ff");
			StopAllCoroutines();
			StartCoroutine(UpdateTimer());
		}
	}
}
