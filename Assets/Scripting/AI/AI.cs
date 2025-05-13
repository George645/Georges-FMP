using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;

public class AI : MonoBehaviour {

    #region Constants
    const int RookValue = 500;
    const int BishopValue = 320;
    const int KnightValue = 300;
    const int PawnValue = 100;
    const int QueenValue = 900;
    const int PeanutValue = 100;
    const int WispValue = 100;
    const int ElephantValue = 100;
    const int SnailValue = 100;
    const int PedestalValue = 100;
    const int LightningBoltValue = 100;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AI ai;
    public List<PieceMovement> PlayersTeam;
    public List<PieceMovement> AITeam;

    void Awake() {
        ai = this;
    }

    int Evaluation() {
        int evaluation = 0;
        foreach (PieceMovement piece in PlayersTeam) {
            //WORK ON THIS
            asfoyu
        }
        return evaluation;
    }

    public void BeginTurn() {
        


        foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
