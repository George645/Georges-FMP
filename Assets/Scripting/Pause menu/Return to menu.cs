using UnityEngine;
using UnityEngine.SceneManagement;

public class Returntomenu : MonoBehaviour{
    public void ReturnToMenu() {
        MenuHandling.boardState = new Gamestate(AI.ai.PlayersTeam, AI.ai.AITeam);
        SceneManager.LoadScene("main menu");
    }
}
