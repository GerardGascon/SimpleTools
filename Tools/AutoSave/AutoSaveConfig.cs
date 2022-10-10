#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Threading;
using UnityEditor.SceneManagement;
using System.Threading.Tasks;

namespace SimpleTools.AutoSave {
	public class AutoSaveConfig : EditorWindow {

		[MenuItem("Simple Tools/Auto Save Configuration")]
		public static void ShowWindow(){
			EditorWindow w = GetWindow<AutoSaveConfig>("Auto-save Configuration");
			w.position = new Rect(w.position.position, new Vector2(400, 150));
			var data = EditorPrefs.GetString("AutoSave", JsonUtility.ToJson(w, false));
			JsonUtility.FromJsonOverwrite(data, w);
		}

		[InitializeOnLoadMethod]
		static void OnInitialize(){
			int _index = EditorPrefs.GetInt("Index", 0);
			bool _logging = EditorPrefs.GetBool("Logging", false);
			ChangeAutoSaveMode(_index, _logging);
		}

		protected void OnEnable() {
			OnInitialize();
		}

		protected void OnDisable() {
			var data = JsonUtility.ToJson(this, false);
			EditorPrefs.SetString("AutoSave", data);
			EditorPrefs.SetInt("Index", index);
			EditorPrefs.SetBool("Logging", logging);
		}

		readonly static string[] options = new string[] { "Disabled", "On Play", "1 Minute", "10 Minutes", "1 Hour" };
		public static int index;
		public static bool enabled;
		public static bool logging;

		void OnGUI() {
			GUILayout.Label("Select auto-save mode:", EditorStyles.boldLabel);
			int i = EditorGUILayout.Popup(index, options);
			if (i != index) ChangeAutoSaveMode(i, logging);

			GUILayout.Label("Log a message every time a the scene gets saved.");
			if (logging) {
				if (GUILayout.Button("Disable Logging")){
					logging ^= true;
					ChangeAutoSaveMode(i, logging);
				}
				
			} else {
				if (GUILayout.Button("Enable Logging")) {
					logging ^= true;
					ChangeAutoSaveMode(i, logging);
				}
			}
		}

		static CancellationTokenSource _tokenSource;
		static Task _task;
		static int frequency;
		static void ChangeAutoSaveMode(int mode, bool log){
			index = mode;
			logging = log;
			CancelTask();
			enabled = true;
			EditorApplication.playModeStateChanged -= AutoSaveWhenPlayModeStarts;

			switch(index){
				case 0:
					enabled = false;
					return;
				case 1:
					EditorApplication.playModeStateChanged += AutoSaveWhenPlayModeStarts;
					return;
				case 2:
					frequency = 1 * 60 * 1000;
					break;
				case 3:
					frequency = 10 * 60 * 1000;
					break;
				case 4:
					frequency = 60 * 60 * 1000;
					break;
			}

			_tokenSource = new CancellationTokenSource();
			_task = SaveInterval(_tokenSource.Token);
		}

		static void AutoSaveWhenPlayModeStarts(PlayModeStateChange state){
			if(state == PlayModeStateChange.ExitingEditMode){
				EditorSceneManager.SaveOpenScenes();
				AssetDatabase.SaveAssets();
				if (logging) Debug.Log($"Auto-saved at {DateTime.Now:h:mm:ss tt}");
			}
		}

		static void CancelTask() {
			if (_task == null) return;
			_tokenSource.Cancel();
		}

		static async Task SaveInterval(CancellationToken token) {
			while (!token.IsCancellationRequested) {
				await Task.Delay(frequency, token);

				if (token.IsCancellationRequested) break;

				if (!enabled || Application.isPlaying || BuildPipeline.isBuildingPlayer || EditorApplication.isCompiling) return;
				if (!UnityEditorInternal.InternalEditorUtility.isApplicationActive) return;

				EditorSceneManager.SaveOpenScenes();
				AssetDatabase.SaveAssets();
				if (logging) Debug.Log($"Auto-saved at {DateTime.Now:h:mm:ss tt}");
			}
		}
	}
}
#endif