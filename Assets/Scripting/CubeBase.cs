using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {
    public List<Material> temporaryList = new();
    public static Material blackMaterial;
    public static Material whiteMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlignToGridAndColour();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetSquareInDirection(-1, -1) == null && GetSquareInDirection(-1, 0) == null && GetSquareInDirection(-1, 1) == null && GetSquareInDirection(0, 1) == null && GetSquareInDirection(1, 1) == null && GetSquareInDirection(1, 0) == null && GetSquareInDirection(1, -1) == null && GetSquareInDirection(0, -1)) {
            Destroy(this);
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
    public GameObject GetSquareInDirection(float x, float y) {
        GameObject returningGameObject = null;
        Ray newRay = new(new Vector3(transform.position.x + x, -0.2f, transform.position.z + y), new Vector3(0, 100, 0));
        if (Physics.Raycast(newRay, out RaycastHit hitInfo)) {
            returningGameObject = hitInfo.collider.gameObject;
        }
        return (returningGameObject);
    }
}
