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
    public bool[,] moveableTiles;
}

[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor{

    int sizeOfBoard = 1;
    public override void OnInspectorGUI(){

        Piece myTarget = (Piece)target;

        base.OnInspectorGUI();

        GUIContent guiContent = new GUIContent("Please input the size of the moveable tiles for this character");
        EditorGUILayout.IntSlider(sizeOfBoard, 1, 9, guiContent, new GUILayout());
        
    }
}
