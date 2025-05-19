using UnityEngine;

public class Player : UnderlyingPiece {
    public static GameObject player;
    public int numberOfMoves = 1;

    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }

    private void Start() {
        base.previousPosition = new Vector3(0, 2, 0);
        ActualStart();
        playersTeam = true;
    }

    void Update() {
        if (numberOfMoves == 0) { 
            AI.ai.BeginTurn();
        }
        if (mode == Mode.levelling) {
            base.IfNotLevellingReturn();
            selected = true;
        }
        base.Selected();
        EnsureCorrectPositions();
    }
}
