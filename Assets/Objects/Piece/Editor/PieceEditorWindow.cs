using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PieceEditorWindow : EditorWindow
{
    int gridSize = 5;
    float buttonSpace = 15;

    bool[,] checkBoxes = new bool[5,5];


    [MenuItem("Tools/Piece Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PieceEditorWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Piece Builder");
        EditorGUILayout.Space();


        for (int i = 0, x = 0; x < gridSize; x++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < gridSize; y++)
            {
                checkBoxes[x, y] = EditorGUILayout.Toggle(checkBoxes[x,y], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                i++;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

    }


}
