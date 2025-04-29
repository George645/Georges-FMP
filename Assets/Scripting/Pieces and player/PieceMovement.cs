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
    public bool infinitelyScalingRange = false;
    public int currentRange = 8;
    #endregion

    public bool PositionIsUnlocked(int x, int z) {
        try {
            if (moveableTiles[x + (potentialRange - 1) / 2][z + (potentialRange - 1) / 2]) {
                return true;
            }
            else {  return false; }
        }catch(IndexOutOfRangeException e) { return false; }
    }
    public bool CanLevelUp(int level, int capturedPieces){
        if (infinitelyScalingRange) {
            if ((level * level / 2) + (3 * level / 2) + 2 <= capturedPieces) {
                return true;
            }
        }
        else {
            if (level < 4) {
                if (level <= capturedPieces) {
                    return true;
                }
            }
            else{
                if (3 <= capturedPieces){
                    return true;
                }
            }
        }
        return false;
    }
    public void AttemptLevelUp(){
        if (infinitelyScalingRange){
            currentRange += 2;
        }
        else{

        }
    }
}


