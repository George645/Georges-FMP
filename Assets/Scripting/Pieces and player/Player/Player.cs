using UnityEngine;

public class Player : UnderlyingPiece{
    public static GameObject player;
    
    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }
    
    private void Start() {
        ActualStart();
        playersTeam = true;
    }

    void Update(){
        base.Selected();
    }
}
