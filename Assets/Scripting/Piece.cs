using UnityEngine;
using UnityEditor;
using System;

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
    public override void OnInspectorGUI(){
        Piece myTarget = (Piece)target;
        base.OnInspectorGUI();
        GUIContent guiContent = new GUIContent("Please input the radius of the moveable tiles for this character");
        myTarget.sizeOfBoard = EditorGUILayout.IntSlider(guiContent, (myTarget.sizeOfBoard-1)/2, 1, 9) * 2 + 1;
        Array.Resize<bool[]>(ref myTarget.moveableTiles, myTarget.sizeOfBoard);
        for (int i = 0; i<myTarget.sizeOfBoard; i++) {
            Array.Resize<bool>(ref myTarget.moveableTiles[i], myTarget.sizeOfBoard);
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
