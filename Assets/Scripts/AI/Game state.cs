using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TC {
    public class Gamestate {
        public static bool[,] board;
        readonly PieceMovement[,] piecesOnBoard = new PieceMovement[OriginCube.MaxSizeOfBoard * 2, OriginCube.MaxSizeOfBoard * 2];
        public List<PieceMovement> playersTeam;
        public List<PieceMovement> AITeam;

        public Gamestate(Gamestate gamestate) {
            playersTeam = new List<PieceMovement>();
            AITeam = new List<PieceMovement>();
            Array.Copy(gamestate.piecesOnBoard, piecesOnBoard, (int)Math.Pow(OriginCube.MaxSizeOfBoard * 2, 2));
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
                for (int i = -OriginCube.MaxSizeOfBoard; i <= OriginCube.MaxSizeOfBoard; i++) {
                    for (int j = -OriginCube.MaxSizeOfBoard; j <= OriginCube.MaxSizeOfBoard; j++) {
                        if (OriginCube.GetSquareInDirection(i, j) != null) {
                            board[i + OriginCube.MaxSizeOfBoard, j + OriginCube.MaxSizeOfBoard] = true;
                        }
                        else {
                            board[i + OriginCube.MaxSizeOfBoard, j + OriginCube.MaxSizeOfBoard] = false;
                        }
                    }
                }
            }
            try {
                this.playersTeam = new List<PieceMovement>(playersTeam);
                this.AITeam = new List<PieceMovement>(AITeam);
                foreach (PieceMovement pieceMovement in AITeam) {
                    piecesOnBoard[pieceMovement.AIAccessiblePosition.x + OriginCube.MaxSizeOfBoard, pieceMovement.AIAccessiblePosition.y + OriginCube.MaxSizeOfBoard] = pieceMovement;
                }
                foreach (PieceMovement pieceMovement in playersTeam) {
                    piecesOnBoard[pieceMovement.AIAccessiblePosition.x + OriginCube.MaxSizeOfBoard, pieceMovement.AIAccessiblePosition.y + OriginCube.MaxSizeOfBoard] = pieceMovement;
                }
            }
            catch (ArgumentNullException) { }
        }
        public PieceMovement PieceInPosition(Vector2Int position) {
            PieceMovement objecta = null;
            if (position.x < OriginCube.MaxSizeOfBoard + 1 && position.y < OriginCube.MaxSizeOfBoard + 1 && position.x > -OriginCube.MaxSizeOfBoard + 1 && position.y > -OriginCube.MaxSizeOfBoard + 1) {
                if (piecesOnBoard[position.x + OriginCube.MaxSizeOfBoard, position.y + OriginCube.MaxSizeOfBoard] != null) {
                    objecta = piecesOnBoard[position.x + OriginCube.MaxSizeOfBoard, position.y + OriginCube.MaxSizeOfBoard];
                }
            }
            return objecta;
        }
        public static bool DoesPositionExist(Vector2Int position) {
            try {
                return board[position.x + OriginCube.MaxSizeOfBoard, position.y + OriginCube.MaxSizeOfBoard];
            }
            catch { return false; }
        }
    }
}
