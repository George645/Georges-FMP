using System.Collections.Generic;
using UnityEngine;

public class OverarchingPieceMovement : MonoBehaviour {
    public static OverarchingPieceMovement Instance;
    public List<PieceMovement> allPieceMovement;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        foreach (PieceMovement pieceMovement in allPieceMovement) {
            pieceMovement.moveableTiles = new bool[(int)Mathf.Sqrt(pieceMovement.movableTiles1DArray.Length)][];
            for (int i = 0; i < pieceMovement.moveableTiles.Length; i++) {
                pieceMovement.moveableTiles[i] = new bool[pieceMovement.moveableTiles.Length];
            }
            for (int y = 0; y < pieceMovement.moveableTiles.Length; y++) {
                for (int x = 0; x < pieceMovement.moveableTiles.Length; x++) {
                    pieceMovement.moveableTiles[y][x] = pieceMovement.movableTiles1DArray[y * pieceMovement.moveableTiles.Length + x];
                }
            }
            pieceMovement.potentialRange = pieceMovement.potentialRange * 2 + 1;
        }
    }
}
