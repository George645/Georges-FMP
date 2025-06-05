using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TC_Menu {
    public class MenuHandling : MonoBehaviour {
        public static TC.Gamestate boardState;
        [SerializeField]
        GameObject alternateMenuToSwapTo;
        public void ChangeScene(string scene) {
            if (scene == "Intro scene") {
                boardState = null;
            }
            SceneManager.LoadScene(scene);
        }
        public void QuitApplication() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        public void SwapMenu() {
            try {
                StartCoroutine(MoveCamera(true));
            }
            catch {
                Debug.LogError("FIX THIS YOU IDIOT");
            }
        }
        IEnumerator MoveCamera(bool setActiveTo) {
            /*for (float i = 1; i <= 100; i++) {
                Debug.Log(i);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position, i / 100);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.01f);*/
            alternateMenuToSwapTo.SetActive(setActiveTo);
            if (gameObject.name != "Options") {
                transform.parent.gameObject.SetActive(false);
            }
            yield return null;
        }
        public void Return() {
            StartCoroutine(MoveCamera(true));
        }
        public void AdjustSensitivity() {
            int input = (int)transform.GetComponent<Slider>().value;
            PlayerPrefs.SetInt("Sensitivity", input);
        }
        public void Continue() {
            SceneManager.LoadScene("Main gameplay scene");
        }
    }
}
