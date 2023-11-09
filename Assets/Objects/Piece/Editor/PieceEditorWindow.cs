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

    //Save
    string scriptableName = "Enter scriptable name";
    string messageSave = "";

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


            for (int i = 0; i < gridSize; i++)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < gridSize; j++)
                {
                    checkBoxes[j, height, i] = EditorGUILayout.Toggle(checkBoxes[j, height, i], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
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

                    if (checkBoxes[j, height, i] != saveCheckBoxes[j, height, i])
                    {
                        saveCheckBoxes[j, height, i] = checkBoxes[j, height, i];
                        if (checkBoxes[j, height, i])
                        {
                            arrayGO[j, height, i] = UpdateCubeList(new Vector2(i, j), checkBoxes[j, height, i]);
                        }
                        else
                        {
                            DestroyImmediate(arrayGO[j, height, i]);
                            arrayGO[j, height, i] = null;
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

            #region Reset
            EditorGUILayout.Space(20);
            GUILayout.Label("Reset", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset")) ResetBlock();
            #endregion

            EditorGUILayout.Space(20);
            GUILayout.Label("Save To Scriptable", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            scriptableName = EditorGUILayout.TextArea(scriptableName);
            EditorGUILayout.Space();

            if(GUILayout.Button("Save !"))
            {
                EditorGUILayout.Space();
                if (scriptableName == "") 
                {
                    messageSave = "Please, enter a valid name";
                }
                else
                {
                    messageSave = "Saved !";
                    SaveBlock();
                }
            }

            EditorGUILayout.LabelField(messageSave);

        }

    }

    public GameObject UpdateCubeList(Vector3 pos, bool isActive)
    {
        if (isActive)
        {
            Debug.Log("test");
                var go = Instantiate(cube, new Vector3(pos.y, height, pos.x), new Quaternion());

                return go;
        }
        else
        {

                DestroyImmediate(arrayGO[(int)pos.y, height, (int)pos.x]);

            

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

    private void SaveBlock()
    {
        PieceSO newPiece = CreateInstance<PieceSO>();
        newPiece.cubes = new List<Cube>();
        string path = "Assets/Objects/Piece/TypeOfPiece/" + scriptableName + ".asset";
        AssetDatabase.CreateAsset(newPiece, path);

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    if (checkBoxes[i, j, k])
                    {
                        Cube cube = new Cube();
                        cube.pieceLocalPosition = new Vector3(i - 2, j, k - 2);    
                        newPiece.cubes.Add(cube);
                    }
                }
            }
        }
        

        EditorUtility.SetDirty(newPiece);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
