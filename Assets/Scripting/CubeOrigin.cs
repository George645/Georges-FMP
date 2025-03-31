using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SquareColourCorrection : CubeBase {
    public static GameObject cube;
    public GameObject cubetemp;
    public int sizeNumber;
    private bool running;
    int maxValue;

    void Awake(){
        if (blackMaterial == null) {
            blackMaterial = temporaryList[0];
            whiteMaterial = temporaryList[1];
            cube = cubetemp;
        }
        AlignToGridAndColour();
    }
    private void Start() {
        //Debug.Log(GetSquareInDirection(-1, 0));
        StartCoroutine(SpawnSurrounding());
    }
    void Update(){

    }

    public void setSizeOfBoard(Slider slider) {
        sizeNumber = (int)slider.value/* * 2 + 1*/;
        maxValue = (int)slider.maxValue;
        if (false == running) {
            StartCoroutine(SpawnSurrounding());
            StartCoroutine(RemoveSurrounding());
        }
    }

    IEnumerator SpawnSurrounding() {
        running = true;
        for (int i = 0; i <= sizeNumber; i++) {
            for (int j = -i; j <= i - 1; j++) {
                for (int k = -i + 1; k <= i; k++) {
                    if ((j == 0 && k == 0) || GetSquareInDirection(j, k) != null){
                        continue;
                    }
                    Instantiate(cube, new Vector3(j, 0.5f, k), Quaternion.identity);
                    yield return new WaitForSeconds(0.02f);
                }
            }
            for (int k = i; k >= -i; k--) {
                for (int j = i; j >= -i; j--) {
                    if ((j == 0 && k == 0) || GetSquareInDirection(j, k) != null) {
                        continue;
                    }
                    Instantiate(cube, new Vector3(j, 0.5f, k), Quaternion.identity);
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
        running = false;
        yield return null;
    }
    IEnumerator RemoveSurrounding() {
        for (int i = maxValue * 2; i>=0; i--) {
            for (int x = maxValue; x >= 0; x--) {
                int z = i - x;
                if (z > maxValue) {
                    break;
                }
                
                if(Mathf.Abs(z) <= sizeNumber && Mathf.Abs(x) <= sizeNumber || (GetSquareInDirection(z, x) && GetSquareInDirection(z, -x) && GetSquareInDirection(-z, x) && GetSquareInDirection(-z, -x)) == null) {
                    continue;
                }

                
                Debug.Log(x + ", " + z + ", " + GetSquareInDirection(x, z));
                Destroy(GetSquareInDirection(z, x));
                Destroy(GetSquareInDirection(z, -x));
                Destroy(GetSquareInDirection(-z, x));
                Destroy(GetSquareInDirection(-z, -x));
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