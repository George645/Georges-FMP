using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Analytics;
using System.Threading;
using System.Data;

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

    (int, int, int, GameObject) Search(int depth, int startingDepth, List<PieceMovement> localPlayersTeam, List<PieceMovement> localAITeam) {
        Vector2Int position = Vector2Int.zero;
        int evaluation = Evaluation(localPlayersTeam, localAITeam);
        GameObject returningPiece = null;
        depth -= 1;
        if (depth != 0) {
            if ((startingDepth - depth) / 3 % 2 == 0) {
                foreach (PieceMovement movement in localAITeam) {
                    returningPiece = movement.thisObject.gameObject;
                    bool breakAll = false;
                    for (int j = 0; j < movement.moveableTiles.Length; j++) {
                        if (breakAll) {
                            break;
                        }
                        bool[] array = movement.moveableTiles[j];
                        for (int k = 0; k < array.Length; k++) {
                            bool boolean = array[k];
                            if (boolean == false) {
                                continue;
                            }
                            if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().playersTeam) {
                                    continue;
                                }
                            }
                            if (movement.infinitelyScalingRange) {
                                bool continueVar = false;
                                for (int l = 0; l < k; l++) {
                                    if ((l == k && movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().playersTeam) || (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) == null)) {
                                        continue;
                                    }
                                    continueVar = true;
                                    continue;
                                }
                                if (continueVar) {
                                    continue;
                                }
                            }
                            List<PieceMovement> inputList = localPlayersTeam;
                            if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                inputList.Remove(movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().thisPiece);
                            }
                            var thisSearch = Search(depth, startingDepth, inputList, localAITeam);
                            if (thisSearch.Item1 > evaluation) {
                                evaluation = thisSearch.Item1;
                                position = new Vector2Int(j - movement.potentialRange - 1, k - movement.potentialRange - 1);
                            }
                        }
                    }
                }
            }
            else if ((startingDepth - depth) / 3 % 2 == 1) {
                foreach (PieceMovement movement in localPlayersTeam) {
                    returningPiece = movement.thisObject.gameObject;
                    bool breakAll = false;

                    for (int j = 0; j < movement.moveableTiles.Length; j++) {
                        if (breakAll) {
                            break;
                        }
                        bool[] array = movement.moveableTiles[j];
                        for (int k = 0; k < array.Length; k++) {
                            bool boolean = array[k];
                            if (boolean == false) {
                                continue;
                            }
                            if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().playersTeam == false) {
                                    continue;
                                }
                            }
                            if (movement.infinitelyScalingRange) {
                                bool continueVar = false;
                                for (int l = 0; l < k; l++) {
                                    if ((l == k && movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().playersTeam) || (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) == null)) {
                                        continue;
                                    }
                                    continueVar = true;
                                    continue;
                                }
                                if (continueVar) {
                                    continue;
                                }
                            }
                            List<PieceMovement> inputList = localAITeam;
                            if (movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1) != null) {
                                inputList.Remove(movement.thisObject.PieceInDirection(j - movement.currentRange - 1, k - movement.currentRange - 1).GetComponent<UnderlyingPiece>().thisPiece);
                            }
                            var thisSearch = Search(depth, startingDepth, localPlayersTeam, inputList);
                            if (thisSearch.Item1 > evaluation) {
                                evaluation = thisSearch.Item1;
                                position = new Vector2Int(j - movement.potentialRange - 1, k - movement.potentialRange - 1);
                            }
                        }
                    }
                }
            }
            else {
                throw new ArithmeticException("figure out a different formula that takes in two numbers, subtracts one from the other and then divides that by three and then figures out if it is player's or enemie's turn");
            }
        }
        else {
            evaluation = Evaluation(localPlayersTeam, localAITeam); 
        }
            return (evaluation, position.x, position.y, returningPiece);
    }

    void FigureOutANameAndAReturnType() {

    }

    int GetAmountOfMaterial(PieceMovement movement) {
        string pieceName = movement.name[..movement.name.IndexOf(',')];
        int returningInt = UnityEngine.Random.Range(0, 100) - 50;
        return pieceName switch {
            "Rook" => 500 + returningInt,
            "Bishop" => 300 + returningInt,
            "Knight" => 300 + returningInt,
            "Pawn" => 100 + returningInt,
            "Queen" => 900 + returningInt,
            "Peanut" => 100 + returningInt,
            "Wisp" => 100 + returningInt,
            "Player" => 200 + returningInt,
            "Elephant" => 100 + returningInt,
            "Snail" => 100 + returningInt,
            "Lightning bolt" => 100 + returningInt,
            "Pedestal" => 100 + returningInt,
            _ => throw new KeyNotFoundException("name not found: " + pieceName),
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

        foreach (PieceMovement thing in AITeam.Where(thing => thing == null)) {
            AITeam.Remove(thing);
        }

        List<PieceMovement> tempList = AITeam;
        int numberOfMoves = 3;
        for (int i = numberOfMoves; i > 1; i--) {
            var evaluatedPieceAndMovement = Search(3 + i, 6, PlayersTeam, AITeam);
            if (evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().playersTeam == false) {
                if (evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3) != null) {
                    if (evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3).GetComponent<UnderlyingPiece>().playersTeam) {
                        PlayersTeam.Remove(evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3).GetComponent<UnderlyingPiece>().thisPiece);
                        Destroy(evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3));
                    }
                    else {
                        throw new Exception("tried to capture it's own piece, that will still be an error");
                    }
                }
                evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(evaluatedPieceAndMovement.Item4.transform.position.x + evaluatedPieceAndMovement.Item2, UnderlyingPiece.pieceHeight, evaluatedPieceAndMovement.Item4.transform.position.z + evaluatedPieceAndMovement.Item3);
                tempList.Remove(evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().thisPiece);
            }
            else {
                throw new Exception("tried to move one of the Player's pieces");
            }
            isEnemysTurn = false;
            Player.player.GetComponent<Player>().numberOfMoves = 1;
        }


        foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
    }
}
