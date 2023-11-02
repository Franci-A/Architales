using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid3DManager : MonoBehaviour
{
    Dictionary<Vector3, Block> grid = new Dictionary<Vector3, Block>(); // x = right; y = up; z = forward;

    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private Piece brick;

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


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
    }

    public void PlacePiece(Vector3 position)
    {
        Vector3 gridPos = WorldToGridPosition(position);
        if (!IsPiecePlaceable(brick, gridPos)) return;

        position = GridToWorldPosition(gridPos);

        var piece = Instantiate(brick, position, Quaternion.identity);

        foreach (var block in piece.Blocks)
        {
            grid.Add(block.pieceLocalPosition + gridPos, block);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * maxDistance);
    }

    public static Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector3(Mathf.Floor(worldPosition.x), Mathf.Floor(worldPosition.y), Mathf.Floor(worldPosition.z));
    }

    public static Vector3 GridToWorldPosition(Vector3 gridPosition)
    {
        return new Vector3(gridPosition.x + .5f, gridPosition.y + .5f, gridPosition.z + .5f);
    }

    public static bool IsPiecePlaceable(Piece piece, Vector3 gridPosition)
    { 

        if (piece == null || instance == null) return false;

        foreach (var block in piece.Blocks)
        {
            if (instance.grid.ContainsKey(block.pieceLocalPosition + gridPosition)) return false;
        }

        return true;
    }
}
