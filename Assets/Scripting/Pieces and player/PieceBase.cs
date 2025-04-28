using UnityEngine;
using System;
using Unity.Mathematics;
using NUnit.Framework;
using System.Collections.Generic;

public class PieceBase : UnderlyingPiece{
    List<GameObject> movableTiles;

    void Start() {
        //base.ActualStart();
        FindStartPosition();
        if (selected) {
            if (firstFrameSelected) {
                int count = 0;
                for (int i = -(thisPiece.potentialRange - 1) / 2; i <= (thisPiece.potentialRange - 1) / 2; i++) {
                    for (int j = -(thisPiece.potentialRange - 1) / 2; j <= (thisPiece.potentialRange - 1) / 2; j++) {
                        if (thisPiece.PositionIsUnlocked(i, j)) {
                            movableTiles.Add(Moveabledisplays.Instance.GetObject());
                            movableTiles[count].SetActive(true);
                            movableTiles[count].GetComponent<MovementCircles>().OriginalObject = gameObject;
                            movableTiles[count].GetComponent<MovementCircles>().offset = new Vector2(i, j);
                            movableTiles[count].transform.position = new Vector3(transform.position.x + i, 1.1f, transform.position.z + j);
                            count++;
                        }
                    }
                }
                firstFrameSelected = false;
            }
        }
        else {
            firstFrameSelected = true;
            foreach (GameObject obj in movableTiles) {
                obj.SetActive(false);
            }
            movableTiles = new();
        }
    }

    void FindStartPosition() {
        transform.position = new Vector3(101, 0, 101);
        int newX = 101;
        int newZ = 101;
        int count = 0;
        System.Random random = new();
        bool whileLoopVariable = true;
        while (whileLoopVariable && count < 10) {
            random = new System.Random();
            newX = random.Next(-100, 100);
            newZ = random.Next(-100, 100);
            if (Physics.Raycast(new Vector3(newX, 3.2f, newZ), new Vector3(0, -1, 0), 1) == true) {
                newX = 101;
            }
            count++;
            CubeBase.GetSquareInDirection(newX, newZ);
            try {
                CubeBase.GetSquareInDirection(newX, newZ).GetComponent<MeshRenderer>().name = CubeBase.GetSquareInDirection(newX, newZ).GetComponent<MeshRenderer>().name;
                whileLoopVariable = false;
            }
            catch {
                whileLoopVariable = true;
            }
        }
        transform.position = new Vector3(newX, 2, newZ);
    }

    void Update() {
        if (CubeBase.GetSquareInDirection(transform.position, 0, 0) == null) {
            FindStartPosition();
        }
    }
}