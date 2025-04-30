using UnityEngine;

public class StartingBoardScript : MonoBehaviour {
    public GameObject OriginalObject;
    void Start() {



    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Info)) {
                if (Info.collider.gameObject == gameObject) {
                    OriginalObject.transform.position = transform.position;
                }
            }
        }
    }
}