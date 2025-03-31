using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SquareColourCorrection : CubeBase {
    private int thisNumber = 0;
    public static GameObject cube;
    public GameObject cubetemp;
    public int sizeNumber;
    private bool running;

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
        if (thisNumber == 0) {
            CreateEightByEightBoard();
        }
        StartCoroutine(SpawnSurrounding());
    }
    void Update(){
    }

    public void setSizeOfBoard(Slider slider) {
        sizeNumber = (int)slider.value/* * 2 + 1*/;
        if (false == running) {
            StartCoroutine(SpawnSurrounding());
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