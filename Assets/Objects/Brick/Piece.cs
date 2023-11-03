using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] List<Block> blocks = new List<Block>();
    [SerializeField] private GameObject blockGO;

    public List<Block> Blocks { get => blocks; }

    void Start()
    {
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        foreach (var block in blocks)
        {
            Instantiate(blockGO, transform.position + block.pieceLocalPosition, transform.rotation, transform);
            block.gridPosition = Grid3DManager.WorldToGridPosition(transform.position) + block.pieceLocalPosition;

        }
    }

    public void ChangeBlocks(List<Block> _blocks)
    {
        if(_blocks.Count < 0) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        blocks = _blocks;
        
        SpawnBlock();
    }

}

[Serializable]
public class Block
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
