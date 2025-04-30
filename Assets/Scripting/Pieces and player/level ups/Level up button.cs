using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUpButton : MonoBehaviour{
    public static GameObject levelUpButton;
    public GameObject levelingUpObject;
    public Vector3 PreviousPosition;
    GameObject mainGameCamera;
    void Start() {
        mainGameCamera = Camera.main.gameObject;
        levelUpButton = gameObject;
    }

    void Update() {
        
    }
    public void levelUpSceneChange(){
        Camera.main.gameObject.GetComponent<Player_Camera>().mode = Mode.levelling;
        SceneManager.LoadScene("Level Up scene", LoadSceneMode.Additive);
    }
}
