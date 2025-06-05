using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TC.PieceMovement))]
public class PieceMovementAttributes : PropertyDrawer {
    int previousBoardSize;

    private const float FOLDOUT_HEIGHT = 18f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        /*if (property.FindPropertyRelative("name") == null) {
            property.FindPropertyRelative("name") = property.FindPropertyRelative("name");
        }*/
        float height = FOLDOUT_HEIGHT;
        if (property.isExpanded) {
            //name
            height += FOLDOUT_HEIGHT;
            //gameobject
            height += FOLDOUT_HEIGHT;
            //Allied Material
            height += FOLDOUT_HEIGHT;
            if (property.FindPropertyRelative("playerTeamMaterial").isExpanded) {
                for (int i = 0; i <= property.FindPropertyRelative("playerTeamMaterial").arraySize; i++) {
                    height += 30;
                }
            }
            //Enemy material
            height += FOLDOUT_HEIGHT;
            if (property.FindPropertyRelative("enemyTeamMaterial").isExpanded) {
                for (int i = 0; i <= property.FindPropertyRelative("enemyTeamMaterial").arraySize; i++) {
                    height += 30;
                }
            }
            //infiniteRange
            height += FOLDOUT_HEIGHT;
            //range
            height += FOLDOUT_HEIGHT;
            //1D array
            for (int i = 0; i < Math.Floor(Math.Sqrt(property.FindPropertyRelative("movableTiles1DArray").arraySize)); i++) {
                height += FOLDOUT_HEIGHT;
            }
        }
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        label.text = property.FindPropertyRelative("name").stringValue;
        Rect foldoutRect = new(position.x, position.y, position.width, FOLDOUT_HEIGHT);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

