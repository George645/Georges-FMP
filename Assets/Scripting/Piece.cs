using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditor.TerrainTools;

[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class Piece : ScriptableObject{
    public string cardName;
    public int hi;
    [HideInInspector]
    public List<List<bool>> moveableTiles;
}

[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor{

    int sizeOfBoard = 1;
    public override void OnInspectorGUI(){

        Piece myTarget = (Piece)target;

        base.OnInspectorGUI();

        GUIContent guiContent = new GUIContent("Please input the radius of the moveable tiles for this character");
        sizeOfBoard = EditorGUILayout.IntSlider(guiContent, sizeOfBoard, 1, 9);
        for (int i = 0; i < sizeOfBoard; i++) {
            EditorGUILayout.BeginHorizontal();
            for(int j = 0; i < sizeOfBoard; i++) {
                myTarget.moveableTiles[i][j] = EditorGUILayout.Toggle(myTarget.moveableTiles[i][j]);
            }
            EditorGUILayout.EndHorizontal();
        }
        
    }
}
