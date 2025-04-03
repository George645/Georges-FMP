using UnityEngine;
using System;

public class PieceBase : MonoBehaviour{

    [SerializeField]
    Piece thisPiece;

    void Start() {
        transform.position = new Vector3(101, 0, 101);
    }

    void Update() {

        while (CubeBase.GetSquareInDirection(transform, transform.position.x, transform.position.z) == null){
            System.Random random = new System.Random();
            int newX = random.Next(-100, 100);
            int newZ = random.Next(-100, 100);
            Debug.Log(Physics.Raycast(new Vector3(newX, 3.2f, newZ), new Vector3(0, -1, 0), 1));
            if (Physics.Raycast(new Vector3(newX, 3.2f, newZ), new Vector3(0, -1, 0), 1) == true) {
                transform.position = new Vector3(newX, 2, newZ);
            }
            Debug.Log(CubeBase.GetSquareInDirection(transform, transform.position.x, transform.position.z) + ", " + transform.position);
        }
    }
}
