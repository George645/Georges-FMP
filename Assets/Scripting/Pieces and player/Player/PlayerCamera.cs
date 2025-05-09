using System;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    GameObject player;
    public Mode mode = Mode.gaming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player = Player.player;
    }

    // Update is called once per frame
    void Update() {
        if (mode == Mode.gaming) {
            FaceCamera(player.transform.position);
            WhileRightButtonPressed(player.transform.position);
        }
        else {
            FaceCamera(new Vector3(500, 0.5f, 500));
            WhileRightButtonPressed(new Vector3(500, 0.5f, 500));
        }
        Zoom();
        SelectingPieces();
    }

    #region Selecting a piece
    RaycastHit objecta;
    GameObject objectaObject;

    void SelectingPieces() {
        if (Input.GetMouseButtonUp(0)) {
            try {
                objectaObject.GetComponent<UnderlyingPiece>().selected = false;
            }
            catch (NullReferenceException) { }
            catch (MissingReferenceException) { objectaObject = null; }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out objecta)) {
                if (!objecta.collider.gameObject.name[0..1].ContainsAny("1", "2", "3", "4", "5", "6", "7", "8", "9", "0")) {
                    objectaObject = objecta.collider.gameObject;
                    try {
                        objectaObject.GetComponent<UnderlyingPiece>().selected = true;
                    }
                    catch (MissingReferenceException) { }
                    catch (NullReferenceException) { }
                }
            }
        }
    }

    #endregion

    #region Camera rotation and tilt
    float distanceSpun = 0;
    [Range(-20, 180)]
    float tilt = 20;
    Vector2 previousFrameMousePos = Vector2.zero;
    Vector2 relativeMousePos = Vector2.zero;
    float newRotationXNormalised = 0;
    float newRotationZNormalised = 0;
    Vector2 rotation;
    Vector3 relativeCameraPosition;
    void WhileRightButtonPressed(Vector3 rotateAround) {
        if (Input.GetMouseButton(1)) {
            relativeMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - previousFrameMousePos;
            distanceSpun += (float)relativeMousePos.x / 500 * PlayerPrefs.GetInt("Sensitivity", 70);
            if (distanceSpun < -180) distanceSpun += 360;
            else if (distanceSpun > 180) distanceSpun -= 360;
            tilt += -relativeMousePos.y / 500 * PlayerPrefs.GetInt("Sensitivity", 70);
            tilt = math.clamp(tilt, -5, 90);
            //calculate the rotation around the player
            newRotationXNormalised = Mathf.Cos(Mathf.Deg2Rad * distanceSpun);
            newRotationZNormalised = Mathf.Sin(Mathf.Deg2Rad * distanceSpun);
            rotation = new Vector2(newRotationXNormalised, newRotationZNormalised);

            //calculate the height that the player should be at
            rotation *= Mathf.Cos(Mathf.Deg2Rad * tilt);
            relativeCameraPosition = new Vector3(rotation.x, Mathf.Sin(Mathf.Deg2Rad * tilt), rotation.y);
        }


        gameObject.transform.position = Vector3.Lerp(transform.position, rotateAround + new Vector3(relativeCameraPosition.x * zoomInScale, Math.Clamp(relativeCameraPosition.y * zoomInScale, -.5f, 200), relativeCameraPosition.z * zoomInScale), 0.1f);
        previousFrameMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    #endregion

    void FaceCamera(Vector3 target) {
        //potentially handled by virtual camera
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 0.3f);
    }


    float zoomInScale = 10;
    void Zoom() {
        zoomInScale = math.clamp(zoomInScale + Input.mouseScrollDelta.y, 3, 200);
    }
}
