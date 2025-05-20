using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Data;

public class AI : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AI ai;
    public List<PieceMovement> PlayersTeam;
    public List<PieceMovement> AITeam;
    bool isEnemysTurn = false;
    bool firstSplitSecondCheck = true;
    bool firstFrame = true;
    (UnderlyingPiece, Vector2Int) secondMove;
    (UnderlyingPiece, Vector2Int) thirdMove;

    void Awake() {
        ai = this;
    }

    void CheckAllPlayersAndAITeamsArePresentAndCorrect() {
        List<PieceMovement> tempList = new();
        foreach (PieceMovement movement in PlayersTeam.Where(piece => piece.thisObject.playersTeam == false)) {
            tempList.Add(movement);
        }
        foreach (PieceMovement movement in tempList) {
            PlayersTeam.Remove(movement);
            AITeam.Add(movement);
        }
        tempList.Clear();
        foreach (PieceMovement movement in AITeam.Where(piece => piece.thisObject.playersTeam)) {
            tempList.Add(movement);
        }
        foreach (PieceMovement movement in tempList) {
            AITeam.Remove(movement);
            PlayersTeam.Add(movement);
        }
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

    int bestSecondMoveEval;
    (int, int, int, UnderlyingPiece) Search(int depth, float alpha, float beta, int startingDepth, List<PieceMovement> localPlayersTeam, List<PieceMovement> localAITeam) {
        //create a new class where the gamestate can be saved - things like a 200 by 200 board state and a piece list for each team
        throw new Exception("Read the comment above this");
        Vector2Int bestPosition = Vector2Int.zero;
        depth -= 1;
        int bestEvaluation = ((startingDepth - depth) / 3 % 2 == 0) ? int.MinValue : int.MaxValue;
        UnderlyingPiece bestPiece = null;
        if (depth > 0) {
            //one of the three AI turn moves
            List<PieceMovement> usedMovement = (startingDepth - depth) / 3 % 2 == 0 ? localAITeam : localPlayersTeam;
            foreach (PieceMovement movement in usedMovement) {
                foreach (Vector2Int move in ValidMoves(movement)) {
                    if (movement.infinitelyScalingRange) {
                        //make a new thing in here to check each time it is scaled up
                        for (int l = 0; l <= movement.currentRange; l++) {
                            Vector2Int scalingMove = move * l;
                            PieceMovement capturingPiece = ApplyMove(movement, scalingMove, (startingDepth - depth) / 3 % 2 == 1);
                            if (capturingPiece == movement) {
                                UndoMove(movement);
                                break;
                            }
                            List<PieceMovement> inputList = new List<PieceMovement>(localPlayersTeam);
                            inputList.Remove(movement);
                            var aSearch = Search(depth, alpha, beta, startingDepth, new List<PieceMovement>(localPlayersTeam), inputList);
                            UndoMove(movement);
                            if ((startingDepth - depth) / 3 % 2 == 0) {
                                if (aSearch.Item1 < bestEvaluation) {
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = scalingMove;
                                }
                                alpha = Math.Max(alpha, bestEvaluation);
                            }
                            else {
                                if (aSearch.Item1 > bestEvaluation) {
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = scalingMove;
                                }
                                beta = Math.Min(alpha, bestEvaluation);
                            }
                            if (beta <= alpha) {
                                break;
                            }
                        }
                    }
                    else {
                        PieceMovement capturingPiece = ApplyMove(movement, move, (startingDepth - depth) / 3 % 2 == 1);
                        if (capturingPiece == movement) {
                            UndoMove(movement);
                            continue;
                        }
                        List<PieceMovement> inputList = new List<PieceMovement>(localPlayersTeam);
                        inputList.Remove(movement);
                        var aSearch = Search(depth, alpha, beta, startingDepth, new List<PieceMovement>(localPlayersTeam), inputList);
                        UndoMove(movement);
                        if ((startingDepth - depth) / 3 % 2 == 0) {
                            if (aSearch.Item1 > bestEvaluation) {
                                bestEvaluation = aSearch.Item1;
                                bestPiece = movement.thisObject;
                                bestPosition = move;
                                if (depth == startingDepth) {
                                    secondMove = (aSearch.Item4, new Vector2Int(aSearch.Item2, aSearch.Item3));
                                }
                                if (depth == startingDepth - 1 && bestSecondMoveEval < bestEvaluation) {
                                    bestSecondMoveEval = bestEvaluation;
                                    thirdMove = (bestPiece, new Vector2Int(aSearch.Item2, aSearch.Item3));
                                }
                            }
                            alpha = Math.Max(alpha, bestEvaluation);
                        }
                        else {
                            if (aSearch.Item1 < bestEvaluation) {
                                bestEvaluation = aSearch.Item1;
                                bestPiece = movement.thisObject;
                                bestPosition = move;
                            }
                            beta = Math.Min(beta, bestEvaluation);
                        }
                        if (beta <= alpha) {
                            break;
                        }
                    }
                }
                if (beta <= alpha) {
                    break;
                }
            }
        }
        else {
            bestEvaluation = Evaluation(localPlayersTeam, localAITeam);
        }
        return (bestEvaluation, bestPosition.x, bestPosition.y, bestPiece);
    }

    List<Vector2Int> ValidMoves(PieceMovement piece) {
        List<Vector2Int> validMovePositions = new List<Vector2Int>();

        for (int i = 0; i < piece.moveableTiles.Length; i++) {
            for (int j = 0; j < piece.moveableTiles.Length; j++) {
                if (piece.moveableTiles[i][j]) {
                    validMovePositions.Add(new Vector2Int(i - piece.currentRange - 1, j - piece.currentRange - 1));
                }
            }
        }

        return validMovePositions;
    }

    PieceMovement ApplyMove(PieceMovement piece, Vector2Int relativePosition, bool playersTurn) {
        PieceMovement returningPiece = null;
        try {
            returningPiece = piece.thisObject.PieceInDirection(relativePosition).GetComponent<UnderlyingPiece>().thisPiece;
            if (returningPiece != null && returningPiece.thisObject.playersTeam == playersTurn) {
                returningPiece = piece;
            }
            piece.thisObject.transform.position = new Vector3(piece.thisObject.transform.position.x + relativePosition.x, piece.thisObject.transform.position.y, piece.thisObject.transform.position.z + relativePosition.y);
        }
        catch { }
        return returningPiece;
    }

    void UndoMove(PieceMovement piece) {
        piece.thisObject.transform.position = piece.thisObject.previousPosition;
    }

    int GetAmountOfMaterial(PieceMovement movement) {
        string pieceName = movement.name[..movement.name.IndexOf(',')];
        int returningInt = 0;
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
        if (firstFrame) {
            CheckAllPlayersAndAITeamsArePresentAndCorrect();
            firstFrame = false;
        }
        if (isEnemysTurn) {
            if (firstSplitSecondCheck) {
                StartTime = Time.time;
                firstSplitSecondCheck = !firstSplitSecondCheck;
            }
            isEnemysTurn = Time.time - StartTime <= 1;
        }
    }

    public void BeginTurn() {
        foreach (PieceMovement piece in AITeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }

        foreach (PieceMovement piece in AITeam.Where(piece => piece == null)) {
            AITeam.Remove(piece);
        }
        bestSecondMoveEval = int.MinValue;
        List<PieceMovement> tempList = AITeam;
        int searchDepth = 1;
        int numberOfMoves = 3;
        //for (int i = numberOfMoves; i >= 1; i--) {
        Debug.Log(0);
        var evaluatedPieceAndMovement = Search(searchDepth + numberOfMoves, Mathf.NegativeInfinity, Mathf.Infinity, searchDepth + numberOfMoves - 1, new List<PieceMovement>(PlayersTeam), new List<PieceMovement>(AITeam));
        Debug.Log(1);
        if (evaluatedPieceAndMovement.Item4.playersTeam == false) {
            if (evaluatedPieceAndMovement.Item4.PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3) != null) {
                if (evaluatedPieceAndMovement.Item4.PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3).GetComponent<UnderlyingPiece>().playersTeam) {
                    PlayersTeam.Remove(evaluatedPieceAndMovement.Item4.PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3).GetComponent<UnderlyingPiece>().thisPiece);
                    Destroy(evaluatedPieceAndMovement.Item4.PieceInDirection(evaluatedPieceAndMovement.Item2, evaluatedPieceAndMovement.Item3));
                }
                else {
                    throw new Exception(evaluatedPieceAndMovement.Item4.thisPiece.name + "tried to capture it's own piece");
                }
            }
            evaluatedPieceAndMovement.Item4.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(evaluatedPieceAndMovement.Item4.transform.position.x + evaluatedPieceAndMovement.Item2, UnderlyingPiece.pieceHeight, evaluatedPieceAndMovement.Item4.transform.position.z + evaluatedPieceAndMovement.Item3);
            tempList.Remove(evaluatedPieceAndMovement.Item4.thisPiece);
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + evaluatedPieceAndMovement.Item4.thisPiece.name);
        }

        if (secondMove.Item1.playersTeam == false) {
            if (secondMove.Item1.PieceInDirection(secondMove.Item2.x, secondMove.Item2.y) != null) {
                if (secondMove.Item1.PieceInDirection(secondMove.Item2.x, secondMove.Item2.y).GetComponent<UnderlyingPiece>().playersTeam) {
                    PlayersTeam.Remove(secondMove.Item1.PieceInDirection(secondMove.Item2.x, secondMove.Item2.y).GetComponent<UnderlyingPiece>().thisPiece);
                    Destroy(secondMove.Item1.PieceInDirection(secondMove.Item2.x, secondMove.Item2.y));
                }
                else {
                    throw new Exception(secondMove.Item1.thisPiece.name + "tried to capture it's own piece");
                }
            }
            secondMove.Item1.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(secondMove.Item1.transform.position.x + secondMove.Item2.x, UnderlyingPiece.pieceHeight, secondMove.Item1.transform.position.z + secondMove.Item2.y);
            tempList.Remove(secondMove.Item1.thisPiece);
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + secondMove.Item1.thisPiece.name);
        }

        if (thirdMove.Item1.playersTeam == false) {
            if (thirdMove.Item1.PieceInDirection(thirdMove.Item2.x, thirdMove.Item2.y) != null) {
                if (thirdMove.Item1.PieceInDirection(thirdMove.Item2.x, thirdMove.Item2.y).GetComponent<UnderlyingPiece>().playersTeam) {
                    PlayersTeam.Remove(thirdMove.Item1.PieceInDirection(thirdMove.Item2.x, thirdMove.Item2.y).GetComponent<UnderlyingPiece>().thisPiece);
                    Destroy(thirdMove.Item1.PieceInDirection(thirdMove.Item2.x, thirdMove.Item2.y));
                }
                else {
                    throw new Exception(thirdMove.Item1.thisPiece.name + "tried to capture it's own piece");
                }
            }
            thirdMove.Item1.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(thirdMove.Item1.transform.position.x + thirdMove.Item2.x, UnderlyingPiece.pieceHeight, thirdMove.Item1.transform.position.z + thirdMove.Item2.y);
            tempList.Remove(thirdMove.Item1.thisPiece);
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + thirdMove.Item1.thisPiece.name);
        }

        isEnemysTurn = false;
        Player.player.GetComponent<Player>().numberOfMoves = 3;

        foreach (PieceMovement piece in AITeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
        foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }
        Player.player.GetComponent<Player>().numberOfMoves = 3;
        Debug.Log("Enemy's turn over");
    }
}
