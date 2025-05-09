using UnityEngine;
using System;
[Serializable]
//[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class PieceMovement {
    [SerializeField]
    public Mesh thisPiece;
    [SerializeField]
    public Material[] playerTeamMaterial;
    [SerializeField]
    public Material[] enemyTeamMaterial;
    public string name;
    #region handled by custom editor
    [SerializeField]
    public bool[] movableTiles1DArray = new bool[9];
    public bool[][] moveableTiles = new bool[0][];
    [HideInInspector]
    public int potentialRange;
    public bool infinitelyScalingRange = false;
    public int currentRange = 8;
    #endregion

    public bool PositionIsUnlocked(int x, int z) {
        try {
            if (moveableTiles[x + potentialRange][z + potentialRange]) {
                return true;
            }
            else { return false; }
        }
        catch (IndexOutOfRangeException) { return false; }
    }
    public bool CanLevelUp(int level, int capturedPieces) {
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
            else {
                if (3 <= capturedPieces) {
                    return true;
                }
            }
        }
        return false;
    }
    public void AttemptLevelUp() {
        if (infinitelyScalingRange) {
            currentRange += 2;
        }
        else {

        }
    }
    public void AddPositionToMovables(Vector2Int offset) {
        moveableTiles[offset.x + potentialRange][offset.y + potentialRange] = true;
    }
    public void ExpandSize() {
        bool[][] tempArray = new bool[moveableTiles.Length + 2][];
        for (int i = 0; i < tempArray.Length; i++) {
            tempArray[i] = new bool[moveableTiles.Length + 2];
        }
        for (int x = 0; x < moveableTiles.Length; x++) {
            for (int z = 0; z < moveableTiles.Length; z++) {
                tempArray[x + 1][z + 1] = moveableTiles[x][z];
            }
        }
        try {
            LevelUpOriginCubeIdentifier.instance.GetComponent<OriginCube>().sizeNumber = (tempArray.Length - 1) / 2;
        }
        catch (NullReferenceException){ }
        potentialRange += 2;
        moveableTiles = new bool[tempArray.Length][];
        for (int i = 0; i < moveableTiles.Length; i++) {
            moveableTiles[i] = new bool[moveableTiles.Length];
            moveableTiles[i] = tempArray[i];
        }
        moveableTiles = tempArray;
    }
}