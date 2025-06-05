using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TC {
    public class Gamestate {
        public static bool[,] board;
        readonly PieceMovement[,] piecesOnBoard = new PieceMovement[200, 200];
        public List<PieceMovement> playersTeam;
        public List<PieceMovement> AITeam;

        public Gamestate(Gamestate gamestate) {
            playersTeam = new List<PieceMovement>();
            AITeam = new List<PieceMovement>();
            Array.Copy(gamestate.piecesOnBoard, piecesOnBoard, (int)Math.Pow(200, 2));
            foreach (PieceMovement thing in gamestate.playersTeam) {
                    this.playersTeam.Add(new PieceMovement(thing));
            }
            foreach (PieceMovement thing in gamestate.AITeam) {
                this.AITeam.Add(new PieceMovement(thing));
            }
            AITeam = gamestate.AITeam.Select(thing => thing).ToList();
        }

        public Gamestate(List<PieceMovement> playersTeam, List<PieceMovement> AITeam) {
            if (board == null) {
                board = new bool[201, 201];
                for (int i = -100; i <= 100; i++) {
                    for (int j = -100; j <= 100; j++) {
                        if (OriginCube.GetSquareInDirection(i, j) != null) {
                            board[i + 100, j + 100] = true;
                        }
                        else {
                            board[i + 100, j + 100] = false;
                        }
                    }
                }
            }
            try {
                this.playersTeam = new List<PieceMovement>(playersTeam);
                this.AITeam = new List<PieceMovement>(AITeam);
                foreach (PieceMovement pieceMovement in AITeam) {
                    piecesOnBoard[pieceMovement.AIAccessiblePosition.x + 100, pieceMovement.AIAccessiblePosition.y + 100] = pieceMovement;
                }
                foreach (PieceMovement pieceMovement in playersTeam) {
                    piecesOnBoard[pieceMovement.AIAccessiblePosition.x + 100, pieceMovement.AIAccessiblePosition.y + 100] = pieceMovement;
                }
            }
            catch (ArgumentNullException) { }
        }
        public PieceMovement PieceInPosition(Vector2Int position) {
            PieceMovement objecta = null;
            if (position.x < 101 && position.y < 101 && position.x > -101 && position.y > -101) {
                if (piecesOnBoard[position.x + 100, position.y + 100] != null) {
                    objecta = piecesOnBoard[position.x + 100, position.y + 100];
                }
            }
            return objecta;
        }
        public static bool DoesPositionExist(Vector2Int position) {
            try {
                return board[position.x + 100, position.y + 100];
            }
            catch { return false; }
        }
    }
}
