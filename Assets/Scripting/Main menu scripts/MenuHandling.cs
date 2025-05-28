using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandling : MonoBehaviour {
    public static Gamestate boardState;
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
            StartCoroutine(MoveCamera(transform.position, true));
        }
        catch {
            Debug.LogError("FIX THIS YOU IDIOT");
        }
    }
    IEnumerator MoveCamera(Vector3 position, bool setActiveTo) {
        for (float i = 1; i <= 100; i++) {
            Debug.Log(i);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position, i / 100);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.01f);
        alternateMenuToSwapTo.SetActive(setActiveTo);

        yield break;
    }
    public void Return() {
        StartCoroutine(MoveCamera(new Vector3(0.5f, 8, -0.5f), false));
    }
    public void AdjustSensitivity() {
        int input = (int)transform.GetComponent<Slider>().value;
        PlayerPrefs.SetInt("Sensitivity", input);
    }
    public void Continue() {
        SceneManager.LoadScene("Main gameplay scene");
    }
}
