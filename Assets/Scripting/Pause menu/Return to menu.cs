using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC_PauseMenu {
    public class Returntomenu : MonoBehaviour {
        public void ReturnToMenu() {
            TC_Menu.MenuHandling.boardState = new TC.Gamestate(TC.AI.ai.PlayersTeam, TC.AI.ai.AITeam);
            SceneManager.LoadScene("main menu");
        }
    }
}
