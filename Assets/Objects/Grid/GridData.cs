using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Grid Data")]
public class GridData : ScriptableObject, InitializeOnAwake, UninitializeOnDisable
{
    // Size of a block, WorldToGrid not working with every value
    [SerializeField] private float cellSize = 1; // WIP. DO NOT MODIFY YET
    public float CellSize { get => cellSize; }

    Dictionary<Vector3, GameObject> grid = new Dictionary<Vector3, GameObject>(); // x = right; y = up; z = forward;

    public void Initialize()
    {
        if (grid.Count == 0)
            return;
        grid.Clear();
    }

    public void Uninitialize()
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

    public Vector3 WorldToGridPositionRounded(Vector3 worldPosition)
    {
        return new Vector3(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y / cellSize - 0.5f * cellSize),
            Mathf.RoundToInt(worldPosition.z));
    }

    public Vector3 GridToWorldPosition(Vector3 gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
            gridPosition.y * cellSize + cellSize * .5f,
            gridPosition.z * cellSize);
    }

    /// <summary>
    /// Checks if <paramref name="piece"/> can be placed in the Grid, at <paramref name="gridPosition"/>.
    /// </summary>
    /// <param name="piece">Piece layout to check</param>
    /// <param name="gridPosition">Grid Position of the center block</param>
    /// <returns>True if nothing blocks the <paramref name="piece"/>, and has a supporting block.</returns>
    public bool IsPiecePlaceable(Piece piece, Vector3 gridPosition)
    {
        if (piece == null || piece.Cubes.Count == 0) return false;

        Vector3 blockGridPos;
        bool canPlace = false;
        
        foreach (var block in piece.Cubes)
        {
            blockGridPos = block.pieceLocalPosition + gridPosition;

            // False if any block is already occupied
            if (!IsPositionFree(blockGridPos)
                // OR Placed in the Center
                //|| (blockGridPos.x == 0 && blockGridPos.z == 0)
                || blockGridPos.y < 0
                )
                return false;

            // Check for at least One existing support underneath
            bool hasSupport = !IsPositionFree(blockGridPos + Vector3.down);

            canPlace = canPlace || hasSupport;
        }

        return canPlace;
    }

    /// <summary>
    /// Checks if <paramref name="piece"/> can be placed in the Grid, at <paramref name="gridPosition"/>.
    /// Also checks for specified special cases
    /// </summary>
    /// <param name="piece">Piece layout to check</param>
    /// <param name="gridPosition">Grid Position of the center block</param>
    /// <param name="validPosition">First Valid Position for the Piece found</param>
    /// <returns>True if a <paramref name="validPosition"/> is found for the <paramref name="piece"/> at <paramref name="gridPosition"/></returns>
    public bool IsPiecePlaceValid(Piece piece, Vector3 gridPosition, out Vector3 validPosition)
    {
        Vector3 testedPosition = gridPosition;
        bool value = IsPiecePlaceable(piece, testedPosition);
        
        if(value)
        {
            validPosition = testedPosition;
            return true;
        }

        // Check one block above
        testedPosition += Vector3.up * cellSize;
        value = IsPiecePlaceable(piece, testedPosition);

        validPosition = value ? testedPosition : Vector3.one * -1f;
        return value;
    }


    /// <summary>
    /// Checks if piece at <paramref name="gridPosition"/> can be deleted.
    /// Also checks for specified special cases
    /// </summary>
    /// <param name="gridPosition">Grid Position of the center block</param>
    /// 
    public bool IsPieceDeletable(Vector3 gridPosition)
    {
        bool canPlace = true;

        // False if any block is free
        if (IsPositionFree(gridPosition)
            // OR Placed on the ground
            || (gridPosition.y == 0))
            return false;      

        return canPlace;
    }


    public void AddToGrid(Vector3 gridPosition, GameObject go)
    {
        if (!IsPositionFree(gridPosition))
            throw new Exception($"Already existing block at position {gridPosition} !");

        grid.Add(gridPosition, go);
    }

    public void RemoveToGrid(Vector3 gridPosition)
    {
        grid.Remove(gridPosition);
    }

    public void ShowAllBlocks()
    {
        foreach (var item in grid)
            item.Value.SetActive(true);
    }

    public void HideBlocksAtHeight(int height)
    {
        foreach (var item in grid)
            item.Value.SetActive(item.Key.y <= height);
    }

    public List<GameObject> GetCubes()
    {
        List<GameObject> cubes = new List<GameObject>();

        foreach (var item in grid)
        {
            cubes.Add(item.Value);
        }
        return cubes;
    }

    public GameObject GetBlockAtPosition(Vector3 gridPosition)
    {
        if(IsPositionFree(gridPosition))
            throw new Exception($"No existing block at position {gridPosition} !");

        return grid[gridPosition];
    }

    public bool IsPositionFree(Vector3 gridPosition)
    {
        return !grid.ContainsKey(gridPosition);
    }
}
