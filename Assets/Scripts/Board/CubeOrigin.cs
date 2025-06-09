using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace TC{
    public class OriginCube : CubeBase {
        public static GameObject originCube;
        public static GameObject cube;
        public GameObject cubetemp;
        public int sizeNumber = 1;
        int currentSizeOfBoard = 1;
        private bool running;
        int maxValue = 10;
        [SerializeField]
        AnimationCurve animationCurve;
        GameObject newCube;
        bool firstFrame = true;
        bool coroutineRunning = false;
        public readonly static int MaxSizeOfBoard = 10;

        void Awake() {
            if (originCube == null) {
                originCube = gameObject;
            }
            if (blackMaterial == null) {
                blackMaterial = temporaryList[0];
                whiteMaterial = temporaryList[1];
            }
            if (cube == null) {
                cube = cubetemp;
            }
            AlignToGridAndColour();
            if (mode == Mode.gaming) {
                //if (TC_Menu.MenuHandling.boardState == null) {
                GenerateLevel();
                //}//*/
                //else {
                //RegenerateLevel();
                //}
                Gamestate.board = new bool[MaxSizeOfBoard * 2 + 1, MaxSizeOfBoard * 2 + 1];
                Gamestate.board[MaxSizeOfBoard, MaxSizeOfBoard] = true;
            }
        }
        private void RegenerateLevel() {
            for (int i = 0; i <= MaxSizeOfBoard * 2; i++) {
                for (int j = 0; j <= MaxSizeOfBoard * 2; j++) {
                    if (i == MaxSizeOfBoard && j == MaxSizeOfBoard) continue;
                    if (Gamestate.DoesPositionExist(new Vector2Int(i - MaxSizeOfBoard, j - MaxSizeOfBoard))) {
                        Instantiate(cube, new Vector3(i - MaxSizeOfBoard, 0.5f, j - MaxSizeOfBoard), Quaternion.identity, transform);
                    }/*
                if (MenuHandling.boardState.PieceInPosition(new Vector2Int(i, j)) != null){
                    Instantiate(MenuHandling.boardState.PieceInPosition(new Vector2Int(i, j)).thisObject.gameObject);
                }*/
                }
            }
        }
        private void Start() {
            if (mode == Mode.levelling) {
                StartCoroutine(SpawnSurrounding());
                currentSizeOfBoard = sizeNumber;
            }
        }
        void Update() {
            if (mode == Mode.levelling && currentSizeOfBoard != sizeNumber && !coroutineRunning) {
                currentSizeOfBoard = sizeNumber;
                StartCoroutine(SpawnSurrounding());
            }
            if (firstFrame) {
                BoundaryFill();
                firstFrame = false;
            }
        }

        void BoundaryFill() {
            for (int x = -MaxSizeOfBoard; x < MaxSizeOfBoard; x++) {
                for (int z = -MaxSizeOfBoard; z < MaxSizeOfBoard; z++) {
                    Vector3 suggestedPosition = new(x, 0.5f, z);
                    if (GetSquareInDirection(x, z) == null && CubeInDirection(suggestedPosition, new Vector3(10, 0.5f, 0)) && CubeInDirection(suggestedPosition, new Vector3(-10, 0.5f, 0)) && CubeInDirection(suggestedPosition, new Vector3(0, 0.5f, 10)) && CubeInDirection(suggestedPosition, new Vector3(0, 0.5f, -10))) {
                        newCube = Instantiate(cube, new Vector3(x, 0, z), Quaternion.identity, transform);
                        newCube.name = System.Convert.ToString(Random.Range(0, 1000000));
                        newCube.GetComponent<CubeBase>().connectsToCenter = true;
                    }
                }
            }
        }

        void GenerateLevel() {
            List<GameObject> allTheSquares = new();

            float offset = 0.5f;

            for (int x = 1; x <= MaxSizeOfBoard; x++) {
                for (int y = 0; y <= MaxSizeOfBoard; y++) {
                    //Animation curve 200 * x = combined distance to the square, subtract 0.4 from the animation curve, add plus or minus 0.5
                    if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / (MaxSizeOfBoard * 2)) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                        continue;
                    }

                    //add each square to a list when they are created
                    allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                    allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
                }
            }
            //repeat 4 times, one for each quadrant away from this
            for (int x = -1; x >= -MaxSizeOfBoard; x--) {
                for (int y = 0; y >= -MaxSizeOfBoard; y--) {
                    if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / (MaxSizeOfBoard * 2)) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                        continue;
                    }

                    //add each square to a list when they are created
                    allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                    allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
                }
            }
            for (int y = 1; y <= MaxSizeOfBoard; y++) {
                for (int x = 0; x >= -MaxSizeOfBoard; x--) {
                    if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / (MaxSizeOfBoard * 2)) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                        continue;
                    }

                    //add each square to a list when they are created
                    allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                    allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
                }
            }
            for (int y = -1; y >= -MaxSizeOfBoard; y--) {
                for (int x = 0; x <= MaxSizeOfBoard; x++) {
                    if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / (MaxSizeOfBoard * 2)) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                        continue;
                    }

                    //add each square to a list when they are created
                    allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                    allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
                }
            }
            new Gamestate(null, null);
            //get each square starting from this one to check itself and its neighbours to ensure that they are all directly connected back to the starting one
            ConnectsToCenter();
            foreach (GameObject child in allTheSquares.Where(child => child.GetComponent<CubeBase>().connectsToCenter == false)) {
                Destroy(child);
            }
        }

        public void SetSizeOfBoard(Slider slider) {
            sizeNumber = (int)slider.value;
            maxValue = (int)slider.maxValue;
            if (false == running) {
                StartCoroutine(SpawnSurrounding());
            }
        }

        bool CubeInDirection(Vector3 origin, Vector3 dir) {
            if (Physics.Raycast(origin, dir, 100)) {
                return true;
            }

            return false;
        }

        IEnumerator SpawnSurrounding() {
            coroutineRunning = true;
            if (mode == Mode.levelling) {
                StartCoroutine(RemoveSurrounding());
                running = true;
                for (int i = 0; i <= sizeNumber; i++) {
                    for (int j = -i; j <= i - 1; j++) {
                        for (int k = -i + 1; k <= i; k++) {
                            if ((j == 0 && k == 0) || GetSquareInDirection(transform.position, j, k) != null) {
                                continue;
                            }
                            Instantiate(cube, new Vector3(transform.position.x + j, transform.position.y, transform.position.z + k), Quaternion.identity, transform);
                            //yield return new WaitForSeconds(0.02f);
                        }
                    }
                    for (int k = i; k >= -i; k--) {
                        for (int j = i; j >= -i; j--) {
                            if ((j == 0 && k == 0) || GetSquareInDirection(transform.position, j, k) != null) {
                                continue;
                            }
                            Instantiate(cube, new Vector3(transform.position.x + j, transform.position.y, transform.position.z + k), Quaternion.identity, transform);
                            //yield return new WaitForSeconds(0.02f);
                        }
                    }
                }
                running = false;
                coroutineRunning = false;
                yield return null;
            }
            else {
                yield return null;
            }
            ConnectsToCenter();
        }
        IEnumerator RemoveSurrounding() {
            for (int i = maxValue * 2; i >= 0; i--) {
                for (int x = maxValue; x >= 0; x--) {
                    int z = i - x;
                    if (z > maxValue) {
                        break;
                    }
                    if (Mathf.Abs(z) <= sizeNumber && Mathf.Abs(x) <= sizeNumber || GetSquareInDirection(transform.position, z, x) == null && GetSquareInDirection(transform.position, z, -x) == null && GetSquareInDirection(transform.position, -z, x) == null && GetSquareInDirection(transform.position, -z, -x) == null) {
                        continue;
                    }

                    Destroy(GetSquareInDirection(transform.position, z, x));
                    Destroy(GetSquareInDirection(transform.position, z, -x));
                    Destroy(GetSquareInDirection(transform.position, -z, x));
                    Destroy(GetSquareInDirection(transform.position, -z, -x));
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
}

/*[CustomEditor(typeof(SquareColourCorrection))]
public class SquareColourCorrectionEditor : Editor {
    public override void OnInspectorGUI() {
        SquareColourCorrection script = (SquareColourCorrection)target;
        GUIContent content = new("One of the materials: ", "Only needs to be on one object, but preferably all");
        script.temporaryList[0] = EditorGUILayout.ObjectField(content, /*type*//*);
        

    }
}
*/