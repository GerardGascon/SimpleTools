using UnityEngine;
using Cinemachine;

namespace SimpleTools.Cinemachine {
	public class CMCameraTrigger : MonoBehaviour {

		CinemachineVirtualCamera vcam;
		[SerializeField, Tooltip("Name of the collider's tag that will trigger the camera.")] string triggerTagName;

		void Awake() {
			vcam = GetComponentInChildren<CinemachineVirtualCamera>(true);
			vcam.gameObject.SetActive(false);
		}

		#region 3D
		void OnTriggerEnter(Collider col) {
			if (col.CompareTag(triggerTagName)) {
				vcam.gameObject.SetActive(true);
			}
		}
		void OnTriggerExit(Collider col) {
			if (col.CompareTag(triggerTagName)) {
				vcam.gameObject.SetActive(true);
			}
		}
		#endregion

		#region 2D
		void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag(triggerTagName)) {
				vcam.gameObject.SetActive(true);
			}
		}
		void OnTriggerExit2D(Collider2D col) {
			if (col.CompareTag(triggerTagName)) {
				vcam.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}