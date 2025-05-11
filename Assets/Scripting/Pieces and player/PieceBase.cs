using UnityEngine;

public class PieceBase : UnderlyingPiece {

    void Start() {
        FindStartPosition();
        base.ActualStart();
        AssignMaterialAndMesh();
        FacePlayer();
    }

    void FacePlayer() {
        Quaternion a = new Quaternion();
        a.SetLookRotation(transform.position - Player.player.transform.position);
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

    void Update() {
        if (mode == Mode.levelling) {
            base.IfNotLevellingReturn();
            selected = true;
        }
        if (CubeBase.GetSquareInDirection(transform.position, 0, 0) == null) {
            FindStartPosition();
        }
        Selected();
        EnsureCorrectPositions();
        FacePlayer();
    }
}