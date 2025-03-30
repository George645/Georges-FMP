using UnityEngine;
using System.Collections.Generic;

public class SquareColourCorrection : MonoBehaviour{
    [SerializeField]
    List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;
    public int thisNumber = 0;
    public static GameObject cube;
    public GameObject cubetemp;

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
        if (thisNumber > 0 && thisNumber < ) {
            SpawnSurrounding();
        }
    }
    void Update(){
        if (GetSquareInDirection(-1, -1) == null && GetSquareInDirection(-1, 0) == null && GetSquareInDirection(-1, 1) == null && GetSquareInDirection(0, 1) == null && GetSquareInDirection(1, 1) == null && GetSquareInDirection(1, 0) == null && GetSquareInDirection(1, -1) == null && GetSquareInDirection(0, -1)) {
            Destroy(this);
        }
    }

    void AlignToGridAndColour() {
        transform.position = new Vector3(Mathf.Round(transform.position.x), 0.5f, Mathf.Round(transform.position.z));
        if ((transform.position.x + transform.position.z) % 2 == 0) {
            gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
        }
        else {
            gameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
        }
    }

    public void SpawnSurrounding() {
        GameObject currentCube;
        if (GetSquareInDirection(-1, -1) == null) {
            //Debug.Log(this);
            //Debug.Log(GetSquareInDirection(-1, -1));
            currentCube = Instantiate(cube, new Vector3(transform.position.x - 1, 0, transform.position.z - 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(-1, 0) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x - 1, 0, transform.position.z), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(-1, 1) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x - 1, 0, transform.position.z + 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(0, -1) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x, 0, transform.position.z - 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(0, 1) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x, 0, transform.position.z + 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(1, -1) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x + 1, 0, transform.position.z - 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(1, 0) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x + 1, 0, transform.position.z), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
        if (GetSquareInDirection(1, 1) == null) {
            currentCube = Instantiate(cube, new Vector3(transform.position.x + 1, 0, transform.position.z + 1), Quaternion.identity);
            currentCube.GetComponent<SquareColourCorrection>().thisNumber = thisNumber + 1;
        }
    }

    void CreateEightByEightBoard() {
        SpawnSurrounding();
    }

    GameObject GetSquareInDirection(float x, float y) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(transform.position.x + x, -0.2f, transform.position.z + y), new Vector3(0, 100, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        return (returningGameObject);
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