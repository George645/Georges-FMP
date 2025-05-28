using UnityEngine;
using UnityEngine.SceneManagement;

public class Playagainscript : MonoBehaviour {
    public void playAgain() {
        MenuHandling.boardState = null;
        SceneManager.LoadScene("Intro scene");
    }
}
