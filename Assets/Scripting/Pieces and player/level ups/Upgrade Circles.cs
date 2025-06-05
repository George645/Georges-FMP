using UnityEngine;

namespace TC {
    public class UpgradeCircles : MonoBehaviour {
        public GameObject OriginalObject;
        public Vector2Int offset;

        void FixedUpdate() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Info)) {
                    if (Info.collider.gameObject == gameObject) {
                        Debug.Log(OriginalObject.GetComponent<UnderlyingPiece>().thisPiece.name);
                        OriginalObject.GetComponent<UnderlyingPiece>().thisPiece.AddPositionToMovables(offset);
                        OriginalObject.GetComponent<UnderlyingPiece>().RemoveNumbersOfCapturedPieces();
                        OriginalObject.GetComponent<UnderlyingPiece>().level += 1;
                        OriginalObject.GetComponent<UnderlyingPiece>().selected = false;
                        OriginalObject.GetComponent<UnderlyingPiece>().DeactivateVisibility();
                    }
                }
            }
        }
    }
}