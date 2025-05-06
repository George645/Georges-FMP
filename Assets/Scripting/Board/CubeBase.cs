using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {
    public List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;
    public bool connectsToCenter = false;
    [SerializeField]
    protected Mode mode = Mode.gaming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        try {
            blackMaterial = temporaryList[0];
            whiteMaterial = temporaryList[1];
        }
        catch { }
        AlignToGridAndColour();
        /*if (connectsToCenter == false && mode == Mode.gaming) {
            Destroy(this.gameObject);
        }*/
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
    public static GameObject GetSquareInDirection(Vector3 origin, float x, float z) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(origin.x + x, -0.2f, origin.z + z), new Vector3(0, 1, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo, 1)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        return (returningGameObject);
    }
    public static GameObject GetSquareInDirection(float x, float z) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(x, -0.2f, z), new Vector3(0, 1, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo, 1)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        return (returningGameObject);

    }
    public void ConnectsToCenter() {
        connectsToCenter = true;
        if (transform.position.x >= 0 && Math.Abs(transform.position.x) >= Math.Abs(transform.position.z)) {
            if (GetSquareInDirection(transform.position, 1, 0) != null) {
                GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 0, 1) != null && GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 0, -1) != null && GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
            }
        }
        if (transform.position.x <= 0 && Math.Abs(transform.position.x) >= Math.Abs(transform.position.z)) {
            if (GetSquareInDirection(transform.position, -1, 0) != null) {
                GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 0, 1) != null && GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 0, -1) != null && GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
            }
        }
        if (transform.position.z >= 0 && Math.Abs(transform.position.z) > Math.Abs(transform.position.x)) {
            if (GetSquareInDirection(transform.position, 0, 1) != null) {
                GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 1, 0) != null && GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, -1, 0) != null && GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
        }
        if (transform.position.z <= 0 && Math.Abs(transform.position.z) > Math.Abs(transform.position.x)) {
            if (GetSquareInDirection(transform.position, 0, -1) != null) {
                GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, 1, 0) != null && GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
            if (GetSquareInDirection(transform.position, -1, 0) != null && GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
            }
        }
    }
}
