using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleTools.SceneManagement {
    public static class Loader {

        class LoadingMonoBehaviour : MonoBehaviour { }

        static Action onLoaderCallback;
        static AsyncOperation loadingAsyncOperation;

        /// <summary>Load a scene with a loading scene
        /// <para>It requires a scene called "Loading" where the loading screen is located.</para>
        /// </summary>
        public static void Load(int scene) {
            onLoaderCallback = () => {
                GameObject loadingGameObject = new GameObject("LoadingGameObject");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene("Loading");
        }
        /// <summary>Load a scene with a loading scene
        /// <para>It requires a scene called "Loading" where the loading screen is located.</para>
        /// </summary>
        public static void Load(string scene) {
            onLoaderCallback = () => {
                GameObject loadingGameObject = new GameObject("LoadingGameObject");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene("Loading");
        }

        static IEnumerator LoadSceneAsync(int scene) {
            yield return null;
            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene);

            while (!loadingAsyncOperation.isDone) {
                yield return null;
            }
        }
        static IEnumerator LoadSceneAsync(string scene) {
            yield return null;
            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene);

            while (!loadingAsyncOperation.isDone) {
                yield return null;
            }
        }

        /// <summary>Returns the loading progress
        /// </summary>
        public static float GetLoadingProgress() {
            if (loadingAsyncOperation != null) {
                return loadingAsyncOperation.progress;
            } else {
                return 0f;
            }
        }

        public static void LoaderCallback() {
            if (onLoaderCallback != null) {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }
    }
}