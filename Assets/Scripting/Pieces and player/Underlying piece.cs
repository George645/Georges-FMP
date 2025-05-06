using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnderlyingPiece : MonoBehaviour {
    [HideInInspector]
    public bool selected;
    [HideInInspector]
    public bool firstFrameSelected = true;
    internal PieceMovement thisPiece;
    internal int capturedPieces = 0;
    internal int level = 1;
    internal int previousLevel = 1;
    internal bool playersTeam = false;
    public static GameObject instance;
    internal Mode mode = Mode.gaming;
    public Vector3 previousPosition;
    private void Awake() {
        playersTeam = Random.Range(0, 2) < 0.5f;
    }
    private void Start() {
        ActualStart();
    }
    public void ActualStart() {
        foreach (PieceMovement piece in OverarchingPieceMovement.Instance.allPieceMovement) {
            if (piece.name == name) {
                thisPiece = piece;
                return;
            }
        }
        thisPiece = OverarchingPieceMovement.Instance.allPieceMovement[Random.Range(1, OverarchingPieceMovement.Instance.allPieceMovement.Count)];
    }

    internal void IfNotLevellingReturn() {
        if (!thisPiece.CanLevelUp(level, capturedPieces)) {
            SceneManager.UnloadSceneAsync("Level up scene");
            mode = Mode.gaming;
            Camera.main.gameObject.GetComponent<PlayerCamera>().mode = Mode.gaming;
        }
    }

    public GameObject PieceInDirection(int x, int z) {
        GameObject objecta = null;
        if (Physics.Raycast(new Vector3(transform.position.x + x, 3.5f, transform.position.z + z), Vector3.down * 4, out RaycastHit hit, 2.25f)) {
            objecta = hit.collider.gameObject;
        }
        return objecta;
    }
    public void RunCoroutine() {
        StartCoroutine(SetAlternateSceneVariables());
    }
    IEnumerator SetAlternateSceneVariables() {
        mode = Mode.levelling;
        previousPosition = transform.position;
        yield return null;
        LevelUpOriginCubeIdentifier.instance.GetComponent<OriginCube>().sizeNumber = (thisPiece.potentialRange - 1) / 2;
        yield return null;
    }
    public void EnsureCorrectPositions() {
        if (mode == Mode.gaming) {
            transform.position = previousPosition;
        }
        else {
            transform.position = new Vector3(500, 2f, 500);
        }
    }

    public void RemoveNumbersOfCapturedPieces() {
        if (thisPiece.infinitelyScalingRange) {
            capturedPieces -= level * level / 2 + 3 * level / 2 + 2;
        }
        else {
            capturedPieces -= level;
        }
    }

    #region Selected
    List<GameObject> movableTiles = new();
    internal void Selected() {
        if (mode == Mode.gaming) {
            if (selected && playersTeam) {
                if (thisPiece.CanLevelUp(level, capturedPieces)) {
                    LevelUpButton.levelUpButton.SetActive(true);
                    LevelUpButton.levelUpButton.GetComponent<LevelUpButton>().levelingUpObject = gameObject;
                }
                if (firstFrameSelected) {
                    int count = 0;
                    if (thisPiece.infinitelyScalingRange) {
                        for (int x = -1; x < 2; x++) {
                            for (int z = -1; z < 2; z++) {
                                if (thisPiece.PositionIsUnlocked(x, z)) {
                                    for (int i = 1; i < 100; i++) {
                                        if (CubeBase.GetSquareInDirection(transform.position, x * i, z * i) != null && (PieceInDirection(x * i, z * i) == null || PieceInDirection(x * i, z * i).GetComponent<UnderlyingPiece>().playersTeam == false)) {
                                            movableTiles.Add(MoveableDisplays.Instance.GetObject());
                                            movableTiles[count].SetActive(true);
                                            movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                            movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2Int(x, z) * i;
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
                                    movableTiles.Add(MoveableDisplays.Instance.GetObject());
                                    movableTiles[count].SetActive(true);
                                    movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                    movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2Int(i, j);
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
        else {
            if (firstFrameSelected) {
                int count = 0;
                if (selected) {
                    if (thisPiece.infinitelyScalingRange) {

                    }
                    else {
                        for (int x = -(thisPiece.potentialRange - 1) / 2; x < (thisPiece.potentialRange - 1) / 2 + 1; x++) {
                            for (int z = -(thisPiece.potentialRange - 1) / 2; z < (thisPiece.potentialRange - 1) / 2 + 1; z++) {
                                if (x == 0 && z == 0) continue;
                                if (!thisPiece.PositionIsUnlocked(x, z)) {
                                    movableTiles.Add(MoveableDisplays.Instance2.GetObject());
                                    movableTiles[count].SetActive(true);
                                    movableTiles[count].GetComponent<UpgradeCircles>().OriginalObject = gameObject;
                                    movableTiles[count].GetComponent<UpgradeCircles>().offset = new Vector2Int(x, z);
                                    movableTiles[count].transform.position = new Vector3(transform.position.x + x, 1.1f, transform.position.z + z);
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            else {
                DeactivateVisibility();
            }
        }
    }

    public void DeactivateVisibility() {
        if (!firstFrameSelected) {
            try {
                LevelUpButton.levelUpButton.SetActive(false);
            }
            catch { }
        }
        firstFrameSelected = true;
        foreach (GameObject obj in movableTiles) {
            obj.SetActive(false);
        }
        movableTiles = new();
    }

    #endregion
}
