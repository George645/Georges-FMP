using UnityEngine;

namespace TC_Tutorial {
    public class DisableOnLeftClick : MonoBehaviour {
        void Update() {
            if (Input.GetMouseButton(0)) {
                gameObject.SetActive(false);
            }
        }
    }
}