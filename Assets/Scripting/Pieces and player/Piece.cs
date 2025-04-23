using UnityEngine;
using UnityEditor;
using System;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class Piece : ScriptableObject {

    public string pieceName;
    #region handled by custom editor
    [HideInInspector]
    public bool[][] moveableTiles;
    [HideInInspector]
    public int sizeOfBoard = 1;
    #endregion

    public bool PositionIsUnlocked(int x, int z) {
        try {
            if (moveableTiles[(x * -1) + sizeOfBoard][(z * -1) + sizeOfBoard]) {
                return true;
            }
            else {  return false; }
        }catch(IndexOutOfRangeException e) { return false; }
    }
}

[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor{
    int difference;
    Piece myTarget;
    int previousBoardSize;


    void MoveAllInArrayDownRight(int difference) {
        //move all of the values in the board list down and right
        for (int i = previousBoardSize; i >= 0; i--) {
            for (int j = previousBoardSize; j >= 0; j--) {
                myTarget.moveableTiles[i + difference][j + difference] = myTarget.moveableTiles[i][j];
            }
        }
        for (int i = 0; i < difference; i++) {
            for (int j = 0; j < myTarget.sizeOfBoard; j++) {
                myTarget.moveableTiles[i][j] = false;
                myTarget.moveableTiles[j][i] = false;
            }
        }
    }

    void MoveAllInArrayUpLeft(int difference) {
        //move all of the values in the board list up and left
        Debug.Log(previousBoardSize);
        for (int i = difference; i <= previousBoardSize - 1; i++) {
            for (int j = difference; j <= previousBoardSize - 1; j++) {
                myTarget.moveableTiles[i - difference][j - difference] = myTarget.moveableTiles[i][j];
            }
        }
    }

    public override void OnInspectorGUI(){
        myTarget = (Piece)target;
        base.OnInspectorGUI();
        GUIContent guiContent = new GUIContent("Please input the radius of the moveable tiles for this character");
        previousBoardSize = myTarget.sizeOfBoard;
        myTarget.sizeOfBoard = EditorGUILayout.IntSlider(guiContent, (myTarget.sizeOfBoard-1)/2, 1, 9) * 2 + 1;
        if (previousBoardSize > myTarget.sizeOfBoard) {
            MoveAllInArrayUpLeft(-(myTarget.sizeOfBoard - previousBoardSize)/2);
        }
        Array.Resize<bool[]>(ref myTarget.moveableTiles, myTarget.sizeOfBoard);
        for (int i = 0; i < myTarget.sizeOfBoard; i++) {
            Array.Resize<bool>(ref myTarget.moveableTiles[i], myTarget.sizeOfBoard);
        }
        if (previousBoardSize < myTarget.sizeOfBoard) {
            MoveAllInArrayDownRight((myTarget.sizeOfBoard - previousBoardSize)/2);
        }
        EditorGUILayout.PrefixLabel("Please input the square that can be moved to by this piece");
        EditorGUILayout.BeginVertical();
        GUIContent gridName;
        for (int i = 0; i < myTarget.sizeOfBoard; i++) {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < myTarget.sizeOfBoard; j++) {
                if (i == j && j == (myTarget.sizeOfBoard - 1) / 2) {
                    gridName = new GUIContent("X", "This is where the piece is relative to all the other positions");
                    EditorGUILayout.LabelField(gridName, GUILayout.Width(18), GUILayout.ExpandWidth(false));
                    continue;
                }
                gridName = new GUIContent("");
                myTarget.moveableTiles[i][j] = EditorGUILayout.ToggleLeft(gridName, myTarget.moveableTiles[i][j], GUILayout.Width(18), GUILayout.ExpandWidth(false));
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        if (myTarget.pieceName == null) {
            myTarget.pieceName = myTarget.name;
        }
    }
}
