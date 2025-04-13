using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class SquareColourCorrection : CubeBase {
    public static GameObject cube;
    public GameObject cubetemp;
    public int sizeNumber = 1;
    private bool running;
    int maxValue;
    [SerializeField]
    AnimationCurve animationCurve;
    int boundaryFillCount = 0;
    GameObject newCube;
    bool firstFrame = true;

    void Awake(){
        if (blackMaterial == null) {
            blackMaterial = temporaryList[0];
            whiteMaterial = temporaryList[1];
            cube = cubetemp;
        }
        AlignToGridAndColour();
        if (mode == Mode.gaming) {
            GenerateLevel();
        }
    }
    private void Start() {
        if (mode == Mode.levelling) {
            StartCoroutine(SpawnSurrounding());
        }
    }
    void Update() {
        if (firstFrame) {
            for (int x = -100; x < 100; x++) {
                for (int z = -100; z < 100; z++) {
                    Vector3 suggestedPosition = new Vector3(x, 0.5f, z);
                    if (GetSquareInDirection(x, z) == null && CubeInDirection(suggestedPosition, new Vector3(10, 0.5f, 0)) && CubeInDirection(suggestedPosition, new Vector3(-10, 0.5f, 0)) && CubeInDirection(suggestedPosition, new Vector3(0, 0.5f, 10)) && CubeInDirection(suggestedPosition, new Vector3(0, 0.5f, -10))) {
                        newCube = Instantiate(cube, new Vector3(x, 0, z), Quaternion.identity, transform);
                        newCube.GetComponent<CubeBase>().connectsToCenter = true;
                    }
                }
            }
            firstFrame = false;
        }
    }

    void GenerateLevel() {
        List<GameObject> allTheSquares = new();

        float offset = 0.5f;

        for (int x = 1; x <= 100; x++) {
            for (int y = 0; y <= 100; y++) {
                //Animation curve 200 * x = combined distance to the square, subtract 0.4 from the animation curve, add plus or minus 0.5
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
            }
        }
        //repeat 4 times, one for each quadrant away from this
        for (int x = -1; x >= -100; x--) {
            for (int y = 0; y >= -100; y--) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
            }
        }
        for (int y = 1; y <= 100; y++) {
            for (int x = 0; x >= -100; x--) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
            }
        }
        for (int y = -1; y >= -100; y--) {
            for (int x = 0; x <= 100; x++) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - offset + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
                allTheSquares.LastOrDefault().name = System.Convert.ToString(Mathf.RoundToInt(Random.Range(0, 1000000)));
            }
        }
        List<GameObject> removingSquares = new();
        //get each square starting from this one to check itself and its neighbours to ensure that they are all directly connected back to the starting one
        ConnectsToCenter();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<CubeBase>().connectsToCenter == false) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void setSizeOfBoard(Slider slider) {
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
        if (mode == Mode.levelling) {
            StartCoroutine(RemoveSurrounding());
            running = true;
            for (int i = 0; i <= sizeNumber; i++) {
                for (int j = -i; j <= i - 1; j++) {
                    for (int k = -i + 1; k <= i; k++) {
                        if ((j == 0 && k == 0) || GetSquareInDirection(transform.position, j, k) != null) {
                            continue;
                        }
                        Instantiate(cube, new Vector3(j, 0.5f, k), Quaternion.identity, transform);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
                for (int k = i; k >= -i; k--) {
                    for (int j = i; j >= -i; j--) {
                        if ((j == 0 && k == 0) || GetSquareInDirection(transform.position, j, k) != null) {
                            continue;
                        }
                        Instantiate(cube, new Vector3(j, 0.5f, k), Quaternion.identity, transform);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
            }
            running = false;
            yield return null;
        }
        else {
            yield return null;
        }
        ConnectsToCenter();
    }
    IEnumerator RemoveSurrounding() {
        for (int i = maxValue * 2; i>=0; i--) {
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

/*[CustomEditor(typeof(SquareColourCorrection))]
public class SquareColourCorrectionEditor : Editor {
    public override void OnInspectorGUI() {
        SquareColourCorrection script = (SquareColourCorrection)target;
        GUIContent content = new("One of the materials: ", "Only needs to be on one object, but preferably all");
        script.temporaryList[0] = EditorGUILayout.ObjectField(content, /*type*//*);
        

    }
}
*/