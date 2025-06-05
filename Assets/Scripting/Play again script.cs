using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC_Menu {
    public class Playagainscript : MonoBehaviour {
        public void playAgain() {
            MenuHandling.boardState = null;
            SceneManager.LoadScene("Intro scene");
        }
    }
}
