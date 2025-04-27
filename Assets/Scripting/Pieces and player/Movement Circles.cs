using UnityEngine;

public class MovementCircles : MonoBehaviour {
    public GameObject OriginalObject;
    public Vector2 offset;
    void Start() {

    }


    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Info)) {
                if (Info.collider.gameObject == gameObject) {
                    CheckIfTaking();
                    OriginalObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
                }
            }
        }
    }
    void CheckIfTaking() {
        if (OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection((int)offset.x, (int)offset.y) != null) {
            OriginalObject.GetComponent<UnderlyingPiece>().capturedPieces += 1;
            Destroy(OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection((int)offset.x, (int)offset.y));
        }
    }
}
