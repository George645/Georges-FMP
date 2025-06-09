using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC {
    public class CubeBase : MonoBehaviour {
        public List<Material> temporaryList = new();
        public static Material blackMaterial;
        public static Material whiteMaterial;
        [HideInInspector]
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
        private void Update() {
            try {
                if (connectsToCenter && SceneManager.GetActiveScene().name == "Main gameplay scene" && Gamestate.DoesPositionExist(new Vector2Int())) Gamestate.board[(int)transform.position.x + 100, (int)transform.position.z + 100] = true;
                else if (SceneManager.GetActiveScene().name == "Main gameplay scene") Destroy(gameObject);
            }
            catch (IndexOutOfRangeException) {
                throw new IndexOutOfRangeException(transform.position + "was the position that was out of range when adding 100 to both x and z");
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
        public static GameObject GetSquareInDirection(Vector2Int input) {
            GameObject returningGameObject = null;
            Ray newRay = new(new Vector3(input.x, -0.2f, input.y), new Vector3(0, 1, 0));
            if (Physics.Raycast(newRay, out RaycastHit hitInfo, 1)) {
                returningGameObject = hitInfo.collider.gameObject;
            }
            return (returningGameObject);
        }
        public void ConnectsToCenter() {
            connectsToCenter = true;
            if (transform.position.x >= 0 && Math.Abs(transform.position.x) >= Math.Abs(transform.position.z)) {
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0))) {
                    GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
                }
            }
            if (transform.position.x <= 0 && Math.Abs(transform.position.x) >= Math.Abs(transform.position.z)) {
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0))) {
                    GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
                }
            }
            if (transform.position.z >= 0 && Math.Abs(transform.position.z) > Math.Abs(transform.position.x)) {
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + 1))) {
                    GetSquareInDirection(transform.position, 0, 1).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
            }
            if (transform.position.z <= 0 && Math.Abs(transform.position.z) > Math.Abs(transform.position.x)) {
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 0, (int)transform.position.z + -1))) {
                    GetSquareInDirection(transform.position, 0, -1).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + 1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, 1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
                if (Gamestate.DoesPositionExist(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)) && GetSquareInDirection(new Vector2Int((int)transform.position.x + -1, (int)transform.position.z + 0)).GetComponent<CubeBase>().connectsToCenter == false) {
                    GetSquareInDirection(transform.position, -1, 0).GetComponent<CubeBase>().ConnectsToCenter();
                }
            }
        }
    }
}