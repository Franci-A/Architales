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
    int gridHeight = 9;
    float buttonSpace = 15;
    bool[,,] checkBoxes = new bool[5,9,5];
    bool[,,] saveCheckBoxes = new bool[5,9,5];

    //Preview
    GameObject cube;
    GameObject[,,] arrayGO = new GameObject[5, 9, 5];

    //Height
    int height = 0;

    //Race
    Resident resident;

    //Save
    string scriptableName = "Enter scriptable name";
    string messageSave = "";
    bool saveToInGameList = false;

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
                    checkBoxes[j, height + 4, i] = EditorGUILayout.Toggle(checkBoxes[j, height + 4, i], GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            #endregion

            #region Spawn Blocks
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {

                    if (checkBoxes[j, height + 4, i] != saveCheckBoxes[j, height + 4, i])
                    {
                        saveCheckBoxes[j, height + 4, i] = checkBoxes[j, height + 4, i];
                        if (checkBoxes[j, height + 4, i])
                        {
                            arrayGO[j, height + 4, i] = UpdateCubeList(new Vector2(i, j), checkBoxes[j, height + 4, i]);
                        }
                        else
                        {
                            DestroyImmediate(arrayGO[j, height + 4, i]);
                            arrayGO[j, height + 4, i] = null;
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

            height = Mathf.Clamp(height, -4, 4);
            #endregion

            #region Reset
            EditorGUILayout.Space(20);
            GUILayout.Label("Reset", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset")) ResetBlock();
            #endregion

            #region Race
            EditorGUILayout.Space(20);
            GUILayout.Label("Race", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            resident = (Resident)EditorGUILayout.ObjectField( resident, typeof(Resident), true);
            #endregion

            #region Save

            EditorGUILayout.Space(20);
            GUILayout.Label("Save To Scriptable", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            scriptableName = EditorGUILayout.TextArea(scriptableName);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add this Piece to the 'in-game' list ?", EditorStyles.boldLabel);
            saveToInGameList =  EditorGUILayout.Toggle(saveToInGameList, GUILayout.Width(buttonSpace), GUILayout.Height(buttonSpace));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            if(GUILayout.Button("Save !"))
            {
                EditorGUILayout.Space();
                if (scriptableName == "") 
                {
                    messageSave = "Please, enter a valid name";
                }
                else if (resident == null)
                {
                    messageSave = "Please, set a valid resident scriptable object";
                }
                else
                {
                    messageSave = "Saved !";
                    SaveBlock();
                }
            }

            EditorGUILayout.LabelField(messageSave);
            #endregion

        }

    }

    public GameObject UpdateCubeList(Vector3 pos, bool isActive)
    {
        if (isActive)
        {
            Debug.Log("test");
                var go = Instantiate(cube, new Vector3(pos.y, height + 4, pos.x), new Quaternion());

                return go;
        }
        else
        {

                DestroyImmediate(arrayGO[(int)pos.y, height + 4, (int)pos.x]);

            

        }
        return null;
    }

    private void ResetBlock()
    {
        height = 0;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridHeight; j++)
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

        newPiece.resident = resident;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    if (checkBoxes[i, j, k])
                    {
                        Cube cube = new Cube();
                        cube.pieceLocalPosition = new Vector3(i - 2, j - 4, k - 2);    
                        newPiece.cubes.Add(cube);
                    }
                }
            }
        }
        

        EditorUtility.SetDirty(newPiece);


        if (saveToInGameList)
        {
            string inGameListPath = "Assets/Objects/Piece/ListOfBlocksSO.asset";
             ListOfBlocksSO list = (ListOfBlocksSO)AssetDatabase.LoadAssetAtPath(inGameListPath, typeof(ListOfBlocksSO));
            list.pieceList.Add(newPiece);
            EditorUtility.SetDirty(list);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