        if (property.isExpanded) {

            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("name"));
            position.y += FOLDOUT_HEIGHT;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("name"));
            position.y += FOLDOUT_HEIGHT;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("thisPiece"));
            position.y += FOLDOUT_HEIGHT;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("playerTeamMaterial"));
            position.y += FOLDOUT_HEIGHT;
            if (property.FindPropertyRelative("playerTeamMaterial").isExpanded) {
                position.y += 38;
                for (int i = 0; i < property.FindPropertyRelative("playerTeamMaterial").arraySize; i++) {
                    position.y += 20;
                }
            }
            EditorGUI.PropertyField(position, property.FindPropertyRelative("enemyTeamMaterial"));
            position.y += FOLDOUT_HEIGHT;
            if (property.FindPropertyRelative("enemyTeamMaterial").isExpanded) {
                position.y += 38;
                for (int i = 0; i < property.FindPropertyRelative("enemyTeamMaterial").arraySize; i++) {
                    position.y += 20;
                }
            }
            EditorGUI.PropertyField(position, property.FindPropertyRelative("infinitelyScalingRange"));
            position.y += FOLDOUT_HEIGHT;
            previousBoardSize = property.FindPropertyRelative("potentialRange").intValue;
            EditorGUI.IntSlider(position, property.FindPropertyRelative("potentialRange"), 1, 9);

            if (previousBoardSize < property.FindPropertyRelative("potentialRange").intValue) {
                property.FindPropertyRelative("movableTiles1DArray").FindPropertyRelative("Array.size").intValue = (property.FindPropertyRelative("potentialRange").intValue * 2 + 1) * (property.FindPropertyRelative("potentialRange").intValue * 2 + 1);
                MoveAllInArrayDownRight(property.FindPropertyRelative("potentialRange").intValue - previousBoardSize, property);
            }
            else if (previousBoardSize > property.FindPropertyRelative("potentialRange").intValue) {
                MoveAllInArrayUpLeft(previousBoardSize - property.FindPropertyRelative("potentialRange").intValue, property);
                property.FindPropertyRelative("movableTiles1DArray").FindPropertyRelative("Array.size").intValue = (property.FindPropertyRelative("potentialRange").intValue * 2 + 1) * (property.FindPropertyRelative("potentialRange").intValue * 2 + 1);
            }
            //hook the difference things up; oh yeah, they will need to be completely rewritten as they are for the old 2D array

            float addY = 0;
            Rect rect;
            position.y += FOLDOUT_HEIGHT;
            EditorGUI.PrefixLabel(position, EditorGUIUtility.GetControlID(FocusType.Passive), new GUIContent("Movement array"));
            for (int i = 0; i < Math.Floor(Math.Sqrt(property.FindPropertyRelative("movableTiles1DArray").arraySize)); i++) {
                float addX = 265;
                for (int j = 0; j < Math.Floor(Math.Sqrt(property.FindPropertyRelative("movableTiles1DArray").arraySize)); j++) {
                    rect = new(position.x + addX, position.y + addY, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i)), EditorGUI.GetPropertyHeight(property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i)));
                    addX += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i));
                    if (i == j && j == property.FindPropertyRelative("potentialRange").intValue) {
                        property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i * (int)Math.Floor(Math.Sqrt(property.FindPropertyRelative("movableTiles1DArray").arraySize)) + j).boolValue = false;
                        rect.x += 2;
                        EditorGUI.PrefixLabel(rect, new GUIContent("X"));
                        rect.x -= 2;
                        continue;
                    }
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i * (int)Math.Floor(Math.Sqrt(property.FindPropertyRelative("movableTiles1DArray").arraySize)) + j), GUIContent.none, false);
                }
                addY += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i));
            }
        }
        EditorGUI.EndProperty();
    }

    void MoveAllInArrayDownRight(int difference, SerializedProperty property) {
        //move all of the values in the board list down and right
        bool[] tempBoolArray = new bool[(property.FindPropertyRelative("potentialRange").intValue * 2 + 1) * (property.FindPropertyRelative("potentialRange").intValue * 2 + 1)];
        for (int y = previousBoardSize * 2; y >= 0; y--) {
            for (int x = previousBoardSize * 2; x >= 0; x--) {
                tempBoolArray[(y + difference) * (2 * property.FindPropertyRelative("potentialRange").intValue + 1) + (x + difference)] = property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(y * (2 * previousBoardSize + 1) + x).boolValue;
            }
        }
        for (int i = 0; i < tempBoolArray.Length; i++) {
            property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i).boolValue = tempBoolArray[i];
        }
        /*
        for (int i = 0; i < difference; i++) {
            for (int j = 0; j < myTarget.potentialRange; j++) {
                myTarget.moveableTiles[i][j] = false;
                myTarget.moveableTiles[j][i] = false;
            }
        }//*/
    }

    void MoveAllInArrayUpLeft(int difference, SerializedProperty property) {
        //move all of the values in the board list up and left
        bool[] tempBoolArray = new bool[(property.FindPropertyRelative("potentialRange").intValue * 2 + 1) * (property.FindPropertyRelative("potentialRange").intValue * 2 + 1)];
        for (int y = 0; y <= property.FindPropertyRelative("potentialRange").intValue * 2; y++) {
            for (int x = 0; x <= property.FindPropertyRelative("potentialRange").intValue * 2; x++) {
                tempBoolArray[x + y * (2 * property.FindPropertyRelative("potentialRange").intValue + 1)] = property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex((x + difference) + (y + difference) * (previousBoardSize * 2 + 1)).boolValue;
                //myTarget.moveableTiles[i - difference][j - difference] = myTarget.moveableTiles[i][j];
            }
        }
        for (int i = 0; i < tempBoolArray.Length; i++) {
            property.FindPropertyRelative("movableTiles1DArray").GetArrayElementAtIndex(i).boolValue = tempBoolArray[i];
        }
    }



    /*public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        base.CreatePropertyGUI(property);
        GUIContent guiContent = new GUIContent("Please input the radius of the moveable tiles for this character");
        previousBoardSize = myTarget.potentialRange;
        myTarget.potentialRange = EditorGUILayout.IntSlider(guiContent, (myTarget.potentialRange - 1) / 2, 1, 9) * 2 + 1;
        if (previousBoardSize > myTarget.potentialRange) {
            MoveAllInArrayUpLeft(-(myTarget.potentialRange - previousBoardSize) / 2);
        }
        Array.Resize<bool[]>(ref myTarget.moveableTiles, myTarget.potentialRange);
        for (int i = 0; i < myTarget.potentialRange; i++) {
            Array.Resize<bool>(ref myTarget.moveableTiles[i], myTarget.potentialRange);
        }
        if (previousBoardSize < myTarget.potentialRange) {
            MoveAllInArrayDownRight((myTarget.potentialRange - previousBoardSize) / 2);
        }
        EditorGUILayout.PrefixLabel("Please input the square that can be moved to by this piece");
        EditorGUILayout.BeginVertical();
        GUIContent gridName;
        for (int i = 0; i < myTarget.potentialRange; i++) {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < myTarget.potentialRange; j++) {
                if (i == j && j == (myTarget.potentialRange - 1) / 2) {
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
        if (myTarget.name == null) {
            myTarget.name = myTarget.name;
        }
    }*/
}
