using UnityEngine;
using System.Collections.Generic;

public class SquareColourCorrection : MonoBehaviour{
    [SerializeField]
    List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;

    void Awake(){
        if (blackMaterial == null) {
            blackMaterial = temporaryList[0];
            whiteMaterial = temporaryList[1];
        }
        AlignToGrid();
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

    GameObject GetSquareInDirection(float x, float y) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(transform.position.x + x, 0, transform.position.z), new Vector3(0, 1, 0));
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