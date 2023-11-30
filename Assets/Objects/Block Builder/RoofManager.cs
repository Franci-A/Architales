using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofManager : MonoBehaviour
{
    private static RoofManager instance;
    public static RoofManager Instance { get { return instance; } }

    [SerializeField] private GridData data;
    Dictionary<Vector2, RoofObject> grid = new Dictionary<Vector2, RoofObject>(); // x = right; z = forward;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        Initialize();
    }

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

    public void PiecePlaced(RoofObject obj, Vector2 gridPosition)
    {
        var roofObject = GetBlockAtPosition(gridPosition);
        if(roofObject == null)
        {
            AddToGrid(gridPosition, obj);
            obj.socketHandler.ActivateRoof();
            return;
        }

        if( obj.yHeight > roofObject.yHeight ) 
        {
            roofObject.socketHandler.RemoveRoof();
            AddToGrid(gridPosition, obj);
            obj.socketHandler.ActivateRoof();
        }
    }

    public void AddToGrid(Vector2 gridPosition, RoofObject go)
    {
        if (!IsPositionFree(gridPosition))
        {
            grid[gridPosition] = go;
        }else
            grid.Add(gridPosition, go);
    }

    public void RemoveFromGrid(Vector2 gridPosition)
    {
        grid[gridPosition].socketHandler.RemoveRoof();
        grid.Remove(gridPosition);
    }

    public RoofObject GetBlockAtPosition(Vector2 gridPosition)
    {
        if (IsPositionFree(gridPosition))
            return null;

        return grid[gridPosition];
    }

    public bool IsPositionFree(Vector2 gridPosition)
    {
        return !grid.ContainsKey(gridPosition);
    }
}

public class RoofObject
{
    public float yHeight;
    public BlockSocketHandler socketHandler;

    public RoofObject( float y, BlockSocketHandler handler)
    {
        this.yHeight = y;
        this.socketHandler = handler;
    }
}
