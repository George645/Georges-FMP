using UnityEngine;

public class DisableOnLeftClick : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButton(0)) {
            gameObject.SetActive(false);
        }
    }
}
