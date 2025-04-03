using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {
    public List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;
    public bool connectsToCenter = false;
    protected Mode mode = Mode.gaming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlignToGridAndColour();
    }

    // Update is called once per frame
    void Update(){
        if (connectsToCenter == false && mode == Mode.gaming) {
            Destroy(this.gameObject);
        }
    }
    public void AlignToGridAndColour() {
        transform.position = new Vector3(Mathf.Round(transform.position.x), 0.5f, Mathf.Round(transform.position.z));
        if ((transform.position.x + transform.position.z) % 2 == 0) {
            gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
        }
        else {
            gameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
        }
    }
    public static GameObject GetSquareInDirection(Transform origin, float x, float y) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(origin.transform.position.x + x, -0.2f, origin.transform.position.z + y), new Vector3(0, 100, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        return (returningGameObject);
    }
    public void ConnectsToCenter() {
        connectsToCenter = true;
        for (int i = -1; i <= 1; i += 2){
            if (GetSquareInDirection(transform, i, 0) != null && GetSquareInDirection(transform, i, 0).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform, i, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform, 0, i) != null && GetSquareInDirection(transform, 0, i).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform, 0, i).GetComponent<CubeBase>().ConnectsToCenter();
            }
        }
    }
}
