using UnityEngine;

namespace TC_PauseMenu{
    public class Returntogame : MonoBehaviour {
        public void ReturnToGame() {
            transform.parent.gameObject.SetActive(false);
        }
    }
}