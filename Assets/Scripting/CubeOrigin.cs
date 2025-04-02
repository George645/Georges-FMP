using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.UIElements;
using UnityEditor.Rendering;
using NUnit.Framework;
using System.Collections.Generic;

public class SquareColourCorrection : CubeBase {
    public static GameObject cube;
    public GameObject cubetemp;
    public int sizeNumber = 1;
    private bool running;
    int maxValue;
    Mode mode = Mode.gaming;
    [SerializeField]
    AnimationCurve animationCurve;

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
        StartCoroutine(SpawnSurrounding());
    }
    void Update(){

    }

    void GenerateLevel() {
        List<GameObject> allTheSquares = new();
        for (int x = 1; x <= 100; x++) {
            for (int y = 0; y <= 100; y++) {
                //Animation curve 200 * x = combined distance to the square, subtract 0.4 from the animation curve, add plus or minus 0.5
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - 0.4 + Random.Range(-0.5f, 0.5f) < 0 && y != 0) {
                    continue;
                }
                Debug.Log(((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200));
                Debug.Log(animationCurve.Evaluate(1-(x + y) / 200));

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
            }
        }
        for (int x = -1; x >= -100; x--) {
            for (int y = 0; y >= -100; y--) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - 0.4 + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
            }
        }
        for (int y = 1; y <= 100; y++) {
            for (int x = 0; x >= -100; x--) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - 0.4 + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
            }
        }
        for (int y = -1; y >= -100; y--) {
            for (int x = 0; x <= 100; x++) {
                if (animationCurve.Evaluate((float)(Mathf.Abs(x) + Mathf.Abs(y)) / 200) - 0.4 + Random.Range(-0.5f, 0.5f) < 0) {
                    continue;
                }

                //add each square to a list when they are created
                allTheSquares.Add(Instantiate(cube, new Vector3(x, 0.5f, y), Quaternion.identity, transform));
            }
        }
            //repeat 4 times, one for each quadrant away from this
            //get each square starting from this one to check itself and its neighbours to ensure that they are all directly connected back to the starting one
        }

    public void setSizeOfBoard(Slider slider) {
        sizeNumber = (int)slider.value;
        maxValue = (int)slider.maxValue;
        if (false == running) {
            StartCoroutine(SpawnSurrounding());
        }
    }

    IEnumerator SpawnSurrounding() {
        if (mode == Mode.levelling) {
            StartCoroutine(RemoveSurrounding());
            running = true;
            for (int i = 0; i <= sizeNumber; i++) {
                for (int j = -i; j <= i - 1; j++) {
                    for (int k = -i + 1; k <= i; k++) {
                        if ((j == 0 && k == 0) || GetSquareInDirection(transform, j, k) != null) {
                            continue;
                        }
                        Instantiate(cube, new Vector3(j, 0.5f, k), Quaternion.identity, transform);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
                for (int k = i; k >= -i; k--) {
                    for (int j = i; j >= -i; j--) {
                        if ((j == 0 && k == 0) || GetSquareInDirection(transform, j, k) != null) {
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
    }
    IEnumerator RemoveSurrounding() {
        for (int i = maxValue * 2; i>=0; i--) {
            for (int x = maxValue; x >= 0; x--) {
                int z = i - x;
                if (z > maxValue) {
                    break;
                }
                
                if(Mathf.Abs(z) <= sizeNumber && Mathf.Abs(x) <= sizeNumber || GetSquareInDirection(transform, z, x) == null && GetSquareInDirection(transform, z, -x) == null && GetSquareInDirection(transform, -z, x) == null && GetSquareInDirection(transform, -z, -x) == null) {
                    continue;
                }
                Destroy(GetSquareInDirection(transform, z, x));
                Destroy(GetSquareInDirection(transform, z, -x));
                Destroy(GetSquareInDirection(transform, -z, x));
                Destroy(GetSquareInDirection(transform, -z, -x));
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    void CreateEightByEightBoard() {
        SpawnSurrounding();
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