using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC_PauseMenu {
    public class Returntomenu : MonoBehaviour {
        public void ReturnToMenu() {
            SceneManager.LoadScene("main menu");
        }
    }
}
