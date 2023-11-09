using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PieceEditorWindow : EditorWindow
{
    //Checkboxes grid
    int gridSize = 5;
    float buttonSpace = 15;
    bool[,,] checkBoxes = new bool[5,5,5];
    bool[,,] saveCheckBoxes = new bool[5,5,5];

    //Preview
    GameObject cube;
    GameObject[,,] arrayGO = new GameObject[5, 5, 5];

    //Height
    int height = 0;

    [MenuItem("Tools/Piece Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PieceEditorWindow));
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Cube", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        cube = (GameObject)EditorGUILayout.ObjectField(cube, typeof(GameObject), true);
        EditorGUILayout.Space(20);


        if (cube == null) EditorGUILayout.LabelField("Please set cube 1x1x1");
        else
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
                    checkBoxes[height, x, y] = EditorGUILayout.Toggle(checkBoxes[height, x, y], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                    i++;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            #endregion

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            #region Spawn Blocks
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (checkBoxes[height, i, j] != saveCheckBoxes[height, i, j])
                    {
                        saveCheckBoxes[height, i, j] = checkBoxes[height, i, j];
                        if (checkBoxes[height, i, j]) arrayGO[height, i, j] = UpdateCubeList(new Vector2(i, j), checkBoxes[height, i, j]);
                        else
                        {
                            DestroyImmediate(arrayGO[height, i, j]);
                            arrayGO[height, i, j] = null;
                        }
                    }
                }
            }
            #endregion


            #region Height

            EditorGUILayout.Space(20);
            GUILayout.Label("Heigth", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            if (GUILayout.Button("+")) height++;
            EditorGUILayout.LabelField(height.ToString(), style);
            if (GUILayout.Button("-")) height--;

            height = Mathf.Clamp(height, 0, 4);
            #endregion


            EditorGUILayout.Space(20);
            GUILayout.Label("Reset", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset")) ResetBlock();
        }

    }

    public GameObject UpdateCubeList(Vector3 pos, bool isActive)
    {
        if (isActive)
        {

                var go = Instantiate(cube, new Vector3(pos.x, height, pos.y), new Quaternion());

                return go;
        }
        else
        {

                DestroyImmediate(arrayGO[height, (int)pos.x, (int)pos.y]);

            

        }
        return null;
    }

    private void ResetBlock()
    {
        height = 0;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    checkBoxes[i, j, k] = false;
                    DestroyImmediate(arrayGO[i, j, k]);
                    arrayGO[i, j, k] = null;
                }
            }
        }
    }

}
