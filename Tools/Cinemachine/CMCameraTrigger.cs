using UnityEngine;
using Cinemachine;

namespace SimpleTools.Cinemachine {
	public class CMCameraTrigger : MonoBehaviour {

		CinemachineVirtualCamera vcam;

		void Awake() {
			vcam = GetComponentInChildren<CinemachineVirtualCamera>(true);
			vcam.gameObject.SetActive(false);
		}

		#region 3D
		void OnTriggerEnter(Collider col) {
			if (col.CompareTag("Player")) {
				vcam.gameObject.SetActive(true);
			}
		}
		void OnTriggerExit(Collider col) {
			if (col.CompareTag("Player")) {
				vcam.gameObject.SetActive(true);
			}
		}
		#endregion

		#region 2D
		void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				vcam.gameObject.SetActive(true);
			}
		}
		void OnTriggerExit2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				vcam.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}