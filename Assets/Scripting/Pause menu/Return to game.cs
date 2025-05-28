using UnityEngine;

public class Returntogame : MonoBehaviour{
    public void ReturnToGame() {
        transform.parent.gameObject.SetActive(false);
    }
}
