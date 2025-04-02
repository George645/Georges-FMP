using UnityEngine;
using System;

public class PieceBase : MonoBehaviour{

    [SerializeField]
    Piece thisPiece;

    void Start(){
        while (CubeBase.GetSquareInDirection(transform, 0, 0) == null) {
            System.Random random = new System.Random();
            int newX = random.Next(-100, 100);
            int newZ = random.Next(-100, 100);
            if (Physics.Raycast(new Vector3(newX, 3.2f, newZ), new Vector3(0, -1, 0)) == false){
                transform.position = new Vector3(newX, 2, newZ);
            }
        }
    }

    void Update(){
        
    }
}
