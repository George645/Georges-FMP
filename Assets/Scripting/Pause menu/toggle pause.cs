using UnityEngine;

namespace TC_PauseMenu {
    public class togglepause : MonoBehaviour {
        void Start() {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        void Update() {
            if (Input.GetKeyDown("escape")) {
                transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            }
        }
    }
}