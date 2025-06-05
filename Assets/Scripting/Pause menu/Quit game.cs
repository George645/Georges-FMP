using UnityEngine;

namespace TC_PauseMenu{
    public class Quitgame : MonoBehaviour {
        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
