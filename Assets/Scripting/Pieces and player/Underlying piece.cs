using System.Collections.Generic;
using UnityEngine;

public class UnderlyingPiece : MonoBehaviour{
    [HideInInspector]
    public bool selected;
    [HideInInspector]
    public bool firstFrameSelected = true;
    [SerializeField]
    internal PieceMovement thisPiece;
    internal int capturedPieces = 0;
    internal int level = 1;
    internal int previousLevel = 1;
    internal bool playersTeam = false;
    private void Awake() {
        playersTeam = Random.Range(0, 2) < 0.5f;
    }
    private void Start() {
        ActualStart();
    }
    public void ActualStart() {
        foreach (PieceMovement piece in Overarchingpiecemovement.Instance.allPieceMovement) {
            if (piece.name == name) {
                thisPiece = piece;
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

    #region Selected
    List<GameObject> movableTiles = new();
    internal void Selected() {
        if (selected && playersTeam) {
            if (firstFrameSelected) {
                int count = 0;
                if (thisPiece.infiniteRange) {
                    for (int x = -1; x < 2; x++) {
                        for (int z = -1; z < 2; z++) {
                            if (thisPiece.PositionIsUnlocked(x, z)) {
                                for (int i = 1; i < 100; i++) {
                                    if (CubeBase.GetSquareInDirection(transform.position, x * i, z * i) != null && (PieceInDirection(x * i, z * i) == null || PieceInDirection(x * i, z * i).GetComponent<UnderlyingPiece>().playersTeam == false)) {
                                        movableTiles.Add(Moveabledisplays.Instance.GetObject());
                                        movableTiles[count].SetActive(true);
                                        movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                        movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2(x, z) * i;
                                        movableTiles[count].transform.position = new Vector3(transform.position.x + x * i, 1.1f, transform.position.z + z * i);
                                        count++;
                                    }
                                    else {
                                        break;
                                    }
                                    try {
                                        if (!PieceInDirection(x * i, z * i).GetComponent<UnderlyingPiece>().playersTeam) {
                                            break;
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                else {
                    for (int i = -(thisPiece.potentialRange - 1) / 2; i <= (thisPiece.potentialRange - 1) / 2; i++) {
                        for (int j = -(thisPiece.potentialRange - 1) / 2; j <= (thisPiece.potentialRange - 1) / 2; j++) {
                            if (thisPiece.PositionIsUnlocked(i, j)) {
                                movableTiles.Add(Moveabledisplays.Instance.GetObject());
                                movableTiles[count].SetActive(true);
                                movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2(i, j);
                                movableTiles[count].transform.position = new Vector3(transform.position.x + i, 1.1f, transform.position.z + j);
                                count++;
                            }
                        }
                    }
                    firstFrameSelected = false;

                }
            }
        }
        else {
            DeactivateVisibility();
        }
    }

    public void DeactivateVisibility() {
        firstFrameSelected = true;
        foreach (GameObject obj in movableTiles) {
            obj.SetActive(false);
        }
        movableTiles = new();
    }

    #endregion
}
