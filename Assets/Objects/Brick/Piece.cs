using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] List<Block> blocks = new List<Block>();
    [SerializeField] private GameObject blockGO;

    public List<Block> Blocks { get => blocks;}

    void Start()
    {
        foreach (var block in blocks)
        {
            Instantiate(blockGO, transform.position + block.pieceLocalPosition, transform.rotation, transform);
            block.gridPosition = Grid3DManager.WorldToGridPosition(transform.position) + block.pieceLocalPosition;
            WeightManager.Instance.UpdateWeight(block.gridPosition);
        }
    }

}

[Serializable]
public class Block
{
    public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
