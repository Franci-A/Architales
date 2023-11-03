using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grid3DManager : MonoBehaviour
{
    [Header("Grid")]
    Dictionary<Vector3, Block> grid = new Dictionary<Vector3, Block>(); // x = right; y = up; z = forward;
    // Size of a block, WorldToGrid not working with every value
    private float cellSize = 1; // WIP. DO NOT MODIFY YET

    [Header("Mouse Check")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask blockLayer;

    [Header("Brick")]
    [SerializeField] private Piece brick;


    private List<Block> brickSO; // current list
    public List<Block> BrickSO { get => brickSO; } // get
    private List<Block> _brickSO; // check si ca a changer


    [SerializeField] List<BrickSO> brickSOList = new List<BrickSO>(); // liste des trucs random
    
    public delegate void OnBrickChangeDelegate(List<Block> newBrick);
    public event OnBrickChangeDelegate OnBrickChange;




    private static Grid3DManager instance;
    public static Grid3DManager Instance { get => instance;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        ChangeBrickSORandom();
    }


    void Update()
    {
        IsBlockChanged();
    }


    void IsBlockChanged()
    {
        if(_brickSO != BrickSO)
        {
            _brickSO = BrickSO;
            OnBrickChange(BrickSO);
            brick.ChangeBlocks(brickSO);
        }
    }

    void RotatePiece(bool rotateLeft)
    {
        brickSO =  brick.Rotate(rotateLeft);
    }
        


    void CanPlacePiece()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, blockLayer))
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

        var piece = Instantiate(brick, position, Quaternion.identity);
        piece.ChangeBlocks(BrickSO);
        
        if (!IsPiecePlaceable(piece, gridPos)) return;
        
        piece.SpawnBlock();

        foreach (var block in piece.Blocks)
        {
            grid.Add(block.pieceLocalPosition + gridPos, block);
            WeightManager.Instance.UpdateWeight(block.pieceLocalPosition + gridPos);
        }

        ChangeBrickSORandom();
    }



    public static Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        if (instance == null)
            throw new NullSingletonException();

        float size = instance.cellSize;
        return new Vector3(
            Mathf.Floor(worldPosition.x / size + .5f * size),
            .5f * size + Mathf.Floor(worldPosition.y / size),
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

    public static bool IsPiecePlaceable(Piece piece, Vector3 gridPosition)
    {
        if (instance == null)
            throw new NullSingletonException();

        if (piece == null) return false;

        foreach (var block in piece.Blocks)
        {
            if (instance.grid.ContainsKey(block.pieceLocalPosition + gridPosition)) return false;
        }

        return true;
    }


    private void ChangeBrickSORandom()
    {
        brickSO = brickSOList[Random.Range(0, brickSOList.Count)].Blocks;
    }



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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, blockLayer))
        {
            Vector3 gridPos = WorldToGridPosition(hit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gridPos, Vector3.one * .9f * cellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridPos + Vector3.down * cellSize * .5f, new Vector3(cellSize, 0, cellSize));
        }
    }
}
