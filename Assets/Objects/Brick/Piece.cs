using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject blockGO;

    BrickSO blocks;
    public BrickSO Blocks { get => blocks; }

    public void SpawnBlock()
    {
        foreach (var block in blocks.Blocks)
        {
            Instantiate(blockGO, transform.position + block.pieceLocalPosition, transform.rotation, transform);
            block.gridPosition = Grid3DManager.WorldToGridPosition(transform.position) + block.pieceLocalPosition;

        }
    }

    public void ChangeBlocks(BrickSO _blocks)
    {
        if(_blocks.Blocks.Count < 0) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        blocks = _blocks;
    }

}

[Serializable]
public class Block
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
