using UnityEngine;

namespace TC_Tutorial {
    public class DisableOnRightClickPressed : MonoBehaviour {
        void Update() {
            if (Input.GetMouseButton(1)) {
                gameObject.SetActive(false);
            }
        }
    }
}
