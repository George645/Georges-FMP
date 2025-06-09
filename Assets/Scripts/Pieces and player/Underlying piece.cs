using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC{
    public class UnderlyingPiece : MonoBehaviour {
        [HideInInspector]
        public bool selected;
        [HideInInspector]
        public bool firstFrameSelected = true;
        [SerializeField]
        internal PieceMovement thisPiece;
        [SerializeField]
        internal int capturedPieces = 0;
        [SerializeField]
        internal int level = 1;
        internal bool playersTeam = false;
        internal Mode mode = Mode.gaming;
        public Vector3 previousPosition;
        public bool hasMoved = false;

        public const int pieceHeight = 2;
        private void Awake() {
            playersTeam = Random.Range(0, 2) < 0.5f;
        }
        private void Start() {
            previousFrame = thisPiece.moveableTiles;
            ActualStart();
        }
        public void ActualStart() {
            for (int i = 0; i < OverarchingPieceMovement.Instance.allPieceMovement.Count; i++) {
                PieceMovement piece = OverarchingPieceMovement.Instance.allPieceMovement[i];
                if (piece.name == name) {
                    thisPiece = new PieceMovement(i, playersTeam, this);
                    return;
                }
            }
            int randomNumber = Random.Range(1, OverarchingPieceMovement.Instance.allPieceMovement.Count);
            thisPiece = new PieceMovement(randomNumber, playersTeam, this);


        }

        internal void IfNotLevellingReturn() {
            if (!thisPiece.CanLevelUp(level, capturedPieces)) {
                SceneManager.UnloadSceneAsync("Level up scene");
                mode = Mode.gaming;
                EnlargeAreaButton.thisGameObject.transform.parent.gameObject.SetActive(false);
                Camera.main.gameObject.GetComponent<PlayerCamera>().mode = Mode.gaming;
            }
        }

        private void OnDestroy() {
            if (playersTeam) {
                AI.ai.AITeam.Remove(thisPiece);
            }
            else {
                AI.ai.AITeam.Remove(thisPiece);
            }
        }

        public GameObject PieceInDirection(int x, int z) {
            GameObject objecta = null;
            if (Physics.Raycast(new Vector3(transform.position.x + x, 3.5f, transform.position.z + z), Vector3.down * 4, out RaycastHit hit, 2.25f)) {
                objecta = hit.collider.gameObject;
            }
            return objecta;
        }
        public GameObject PieceInDirection(Vector2 direction) {
            GameObject objecta = null;
            if (Physics.Raycast(new Vector3(transform.position.x + (int)direction.x, 3.5f, transform.position.z + (int)direction.y), Vector3.down * 4, out RaycastHit hit, 2.25f)) {
                objecta = hit.collider.gameObject;
            }
            return objecta;
        }
        public GameObject PieceInDirection(Vector2Int direction) {
            GameObject objecta = null;
            if (Physics.Raycast(new Vector3(transform.position.x + direction.x, 3.5f, transform.position.z + direction.y), Vector3.down * 4, out RaycastHit hit, 2.25f)) {
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
            LevelUpOriginCubeIdentifier.instance.GetComponent<OriginCube>().sizeNumber = thisPiece.potentialRange;
            yield return null;
        }
        public void EnsureCorrectPositions() {
            if (mode == Mode.gaming) {
                transform.position = previousPosition;
                thisPiece.AIAccessiblePosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            }
            else {
                if (name != "Player") {
                    transform.position = new Vector3(500, 2f, 500);
                }
                else {
                    transform.position = new Vector3(500, 1f, 500);
                }
            }
        }

        public void EnsureCorrectPositions(string hi) {
            if (mode == Mode.gaming) {
                if (previousPosition.y != 1) {
                    previousPosition = new Vector3(previousPosition.x, 1, previousPosition.z);
                }
                transform.position = previousPosition;
                thisPiece.AIAccessiblePosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            }
            else {
                transform.position = new Vector3(500, 1f, 500);
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
        bool[][] previousFrame = new bool[1][];
        internal void Selected() {
            previousFrame = thisPiece.moveableTiles;
            if (mode == Mode.gaming) {
                if (selected && playersTeam && !hasMoved) {
                    if (thisPiece.CanLevelUp(level, capturedPieces)) {
                        LevelUpButton.levelUpButton.SetActive(true);
                        LevelUpButton.levelUpButton.GetComponent<LevelUpButton>().levelingUpObject = gameObject;
                    }
                    if (firstFrameSelected) {
                        int count = 0;
                        try {
                            if (movableTiles[count].name.Contains("Starting")) {
                                DeactivateVisibility();
                            }
                        }
                        catch { }
                        if (thisPiece.infinitelyScalingRange) {
                            for (int x = -1; x <= 1; x++) {
                                for (int z = -1; z <= 1; z++) {
                                    if (thisPiece.PositionIsUnlocked(x, z)) {
                                        for (int i = 1; i < thisPiece.potentialRange; i++) {
                                            if (CubeBase.GetSquareInDirection(transform.position, x * i, z * i) != null && PieceInDirection(x * i, z * i) == null) {
                                                movableTiles.Add(MoveableDisplays.Instance.GetObject());
                                                movableTiles[count].SetActive(true);
                                                movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                                movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2Int(x, z) * i;
                                                movableTiles[count].transform.position = new Vector3(transform.position.x + x * i, 1.1f, transform.position.z + z * i);
                                                count++;
                                            }
                                            else if (CubeBase.GetSquareInDirection(transform.position, x * i, z * i) != null && PieceInDirection(x * i, z * i).GetComponent<UnderlyingPiece>().playersTeam == false) {
                                                movableTiles.Add(MoveableDisplays.Instance3.GetObject());
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
                            /*for (int i = 0; i < thisPiece.moveableTiles.Length; i++) {
                                for (int j = 0; j < thisPiece.moveableTiles.Length; j++) {
                                    Debug.Log(thisPiece.moveableTiles[i][j] + ", " + (i - thisPiece.potentialRange) + ", " + (j - thisPiece.potentialRange));
                                }
                            }*/
                            for (int x = -thisPiece.potentialRange; x <= thisPiece.potentialRange; x++) {
                                for (int z = -thisPiece.potentialRange; z <= thisPiece.potentialRange; z++) {
                                    if (thisPiece.PositionIsUnlocked(x, z) && PieceInDirection(x, z) == null) {
                                        movableTiles.Add(MoveableDisplays.Instance.GetObject());
                                        movableTiles[count].SetActive(true);
                                        movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                        movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2Int(x, z);
                                        movableTiles[count].transform.position = new Vector3(transform.position.x + x, 1.1f, transform.position.z + z);
                                        count++;
                                    }
                                    else if (thisPiece.PositionIsUnlocked(x, z) && !PieceInDirection(x, z).GetComponent<UnderlyingPiece>().playersTeam) {
                                        movableTiles.Add(MoveableDisplays.Instance3.GetObject());
                                        movableTiles[count].SetActive(true);
                                        movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                                        movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2Int(x, z);
                                        movableTiles[count].transform.position = new Vector3(transform.position.x + x, 1.1f, transform.position.z + z);
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
                            for (int x = -thisPiece.potentialRange; x <= thisPiece.potentialRange; x++) {
                                for (int z = -thisPiece.potentialRange; z <= thisPiece.potentialRange; z++) {
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
            selected = false;
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
}