using UnityEngine;

namespace TC{
    public class PieceBase : UnderlyingPiece {

        void Start() {
            FindStartPosition();
            ActualStart();
            AssignMaterialAndMesh();
            FacePlayer();
        }

        void FacePlayer() {
            Quaternion a = new();
            a.SetLookRotation(transform.position - Player.player.transform.position + new Vector3(0, -1, 0));
            a = new Quaternion(0, a.y, 0, a.w);
            transform.rotation = a;
        }

        void AssignMaterialAndMesh() {
            gameObject.GetComponent<MeshFilter>().mesh = thisPiece.thisPiece;
            if (playersTeam) {
                gameObject.GetComponent<MeshRenderer>().materials = thisPiece.playerTeamMaterial;
            }
            else {
                gameObject.GetComponent<MeshRenderer>().materials = thisPiece.enemyTeamMaterial;
            }
        }

        void FindStartPosition() {
            transform.position = new Vector3(101, 0, 101);
            int newX = 101;
            int newZ = 101;
            int count = 0;
            System.Random random;
            bool whileLoopVariable = true;
            while (whileLoopVariable && count < 10) {
                random = new System.Random();
                newX = random.Next(-100, 100);
                newZ = random.Next(-100, 100);
                if (Physics.Raycast(new Vector3(newX, 3.2f, newZ), new Vector3(0, -1, 0), 1) == true) {
                    newX = 101;
                }
                count++;
                CubeBase.GetSquareInDirection(newX, newZ);
                try {
                    CubeBase.GetSquareInDirection(newX, newZ).GetComponent<MeshRenderer>().name = CubeBase.GetSquareInDirection(newX, newZ).GetComponent<MeshRenderer>().name;
                    whileLoopVariable = false;
                }
                catch {
                    whileLoopVariable = true;
                }
            }
            base.previousPosition = new Vector3(newX, 2, newZ);
        }

        void LateUpdate() {
            if (mode == Mode.levelling) {
                base.IfNotLevellingReturn();
                selected = true;
            }
            if (CubeBase.GetSquareInDirection(transform.position, 0, 0) == null && MenuHandling.boardState == null) {
                FindStartPosition();
            }
            Selected();
            EnsureCorrectPositions();
            FacePlayer();
        }
    }
}