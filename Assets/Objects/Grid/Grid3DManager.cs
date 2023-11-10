using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grid3DManager : MonoBehaviour
{
    // Singleton
    private static Grid3DManager instance;
    public static Grid3DManager Instance { get => instance; }

    [Header("Grid")]
    Dictionary<Vector3, GameObject> grid = new Dictionary<Vector3, GameObject>(); // x = right; y = up; z = forward;
    // Size of a block, WorldToGrid not working with every value
    private float cellSize = 1; // WIP. DO NOT MODIFY YET

    [Header("Weight")]
    [SerializeField] float maxBalance;
    [SerializeField] private Material displacementShaderMat;
    [SerializeField] private float shaderAnimTime;
    [SerializeField] private AnimationCurve shaderAnimCurve;

    [Header("Residents")]
    [SerializeField] private IntVariable totalNumResidents;

    [Header("Mouse Check")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask cubeLayer;

    [Header("Piece")]
    [SerializeField] PieceSO lobbyPiece; 
    [SerializeField] private Piece piece;
    [SerializeField] List<PieceSO> pieceListRandom = new List<PieceSO>(); // liste des trucs random

    [Header("Event")]
    [SerializeField] private EventScriptable onPiecePlaced;

    [Header("Debug")]
    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();
    
    private List<Cube> cubeList; // current list
    public List<Cube> CubeList { get => cubeList; } // get
    private List<Cube> _cubeList; // check si ca a changer

    public delegate void OnCubeChangeDelegate(List<Cube> newBrick);
    public event OnCubeChangeDelegate OnCubeChange;

    public delegate void OnLayerCubeChangeDelegate(int higherCubeValue);
    public event OnLayerCubeChangeDelegate OnLayerCubeChange;

    int higherBlock = 1;
    public int GetHigherBlock { get => higherBlock; }

    bool isLobby = true;
    private Vector2 balance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        onPiecePlaced.AddListener(UpdateDisplacement);
        
        SpawnBase(Vector3.zero);
        isLobby = false;
        ChangePieceSORandom();
    }

    private void Update()
    {
        IsBlockChanged();
        UpdateWeightDebug();
    }

    //INPUTS
    public void LeftClickInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        CanPlacePiece();
    }

    public void RotatePieceInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RotatePiece(context.ReadValue<float>() < 0);
    }

    public void PlacePiece(Vector3 position)
    {
        Vector3 gridPos = WorldToGridPosition(position);

        position = GridToWorldPosition(gridPos);

        var piece = Instantiate(this.piece, position, Quaternion.identity);
        piece.ChangeCubes(CubeList);

        if (!IsPiecePlaceable(piece, gridPos)) return;

        piece.SpawnCubes();

        foreach (var block in piece.Cubes)
        {
            grid.Add(block.pieceLocalPosition + gridPos, block.cubeGO);
            UpdateWeight(block.pieceLocalPosition + gridPos);

            if (block.gridPosition.y > higherBlock)
                higherBlock = (int)block.gridPosition.y;
        }

        totalNumResidents.Add(piece.Cubes.Count);
        onPiecePlaced.Call();
        if (!isLobby)
            OnLayerCubeChange(higherBlock);
        ChangePieceSORandom();
    }

    private void SpawnBase(Vector3 position)
    {
        cubeList = lobbyPiece.cubes;
        PlacePiece(position);
    }

    private void IsBlockChanged()
    {
        if (_cubeList != CubeList)
        {
            _cubeList = CubeList;
            OnCubeChange(CubeList);
            piece.ChangeCubes(cubeList);
        }
    }

    private void RotatePiece(bool rotateLeft)
    {
        cubeList =  piece.Rotate(rotateLeft);
    }

    private void CanPlacePiece()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            if (hit.normal != Vector3.up)
                Debug.Log("cannot place here");
            else
                PlacePiece(hit.point);
        }
    }

    private void ChangePieceSORandom()
    {
        cubeList = pieceListRandom[Random.Range(0, pieceListRandom.Count)].cubes;
    }

    private void UpdateWeight(Vector3 blockPosition)
    {
        balance.x += blockPosition.x;
        balance.y += blockPosition.z;
    }

    private void UpdateDisplacement()
    {
        displacementShaderMat.SetVector("_LeaningDirection", balance.normalized);
        displacementShaderMat.SetFloat("_MaxHeight", higherBlock);

        StartCoroutine(BalanceDisplacementRoutine());
    }

    private void SetDisplacementValue(float value)
    {
        displacementShaderMat.SetFloat("_Value", value);
    }

    private void ResetDisplacement()
    {
        displacementShaderMat.SetFloat("_UseAngle", 0);
        displacementShaderMat.SetFloat("_MaxHeight", 1f);
        displacementShaderMat.SetVector("_LeaningDirection", Vector2.zero);
        SetDisplacementValue(0f);
    }

    private IEnumerator BalanceDisplacementRoutine()
    {
        float timer = shaderAnimTime;
        float maxValue = Mathf.Clamp01(Mathf.Max(Mathf.Abs(balance.x), Mathf.Abs(balance.y)) / maxBalance);
        float t;
        do
        {
            // 0-1 of time elapsed
            t = 1 - Mathf.InverseLerp(0f, shaderAnimTime, timer);
            SetDisplacementValue(shaderAnimCurve.Evaluate(t) * maxValue);

            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        } while (timer > 0);

        SetDisplacementValue(0f);
    }

    private void UpdateWeightDebug()
    {
        for (int i = 0; i < DebugInfo.Count; i++)
        {
            DebugInfo[i].transform.rotation = Quaternion.LookRotation(DebugInfo[i].transform.position - Camera.main.transform.position);

            switch (i)
            {
                case 0:
                    DebugInfo[i].text = Mathf.Max(-balance.x, 0).ToString();
                    break;

                case 1:
                    DebugInfo[i].text = Mathf.Max(balance.x, 0).ToString();
                    break;

                case 2:
                    DebugInfo[i].text = Mathf.Max(-balance.y, 0).ToString();
                    break;

                default:
                    DebugInfo[i].text = Mathf.Max(balance.y, 0).ToString();
                    break;
            }
        }

        if (Mathf.Abs(balance.x) > maxBalance)
        {
            DebugInfo[0].color = Color.red;
            DebugInfo[1].color = Color.red;
        }

        if (Mathf.Abs(balance.y) > maxBalance)
        {
            DebugInfo[2].color = Color.red;
            DebugInfo[3].color = Color.red;
        }
    }

    public static Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        if (instance == null)
            throw new NullSingletonException();

        float size = instance.cellSize;
        return new Vector3(
            Mathf.Floor(worldPosition.x / size + .5f * size),
            Mathf.Floor(worldPosition.y / size + .5f * size),
            Mathf.Floor(worldPosition.z / size + .5f * size));
    }

    public static Vector3 GridToWorldPosition(Vector3 gridPosition)
    {
        if (instance == null)
            throw new NullSingletonException();

        float size = instance.cellSize;
        return new Vector3(
            gridPosition.x * size,
            gridPosition.y * size + .5f * size,
            gridPosition.z * size);
    }

    public void ShowLayer(int layerHeight)
    {
        foreach (var item in grid)
        {
            if (item.Key.y > layerHeight)
            {
                item.Value.SetActive(false);
            }
            else
            {
                item.Value.SetActive(true);
            }
        }
    }

    public static bool IsPiecePlaceable(Piece piece, Vector3 gridPosition)
    {
        if (instance == null)
            throw new NullSingletonException();

        if (piece == null) return false;

        foreach (var block in piece.Cubes)
        {
            if (instance.grid.ContainsKey(block.pieceLocalPosition + gridPosition)
                || (block.pieceLocalPosition.x + gridPosition.x == 0 && block.pieceLocalPosition.z + gridPosition.z == 0)) return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(UpdateDisplacement);
        ResetDisplacement();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).origin, Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).origin + Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).direction * maxDistance);

        if (instance != null)
        {
            for (int z = -1; z < 2; ++z)
                for (int x = -1; x < 2; ++x)
                    Gizmos.DrawWireCube(GridToWorldPosition(new Vector3(x, 0, z)) + Vector3.down * cellSize * .5f, new Vector3(cellSize, 0, cellSize));
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            Vector3 gridPos = GridToWorldPosition(WorldToGridPosition(hit.point));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gridPos, Vector3.one * .9f * cellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridPos + Vector3.down * cellSize * .5f, new Vector3(cellSize, 0, cellSize));
        }
    }
}
