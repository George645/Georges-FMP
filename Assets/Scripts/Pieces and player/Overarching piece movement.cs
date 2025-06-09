using System.Collections.Generic;
using UnityEngine;

namespace TC{
    public class OverarchingPieceMovement : MonoBehaviour {
        public static OverarchingPieceMovement Instance;
        public List<PieceMovement> allPieceMovement;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}