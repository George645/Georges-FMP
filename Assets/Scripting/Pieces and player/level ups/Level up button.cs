using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUpButton : MonoBehaviour{
    public static GameObject levelUpButton;
    public GameObject levelingUpObject;
    [SerializeField] GameObject canvasToDisplay;
    void Start() {
        levelUpButton = gameObject;
    }

    void Update() {
        
    }
    public void levelUpSceneChange(){
        Camera.main.gameObject.GetComponent<Player_Camera>().mode = Mode.levelling;
        Debug.Log(Camera.main.gameObject.GetComponent<Player_Camera>().mode);
        Debug.Log(Camera.main.gameObject);  
        SceneManager.LoadScene("Level Up scene", LoadSceneMode.Additive);
        levelingUpObject.GetComponent<UnderlyingPiece>().runCoroutine();
        canvasToDisplay.SetActive(true);
        Debug.Log(canvasToDisplay.transform.GetChild(0).GetChild(0).gameObject);
        canvasToDisplay.transform.GetChild(0).gameObject.GetComponent<Enlargeareabutton>().upgradingObject = levelingUpObject;
    }
}
