using UnityEngine;

namespace TC {
    public class StartingBoardScript : MonoBehaviour {
        public GameObject OriginalObject;

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Info)) {
                    if (Info.collider.gameObject == gameObject) {
                        OriginalObject.transform.position = transform.position;
                    }
                }
            }
        }
    }
}