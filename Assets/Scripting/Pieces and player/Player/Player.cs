using NUnit.Framework.Constraints;
using UnityEngine;

public class Player : UnderlyingPiece {
    public static GameObject player;

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
        if (mode == Mode.levelling) {
            base.IfNotLevellingReturn();
            selected = true;
        }
        base.Selected();
        EnsureCorrectPositions();
    }
}
