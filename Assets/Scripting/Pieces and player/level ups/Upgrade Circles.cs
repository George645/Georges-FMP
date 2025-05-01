using UnityEngine;

public class UpgradeCircles : MonoBehaviour {
    public GameObject OriginalObject;
    public Vector2Int offset;
    void Start() {

    }


    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Info)) {
                if (Info.collider.gameObject == gameObject) {
                    OriginalObject.GetComponent<UnderlyingPiece>().thisPiece.AddPositionToMovables(offset);
                    OriginalObject.GetComponent<UnderlyingPiece>().RemoveNumbersOfCapturedPieces();
                    OriginalObject.GetComponent<UnderlyingPiece>().level += 1;
                    OriginalObject.GetComponent<UnderlyingPiece>().DeactivateVisibility();
                }
            }
        }
    }
}