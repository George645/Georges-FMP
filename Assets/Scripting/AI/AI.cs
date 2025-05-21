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

    int Evaluation(Gamestate gamestate) {
        int evaluation = 0;
        foreach (PieceMovement piece in gamestate.playersTeam) {
            //WORK ON THIS
            evaluation -= GetAmountOfMaterial(piece);
        }
        foreach (PieceMovement piece in gamestate.AITeam) {
            evaluation += GetAmountOfMaterial(piece);
        }
        return evaluation;
    }

    int bestSecondMoveEval;
    (int, Vector2Int, UnderlyingPiece) Search(int depth, float alpha, float beta, int startingDepth, Gamestate gamestate) {
        //create a new class where the gamestate can be saved - things like a 200 by 200 board state and a piece list for each team
        Vector2Int bestPosition = Vector2Int.zero;
        depth -= 1;
        bool AITurn = (startingDepth - depth) / 3 % 2 == 0;
        int bestEvaluation = AITurn ? int.MinValue : int.MaxValue;
        UnderlyingPiece bestPiece = null;
        if (numOfTimesValidMoveCacheRefreshed != (int)(startingDepth - depth) / 6) {
            numOfTimesValidMoveCacheRefreshed++;
            foreach (PieceMovement pieceMovement in AITeam) {
                validMoveCache[pieceMovement] = ValidMoves(pieceMovement);
            }
            foreach (PieceMovement pieceMovement in PlayersTeam) {
                validMoveCache[pieceMovement] = ValidMoves(pieceMovement);
            }
        }
        if (depth > 0) {
            //one of the three AI turn moves
            List<PieceMovement> usedMovement = AITurn ? gamestate.AITeam : gamestate.playersTeam;
            foreach (PieceMovement movement in usedMovement) {
                if (movement.hasMoved) {
                    continue;
                }
                foreach (Vector2Int move in validMoveCache[movement].OrderByDescending(move => gamestate.PieceInPosition(movement.AIAccessiblePosition + move) != null ? GetAmountOfMaterial(gamestate.PieceInPosition(movement.AIAccessiblePosition + move)) : 0)) {
                    if (movement.infinitelyScalingRange) {
                        //make a new thing in here to check each time it is scaled up
                        for (int l = 0; l <= movement.currentRange; l++) {
                            Vector2Int scalingMove = move * l;
                            if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + scalingMove)) {
                                break;
                            }
                            PieceMovement capturingPiece = ApplyMove(movement, scalingMove, !AITurn, gamestate);
                            if (capturingPiece == movement) {
                                UndoMove(movement);
                                break;
                            }
                            movement.hasMoved = true;
                            var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, gamestate) : Search(depth, alpha, beta, startingDepth, gamestate);
                            UndoMove(movement);
                            movement.hasMoved = false;
                            if (AITurn) {
                                if (aSearch.Item1 > bestEvaluation) {
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = scalingMove;
                                }
                                alpha = Math.Max(alpha, bestEvaluation);
                            }
                            else {
                                if (aSearch.Item1 < bestEvaluation) {
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
                        if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + move)) {
                            continue;
                        }
                        PieceMovement capturingPiece = ApplyMove(movement, move, !AITurn, gamestate);
                        if (capturingPiece == movement) {
                            UndoMove(movement);
                            continue;
                        }
                        movement.hasMoved = true;
                        var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, gamestate) : Search(depth, alpha, beta, startingDepth, gamestate);
                        UndoMove(movement);
                        movement.hasMoved = false;
                        if (AITurn) {
                            if (aSearch.Item1 > bestEvaluation) {
                                bestEvaluation = aSearch.Item1;
                                bestPiece = movement.thisObject;
                                bestPosition = move;
                                if (depth == startingDepth) {
                                    secondMove = (aSearch.Item3, aSearch.Item2);
                                }
                                if (depth == startingDepth - 1 && bestSecondMoveEval < bestEvaluation) {
                                    bestSecondMoveEval = bestEvaluation;
                                    thirdMove = (bestPiece, aSearch.Item2);
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
                            //Debug.Log("alpha beta pruning success");
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
            bestEvaluation = Evaluation(gamestate);
        }
        return (bestEvaluation, bestPosition, bestPiece);
    }

    List<Vector2Int> ValidMoves(PieceMovement piece) {
        List<Vector2Int> validMovePositions = new();

        for (int i = 0; i < piece.moveableTiles.Length; i++) {
            for (int j = 0; j < piece.moveableTiles.Length; j++) {
                if (piece.moveableTiles[i][j]) {
                    Vector2Int move = new Vector2Int(i - piece.currentRange - 1, j - piece.currentRange - 1);
                    if (Gamestate.DoesPositionExist(move + piece.AIAccessiblePosition)){
                        validMovePositions.Add(move);
                    }
                }
            }
        }
        return validMovePositions;
    }

    PieceMovement ApplyMove(PieceMovement piece, Vector2Int relativePosition, bool playersTurn, Gamestate gamestate) {
        PieceMovement returningPiece = null;
        try {
            returningPiece = gamestate.PieceInPosition(piece.AIAccessiblePosition + relativePosition);
            if (returningPiece != null && returningPiece.thisObject.playersTeam == playersTurn) {
                returningPiece = piece;
            }
            piece.AIAccessiblePosition = new Vector2Int(piece.AIAccessiblePosition.x + relativePosition.x, piece.AIAccessiblePosition.y + relativePosition.y);
        }
        catch { }
        return returningPiece;
    }

    void UndoMove(PieceMovement piece) {
        piece.AIAccessiblePosition = new Vector2Int((int)piece.thisObject.previousPosition.x, (int)piece.thisObject.previousPosition.z);
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
        if (Gamestate.board == null) {
            new Gamestate(PlayersTeam, AITeam);
        }
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

    Dictionary<PieceMovement, List<Vector2Int>> validMoveCache;
    int numOfTimesValidMoveCacheRefreshed;

    public void BeginTurn() {
        validMoveCache = new();
        numOfTimesValidMoveCacheRefreshed = 0;
        foreach (PieceMovement pieceMovement in AITeam) {
            validMoveCache[pieceMovement] = ValidMoves(pieceMovement);
        }
        foreach (PieceMovement pieceMovement in PlayersTeam) {
            validMoveCache[pieceMovement] = ValidMoves(pieceMovement);
        }
        foreach (PieceMovement piece in AITeam.Where(piece => piece.thisObject.hasMoved)) {
            piece.thisObject.thisPiece.thisObject.hasMoved = false;
        }

        foreach (PieceMovement piece in AITeam.Where(piece => piece == null)) {
            AITeam.Remove(piece);
        }
        Gamestate gamestate = new(PlayersTeam, AITeam);
        bestSecondMoveEval = int.MinValue;
        int searchDepth = 1;
        int numberOfMoves = 3;
        //for (int i = numberOfMoves; i >= 1; i--) {
        var evaluatedPieceAndMovement = Search(searchDepth + numberOfMoves, Mathf.NegativeInfinity, Mathf.Infinity, searchDepth + numberOfMoves - 1, gamestate);
        Debug.Log(evaluatedPieceAndMovement);
        if (evaluatedPieceAndMovement.Item3.playersTeam == false) {
            if (evaluatedPieceAndMovement.Item3.PieceInDirection(evaluatedPieceAndMovement.Item2) != null) {
                if (evaluatedPieceAndMovement.Item3.PieceInDirection(evaluatedPieceAndMovement.Item2).GetComponent<UnderlyingPiece>().playersTeam) {
                    PlayersTeam.Remove(evaluatedPieceAndMovement.Item3.PieceInDirection(evaluatedPieceAndMovement.Item2).GetComponent<UnderlyingPiece>().thisPiece);
                    Destroy(evaluatedPieceAndMovement.Item3.PieceInDirection(evaluatedPieceAndMovement.Item2));
                }
                else {
                    throw new Exception(evaluatedPieceAndMovement.Item3.thisPiece.name + "tried to capture it's own piece");
                }
            }
            evaluatedPieceAndMovement.Item3.previousPosition = new Vector3(evaluatedPieceAndMovement.Item3.transform.position.x + evaluatedPieceAndMovement.Item2.x, UnderlyingPiece.pieceHeight, evaluatedPieceAndMovement.Item3.transform.position.z + evaluatedPieceAndMovement.Item2.y);
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + evaluatedPieceAndMovement.Item3.thisPiece.name);
        }
        Debug.Log(secondMove);
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
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + thirdMove.Item1.thisPiece.name);
        }
        Debug.Log(thirdMove);

        isEnemysTurn = false;
        Player.player.GetComponent<Player>().numberOfMoves = 3;

        foreach (PieceMovement piece in AITeam.Where(piece => piece.hasMoved)) {
            piece.thisObject.thisPiece.hasMoved = false;
        }
        Player.player.GetComponent<Player>().numberOfMoves = 3;
    }
}
