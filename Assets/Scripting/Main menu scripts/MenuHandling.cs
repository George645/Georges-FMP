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
        alternateMenuToSwapTo.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
    public void adjustSensitivity() {
        int input = (int)transform.GetComponent<Slider>().value;
        PlayerPrefs.SetInt("Sensitivity", input);
    }
}
