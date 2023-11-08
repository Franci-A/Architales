using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PieceEditorWindow : EditorWindow
{
    //Checkboxes grid
    int gridSize = 5;
    float buttonSpace = 15;
    bool[,] checkBoxes = new bool[5,5];

    //Preview
    GameObject gameObject;
    GameObject[] arrayGO;
    Editor gameObjectEditor;

    [MenuItem("Tools/Piece Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PieceEditorWindow));
    }

    void OnGUI()
    {
        #region Checkboxes

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
                if(x == 2 && y == 2) checkBoxes[x, y] = EditorGUILayout.Toggle(true, GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                else checkBoxes[x, y] = EditorGUILayout.Toggle(checkBoxes[x,y], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                i++;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        Rect rect = new Rect(0,0,10,10);

        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

        arrayGO = new GameObject[gridSize * gridSize];
        int id = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (checkBoxes[i, j])
                {
                    GameObject go = gameObject;
                    arrayGO[id] = go;
                }
                id++;
            }
        }

        
        EditorGUILayout.Space();

        if (EditorGUI.EndChangeCheck())
        {
            if (gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
        }

        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;

        if (gameObject != null)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(arrayGO);


            
            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(250, 250), bgColor);
        }

    }


}
