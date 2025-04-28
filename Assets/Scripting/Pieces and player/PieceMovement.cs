using UnityEngine;
using System;

[Serializable]
//[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class PieceMovement {

    public string name;
    #region handled by custom editor
    [SerializeField]
    public bool[] movableTiles1DArray = new bool[9];
    public bool[][] moveableTiles = new bool[0][];
    [HideInInspector]
    public int potentialRange = 3;
    #endregion

    public bool PositionIsUnlocked(int x, int z) {
        try {
            if (moveableTiles[x + (potentialRange - 1) / 2][z + (potentialRange - 1) / 2]) {
                return true;
            }
            else {  return false; }
        }catch(IndexOutOfRangeException e) { return false; }
    }
}


