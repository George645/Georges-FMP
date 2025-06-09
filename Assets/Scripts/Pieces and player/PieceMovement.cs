using UnityEngine;
using System;

namespace TC{
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
        public Vector2Int AIAccessiblePosition;
        public bool hasMoved = false;
        readonly bool playersTeam;
        public UnderlyingPiece thisObject;
        public PieceMovement inheritingPiece;
        #region handled by custom editor
        [SerializeField]
        public bool[] movableTiles1DArray = new bool[9];
        public bool[][] moveableTiles = new bool[0][];
        [HideInInspector]
        public int potentialRange;
        public bool infinitelyScalingRange = false;
        public int currentRange = 8;
        #endregion

        public PieceMovement(PieceMovement thing) {
            inheritingPiece = thing;
            #region Assign all variables
            thisObject = thing.thisObject;
            AIAccessiblePosition = inheritingPiece.AIAccessiblePosition;
            playersTeam = inheritingPiece.playersTeam;
            name = inheritingPiece.name;
            movableTiles1DArray = inheritingPiece.movableTiles1DArray;
            moveableTiles = inheritingPiece.moveableTiles;
            potentialRange = inheritingPiece.potentialRange;
            infinitelyScalingRange = inheritingPiece.infinitelyScalingRange;
            currentRange = inheritingPiece.currentRange;
            Array.Copy(inheritingPiece.moveableTiles, moveableTiles, inheritingPiece.moveableTiles.Length);
            #endregion

        }

        public PieceMovement(int position, bool isPlayersTeam, UnderlyingPiece thisObject) {
            playersTeam = isPlayersTeam;
            this.thisObject = thisObject;

            inheritingPiece = OverarchingPieceMovement.Instance.allPieceMovement[position];
            #region Assign all variables other than the 2D array
            thisPiece = inheritingPiece.thisPiece;
            playerTeamMaterial = inheritingPiece.playerTeamMaterial;
            enemyTeamMaterial = inheritingPiece.enemyTeamMaterial;
            name = inheritingPiece.name/* + ", " + UnityEngine.Random.Range(0, 1000)*/;
            movableTiles1DArray = inheritingPiece.movableTiles1DArray;
            moveableTiles = inheritingPiece.moveableTiles;
            potentialRange = inheritingPiece.potentialRange;
            infinitelyScalingRange = inheritingPiece.infinitelyScalingRange;
            currentRange = inheritingPiece.currentRange;
            moveableTiles = new bool[(int)Mathf.Sqrt(movableTiles1DArray.Length)][];
            #endregion
            #region Generate the 2D array
            for (int i = 0; i < moveableTiles.Length; i++) {
                moveableTiles[i] = new bool[moveableTiles.Length];
            }
            for (int y = 0; y < moveableTiles.Length; y++) {
                for (int x = 0; x < moveableTiles.Length; x++) {
                    moveableTiles[y][x] = movableTiles1DArray[y * moveableTiles.Length + x];
                }
            }
            #endregion
            #region Make the infinitely scaling pieces start at a small range
            if (infinitelyScalingRange) {
                for (int i = 0; i < 7; i++) {
                    ExpandSize();
                }
            }
            #endregion

            if (playersTeam) {
                AI.ai.PlayersTeam.Add(this);
            }
            else {
                AI.ai.AITeam.Add(this);
            }
        }

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
            catch (NullReferenceException) { }
            potentialRange += 1;
            moveableTiles = new bool[tempArray.Length][];
            for (int i = 0; i < moveableTiles.Length; i++) {
                moveableTiles[i] = new bool[moveableTiles.Length];
                moveableTiles[i] = tempArray[i];
            }
            moveableTiles = tempArray;
        }
    }
}