using TC;
using UnityEngine;

public class Cameramovement : MonoBehaviour{
    GameObject mainCamera;
    Vector2 previousPosition;
    [SerializeField]
    GameObject followee;
    void Start(){
        mainCamera = Camera.main.gameObject;
        previousPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void Update(){
        if (Input.GetMouseButton(2)) {
            MoveCamera();
        }
        previousPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    void MoveCamera() {
        Vector2 deltaMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - previousPosition;
        Vector2 changeInCameraPosition = new Vector2(-deltaMousePosition.x * Camera.main.transform.right.x + deltaMousePosition.y * Camera.main.transform.right.z, -deltaMousePosition.x * Camera.main.transform.right.z + deltaMousePosition.y * -Camera.main.transform.right.x) * 0.7f / PlayerPrefs.GetInt("Sensitivity", 70);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + changeInCameraPosition.x, -OriginCube.MaxSizeOfBoard, OriginCube.MaxSizeOfBoard), transform.position.y, Mathf.Clamp(transform.position.z + changeInCameraPosition.y, -OriginCube.MaxSizeOfBoard, OriginCube.MaxSizeOfBoard));
        followee.transform.position += new Vector3(changeInCameraPosition.x, 0, changeInCameraPosition.y);
    }
}
