using System.Collections.Generic;
using UnityEngine;

public class Player : UnderlyingPiece{
    public static GameObject player;
    List<GameObject> movableTiles = new();
    
    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }
    
    private void Start() {
        ActualStart();
    }

    void Update(){
        Selected();
    }

    #region Selected

    void Selected() {
        if (selected) {
            if (firstFrameSelected) {
                int count = 0;
                for (int i = - (thisPiece.potentialRange - 1) / 2; i <= (thisPiece.potentialRange - 1) / 2; i++) {
                    for (int j = - (thisPiece.potentialRange - 1) / 2; j <= (thisPiece.potentialRange - 1)/2; j++) {
                        Debug.Log(thisPiece.PositionIsUnlocked(i, j));
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

    #endregion
}
