using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;
using System;

public class AI : MonoBehaviour {

    #region Constants
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
            evaluation -= GetAmountOfMaterial(piece);
        }
        foreach (PieceMovement piece in AITeam) {
            evaluation += GetAmountOfMaterial(piece);
        }
        return evaluation;
    }

    //Write down what sort of systems you want, what you want them to do you for and so on

    int GetAmountOfMaterial(PieceMovement movement) {
        return movement.name switch {
            "Rook" => 500,
            "Bishop" => 300,
            "Knight" => 300,
            "Pawn" => 100,
            "Queen" => 900,
            "Peanut" => throw new NotImplementedException(),
            "Wisp" => throw new NotImplementedException(),
            "Player" => 200,
            "Elephant" => throw new NotImplementedException(),
            "Snail" => throw new NotImplementedException(),
            "Lightning Bolt" => throw new NotImplementedException(),
            "Pedestal" => throw new NotImplementedException(),
            _ => throw new KeyNotFoundException("name not found: " + movement.name),
        };
    }

    public void BeginTurn() {



        foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
    }
}
