using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Data;
using System.Threading.Tasks;

public class AI : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AI ai;
    public List<PieceMovement> PlayersTeam;
    public List<PieceMovement> AITeam;
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
        List<PieceMovement> usedMovement = AITurn ? gamestate.AITeam : gamestate.playersTeam;
        if (depth - startingDepth == 0) {
            //one of the three AI turn moves
            Parallel.ForEach(usedMovement, (movement, parallelLoopsState) => {
                if (movement.hasMoved) {
                    return;
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
                                beta = Math.Min(beta, bestEvaluation);
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
                    parallelLoopsState.Stop();
                }
            });
        }
        else if (depth > 0){ 
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
                                beta = Math.Min(beta, bestEvaluation);
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
                if (piece.moveableTiles[i][j] && i + piece.AIAccessiblePosition.x <= 200 && j + piece.AIAccessiblePosition.y <= 200) {
                    Vector2Int move = new(i - piece.currentRange - 1, j - piece.currentRange - 1);
                    try {
                        if (Gamestate.DoesPositionExist(move + piece.AIAccessiblePosition)) {
                            validMovePositions.Add(move);
                        }
                    }
                    catch {
                        UnityEngine.Debug.Log(move + ", " + piece.AIAccessiblePosition + ", " + (i + piece.AIAccessiblePosition.x <= 100) + ", " + (j + piece.AIAccessiblePosition.y <= 100));
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
        string pieceName;
        if (movement == null) {
            return 0;
        }
        pieceName = movement.inheritingPiece.name;
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

    private void Update() {
        if (Gamestate.board == null) {
            new Gamestate(PlayersTeam, AITeam);
        }
        if (firstFrame) {
            CheckAllPlayersAndAITeamsArePresentAndCorrect();
            firstFrame = false;
        }
    }

    void NeedsAName(UnderlyingPiece piece, Vector2Int destination) {
        if (piece.playersTeam == false) {
            if (piece.PieceInDirection(destination) != null) {
                if (piece.PieceInDirection(destination).GetComponent<UnderlyingPiece>().playersTeam) {
                    PlayersTeam.Remove(piece.PieceInDirection(destination).GetComponent<UnderlyingPiece>().thisPiece);
                    Destroy(piece.PieceInDirection(destination));
                }
                else {
                    throw new Exception(piece.thisPiece.name + "tried to capture it's own piece");
                }
            }
            piece.previousPosition = new Vector3(piece.transform.position.x + destination.x, UnderlyingPiece.pieceHeight, piece.transform.position.z + destination.y);
        }
        else {
            throw new Exception("tried to move one of the Player's pieces: " + piece.thisPiece.name);
        }
    }

    Dictionary<PieceMovement, List<Vector2Int>> validMoveCache;
    int numOfTimesValidMoveCacheRefreshed;

    public void BeginTurn() {
        validMoveCache = new();
        numOfTimesValidMoveCacheRefreshed = 0;
        Gamestate gamestate = new(PlayersTeam, AITeam);
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
        bestSecondMoveEval = int.MinValue;
        int searchDepth = 3;
        int numberOfMoves = 3;
        //for (int i = numberOfMoves; i >= 1; i--) {
        var evaluatedPieceAndMovement = Search(searchDepth + numberOfMoves, Mathf.NegativeInfinity, Mathf.Infinity, searchDepth + numberOfMoves - 1, gamestate);

        NeedsAName(evaluatedPieceAndMovement.Item3, evaluatedPieceAndMovement.Item2);
        NeedsAName(secondMove.Item1, secondMove.Item2);
        NeedsAName(thirdMove.Item1, thirdMove.Item2);

        foreach (PieceMovement piece in AITeam.Where(piece => piece.hasMoved)) {
            piece.thisObject.thisPiece.hasMoved = false;
        }
        Player.player.GetComponent<Player>().numberOfMoves = 3;
    }
}
