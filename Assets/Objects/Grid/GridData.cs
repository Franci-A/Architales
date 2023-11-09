using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Grid Data")]
public class GridData : ScriptableObject
{
    // Size of a block, WorldToGrid not working with every value
    [SerializeField] private float cellSize = 1; // WIP. DO NOT MODIFY YET
    public float CellSize { get => cellSize; }

    Dictionary<Vector3, GameObject> grid = new Dictionary<Vector3, GameObject>(); // x = right; y = up; z = forward;

    public void Initialize()
    {
        grid.Clear();
    }

    public Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector3(
            Mathf.Floor(worldPosition.x / cellSize + .5f * cellSize),
            Mathf.Floor(worldPosition.y / cellSize + .5f * cellSize),
            Mathf.Floor(worldPosition.z / cellSize + .5f * cellSize));
    }

    public Vector3 GridToWorldPosition(Vector3 gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
        gridPosition.y * cellSize + .5f * cellSize,
            gridPosition.z * cellSize);
    }

    public bool IsPiecePlaceable(Piece piece, Vector3 gridPosition)
    {
        if (piece == null || piece.Cubes.Count == 0) return false;

        bool hasSupportBlock = false;
        foreach (var block in piece.Cubes)
        {
            if (grid.ContainsKey(block.pieceLocalPosition + gridPosition)
                || (block.pieceLocalPosition.x + gridPosition.x == 0 && block.pieceLocalPosition.z + gridPosition.z == 0)) return false;

            // Check for existing support underneath
            // hasSupportBlock = hasSupportBlock || instance.grid.ContainsKey(block.pieceLocalPosition + gridPosition + Vector3.down);
        }

        return true;
        //return hasSupportBlock;
    }

    public void AddToGrid(Vector3 gridPosition, GameObject go)
    {
        if (grid.ContainsKey(gridPosition))
            throw new Exception($"Already existing block at position {gridPosition} !");

        grid.Add(gridPosition, go);
    }

    public void ShowAllBlocks()
    {
        foreach (var item in grid)
            item.Value.SetActive(true);
    }

    public void HideBlocksAtHeight(int height)
    {
        foreach (var item in grid)
            item.Value.SetActive(item.Key.y < height);
    }
}
