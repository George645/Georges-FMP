using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandling : MonoBehaviour{
    [SerializeField]
    GameObject alternateMenuToSwapTo;
    public void ChangeScene(string scene) {
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
            StartCoroutine(MoveCamera(transform.position));
        }
        catch {
            Debug.LogError("FIX THIS YOU IDIOT");
        }
    }
    IEnumerator MoveCamera(Vector3 position) {
        for (float i = 1; i <= 25; i++) {
            Debug.Log(i / 10);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position, 0.001f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.01f);
        alternateMenuToSwapTo.SetActive(true);

        yield return null;
    }
    public void Return() {
        alternateMenuToSwapTo.SetActive(false);
        StartCoroutine(MoveCamera(new Vector3(0.5f, 8, -0.5f)));
    }
    public void adjustSensitivity() {
        int input = (int)transform.GetComponent<Slider>().value;
        PlayerPrefs.SetInt("Sensitivity", input);
    }
    public void Continue(){
        Debug.LogError("NotImplementedYet");
    }
}
