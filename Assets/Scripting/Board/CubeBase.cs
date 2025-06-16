using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeBase : MonoBehaviour {
    public List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;
    //[HideInInspector]
    public bool connectsToCenter = false;
    [SerializeField]
    protected Mode mode = Mode.gaming;
    private void Awake() {
        connectsToCenter = false;
    }
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
        try {
            returningGameObject.GetComponent<CubeBase>();
        }
        catch {
            returningGameObject = null;
        }
        return (returningGameObject);
    }
    public static GameObject GetSquareInDirection(float x, float z) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(x, -0.2f, z), new Vector3(0, 1, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo, 1)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        try {
            returningGameObject.GetComponent<CubeBase>();
        }
        catch {
            returningGameObject = null;
        }
        return (returningGameObject);
    }
    public static GameObject GetSquareInDirection(Vector2Int input) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(input.x, -0.2f, input.y), new Vector3(0, 1, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo, 1)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        try {
            returningGameObject.GetComponent<CubeBase>();
        }
        catch {
            returningGameObject = null;
        }
        return (returningGameObject);
    }
    public void ConnectsToCenter(int depth) {
        connectsToCenter = true;
        Debug.Log(depth);
        if (depth > 50) return;
        Gamestate.board[(int)transform.position.x + OriginCube.MaxSizeOfBoard, (int)transform.position.z + OriginCube.MaxSizeOfBoard] = true;
        try {
            if (!Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)) && GetSquareInDirection(transform.position, 1, 0) != null && GetSquareInDirection(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
                GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter(depth++);
            }
        }
        catch (NullReferenceException e) {
            Debug.Log(GetSquareInDirection(transform.position, 1, 0));
            throw e;
        }
        if (!Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)) && GetSquareInDirection(transform.position, 0, 1) != null && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)).GetComponent<CubeBase>().connectsToCenter == false) {
            GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter(depth++);
        }
        if (!Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)) && GetSquareInDirection(transform.position, 0, -1) != null && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)).GetComponent<CubeBase>().connectsToCenter == false) {
            GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter(depth++);
        }
        if (!Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)) && GetSquareInDirection(transform.position, -1, 0) != null && GetSquareInDirection(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
            GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter(depth++);
        }
    }
}