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
    bool[,] saveCheckBoxes = new bool[5,5];

    //Preview
    //CubeEditor cubeEditor;
    GameObject cubeEditor;
    GameObject[,] arrayGO = new GameObject[5, 5];
    Editor gameObjectEditor;

    Mesh newMesh;

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
                checkBoxes[x, y] = EditorGUILayout.Toggle(checkBoxes[x,y], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                i++;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        Rect rect = new Rect(0,0,10,10);

        cubeEditor = (GameObject)EditorGUILayout.ObjectField(cubeEditor, typeof(GameObject), true);
        EditorUtility.SetDirty(cubeEditor);
        newMesh = (Mesh)EditorGUILayout.ObjectField(newMesh, typeof(Mesh), true);

        if (cubeEditor != null)
        {
            string path = "Assets/Objects/Piece/Editor/CubeEditor.prefab";
            var go = PrefabUtility.LoadPrefabContents(path);



            PrefabUtility.SaveAsPrefabAsset(go, path);


            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if(checkBoxes[i, j] != saveCheckBoxes[i, j])
                    {
                        saveCheckBoxes[i, j] = checkBoxes[i, j];
                        if (checkBoxes[i, j]) arrayGO[i, j] = go.GetComponent<CubeEditor>().UpdateCubeList(new Vector2(i, j), checkBoxes[i, j]);
                        else 
                        {
                            DestroyImmediate(arrayGO[i, j]); 
                            arrayGO[i, j] = null;
                        }
                    }
                }
            }

        }

        EditorGUILayout.Space();

        if (EditorGUI.EndChangeCheck())
        {
            if (gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
        }

        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;

        if (cubeEditor != null)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(cubeEditor);


            
            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(250, 250), bgColor);
        }

    }


}
