using UnityEngine;

public class Player : UnderlyingPiece{
    public static GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Selected();
    }

    #region Selected

    void Selected() {
        if (selected) {
            if (firstFrameSelected) {
                for (int i = 0; i <= thisPiece.sizeOfBoard; i++) {
                    for (int j = 0; j <= thisPiece.sizeOfBoard; j++) {
                        if (thisPiece.moveableTiles[i][j]) {

                        }
                    }
                }
                firstFrameSelected = false;
            }
        }
        else {
            firstFrameSelected = true;
        }
    }

    #endregion
}
