using UnityEngine;

public class Player : UnderlyingPiece {
    public static GameObject player;
    public int numberOfMoves;

    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }

    private void Start() {
        previousPosition = new Vector3(0, 2, 0);
        playersTeam = true;
        ActualStart();
    }

    void Update() {
        if (numberOfMoves == 0) { 
            AI.ai.BeginTurn();
        }
        if (mode == Mode.levelling) {
            IfNotLevellingReturn();
            selected = true;
        }
        Selected();
        EnsureCorrectPositions();
    }
}
