using UnityEngine;

public class UnderlyingPiece : MonoBehaviour{
    [HideInInspector]
    public bool selected;
    [HideInInspector]
    public bool firstFrameSelected = true;
    internal PieceMovement thisPiece;
    public int capturedPieces = 0;
    private void Start() {
        ActualStart();
    }
    public void ActualStart() {
        foreach (PieceMovement piece in Overarchingpiecemovement.Instance.allPieceMovement) {
            if (piece.name == name) {
                thisPiece = piece;
                Debug.Log(thisPiece.name);
                return;
            }
        }
        thisPiece = Overarchingpiecemovement.Instance.allPieceMovement[Random.Range(1, Overarchingpiecemovement.Instance.allPieceMovement.Count)];
        //thisPiece = Instantiate(basePiece);
    }
    public GameObject PieceInDirection(int x, int z) {
        GameObject objecta = null;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x + x, 3.5f, transform.position.z + z), Vector3.down * 4, out hit, 2.25f)) {
            objecta = hit.collider.gameObject;
        }
        return objecta;
    }
}
