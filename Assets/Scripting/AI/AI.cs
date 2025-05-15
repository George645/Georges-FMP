using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.UIElements;

public class AI : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AI ai;
    public List<PieceMovement> PlayersTeam;
    public List<PieceMovement> AITeam;
    bool isEnemysTurn = false;
    bool firstSplitSecondCheck = true;

    void Awake() {
        ai = this;
    }
    private void Start() {
        
    }

    int Evaluation(List<PieceMovement> localPlayersTeam, List<PieceMovement> localAITeam) {
        int evaluation = 0;
        foreach (PieceMovement piece in localPlayersTeam) {
            //WORK ON THIS
            evaluation -= GetAmountOfMaterial(piece);
        }
        foreach (PieceMovement piece in localAITeam) {
            evaluation += GetAmountOfMaterial(piece);
        }
        return evaluation;
    }

    (int, int, int) Search(int depth, int startingDepth, List<PieceMovement> localPlayersTeam, List<PieceMovement> localAITeam) {
        Vector2Int position = Vector2Int.zero;
        int evaluation = int.MinValue;
        depth -= 1;
        if (depth != 0) {
            if ((startingDepth - depth) / 3 % 2 == 0) {
                foreach (PieceMovement movement in localAITeam) {
                    for (int j = 0; j < movement.moveableTiles.Length; j++) {
                        bool[] array = movement.moveableTiles[j];
                        for (int k = 0; k < array.Length; k++) {
                            bool boolean = array[k];
                            if (boolean == false){
                                continue;
                            }
                            if (movement.infinitelyScalingRange) {
                                for (int l = 0; l < k; l++) {
                                    if ((l != k || movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().playersTeam) && movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                        continue;
                                    }
                                }
                            }
                                List<PieceMovement> inputList = localPlayersTeam;
                            if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                inputList.Remove(movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().thisPiece);
                            }
                            var thisSearch = Search(depth, startingDepth, inputList, localAITeam);
                            if (thisSearch.Item1 > evaluation) {
                                evaluation = thisSearch.Item1;
                            }
                        }

                    }
                }
            }
            else if ((startingDepth - depth) / 3 % 2 == 1) {

            }
        }
        return (evaluation, position.x, position.y);
    }

    void FigureOutANameAndAReturnType() {

    }

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

    float StartTime;
    private void Update() {
        if (isEnemysTurn) {
            if (firstSplitSecondCheck) {
                StartTime = Time.time;
                firstSplitSecondCheck = !firstSplitSecondCheck;
            }
            isEnemysTurn = Time.time - StartTime <= 1;
        }
    }

    public void BeginTurn() {



        foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
    }
}
