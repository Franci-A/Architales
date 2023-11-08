using HelperScripts.EventSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Grid3DManager : MonoBehaviour
{
    [Header("Grid")]
    Dictionary<Vector3, GameObject> grid = new Dictionary<Vector3, GameObject>(); // x = right; y = up; z = forward;
    // Size of a block, WorldToGrid not working with every value
    private float cellSize = 1; // WIP. DO NOT MODIFY YET

    int higherBlock = 1;
    public int GetHigherBlock { get => higherBlock; }

    [Header("Mouse Check")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private EventScriptable onPiecePlaced;

    [Header("Piece")]
    [SerializeField] private Piece piece;


    private List<Cube> cubeList; // current list
    public List<Cube> CubeList { get => cubeList; } // get
    [HideInInspector] private List<Cube> _cubeList; // check si ca a changer


    [SerializeField] PieceSO lobbyPiece;
    bool isLobby = true;
    [SerializeField] List<PieceSO> pieceListRandom = new List<PieceSO>(); // liste des trucs random
    
    public delegate void OnCubeChangeDelegate(List<Cube> newBrick);
    public event OnCubeChangeDelegate OnCubeChange;

    public delegate void OnLayerCubeChangeDelegate(int higherCubeValue);
    public event OnLayerCubeChangeDelegate OnLayerCubeChange;




    private static Grid3DManager instance;
    public static Grid3DManager Instance { get => instance;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);


    }

    private void Start()
    {
        SpawnBase(Vector3.zero);
        isLobby = false;
        ChangePieceSORandom();
        
    }

    void SpawnBase(Vector3 position)
    {
        cubeList = lobbyPiece.cubes;
        PlacePiece(position);
    }



    void Update()
    {
        IsBlockChanged();
    }


    void IsBlockChanged()
    {
        if(_cubeList != CubeList)
        {
            _cubeList = CubeList;
            OnCubeChange(CubeList);
            piece.ChangeCubes(cubeList);
        }
    }

    void RotatePiece(bool rotateLeft)
    {
        cubeList =  piece.Rotate(rotateLeft);
    }
        


    void CanPlacePiece()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, cubeLayer))
        {
            if (hit.normal != Vector3.up)
            {
                Debug.Log("cannot place here");
                //PlaceBlock(hit.point + hit.normal / 2);
            }
            else
                PlacePiece(hit.point);
        }
        
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
            WeightManager.Instance.UpdateWeight(block.pieceLocalPosition + gridPos);

            if (block.gridPosition.y > higherBlock) higherBlock = (int)block.gridPosition.y;
            
        }
        

        onPiecePlaced.Call();
        if(!isLobby) OnLayerCubeChange(higherBlock);
        ChangePieceSORandom();
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


    private void ChangePieceSORandom()
    {
        cubeList = pieceListRandom[Random.Range(0, pieceListRandom.Count)].cubes;
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
        if (context.ReadValue<float>() < 0) RotatePiece(true);
        else RotatePiece(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * maxDistance);

        if (instance != null)
        {
            for (int z = -1; z < 2; ++z)
                for (int x = -1; x < 2; ++x)
                    Gizmos.DrawWireCube(GridToWorldPosition(new Vector3(x, 0, z)) + Vector3.down * cellSize * .5f, new Vector3(cellSize, 0, cellSize));
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, cubeLayer))
        {
            Vector3 gridPos = GridToWorldPosition(WorldToGridPosition(hit.point));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gridPos, Vector3.one * .9f * cellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridPos + Vector3.down * cellSize * .5f, new Vector3(cellSize, 0, cellSize));
        }
    }
}
