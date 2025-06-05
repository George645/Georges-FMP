using UnityEngine;
namespace TC {
    public class MovementCircles : MonoBehaviour {
        public GameObject OriginalObject;
        public Vector2Int offset;

        void FixedUpdate() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Info)) {
                    if (Info.collider.gameObject == gameObject) {
                        CheckIfTaking();
                        OriginalObject.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
                        OriginalObject.GetComponent<UnderlyingPiece>().selected = false;
                        OriginalObject.GetComponent<UnderlyingPiece>().hasMoved = true;
                        OriginalObject.GetComponent<UnderlyingPiece>().DeactivateVisibility();
                        Player.player.GetComponent<Player>().numberOfMoves -= 1;
                    }
                }
            }
        }
        void CheckIfTaking() {
            if (OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y) != null) {
                OriginalObject.GetComponent<UnderlyingPiece>().capturedPieces += 1;
                AI.ai.AITeam.Remove(OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y).GetComponent<UnderlyingPiece>().thisPiece);
                Destroy(OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y));
            }
        }
    }
}