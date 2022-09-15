using Cinemachine;
using UnityEngine;

namespace SimpleTools.Cinemachine {
	public static class ScreenShake {

		static CinemachineVirtualCamera vCam;
		static ScreenShakeUpdate shakeUpdate;

		class ScreenShakeUpdate : MonoBehaviour {
			[HideInInspector] public float shakeTimer;
			[HideInInspector] public float shakeTimerTotal;
			[HideInInspector] public float startingIntensity;

			void Update() {
				if (shakeTimer > 0) {
					shakeTimer -= Time.deltaTime;
					CinemachineBasicMultiChannelPerlin multiChannelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
					multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
				}
			}
		}

		/// <summary>Shake the camera
		/// <para>It needs a cinemachine camera with a noise profile in it.</para>
		/// </summary>
		public static void Shake(float intensity, float time) {
			if (vCam == null) {
				vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
			}
			if (shakeUpdate == null) {
				shakeUpdate = new GameObject("ShakeUpdate").AddComponent<ScreenShakeUpdate>();
			}
			shakeUpdate.startingIntensity = intensity;
			shakeUpdate.shakeTimer = shakeUpdate.shakeTimerTotal = time;
		}
	}
}