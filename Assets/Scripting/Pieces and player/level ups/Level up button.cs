using UnityEngine;
using UnityEngine.SceneManagement;
namespace TC{
    public class LevelUpButton : MonoBehaviour {
        public static GameObject levelUpButton;
        public GameObject levelingUpObject;
        [SerializeField] GameObject canvasToDisplay;
        void Start() {
            levelUpButton = gameObject;
        }

        public void LevelUpSceneChange() {
            Camera.main.gameObject.GetComponent<PlayerCamera>().mode = Mode.levelling;
            SceneManager.LoadScene("Level Up scene", LoadSceneMode.Additive);
            levelingUpObject.GetComponent<UnderlyingPiece>().RunCoroutine();
            canvasToDisplay.SetActive(true);
            canvasToDisplay.transform.GetChild(0).gameObject.GetComponent<EnlargeAreaButton>().upgradingObject = levelingUpObject;
            levelingUpObject.GetComponent<UnderlyingPiece>().selected = false;
        }
    }
}
