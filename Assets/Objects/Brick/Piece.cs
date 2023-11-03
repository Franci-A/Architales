using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject blockGO;

    List<Block> blocks = new List<Block>();
    public List<Block> Blocks { get => blocks; }

    public void SpawnBlock()
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
    }


    public List<Block> Rotate(bool rotateLeft)
    {
        Quaternion rotation;
        if (rotateLeft)
        {
            rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }

        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        List<Block> rotatedBlocks = new List<Block>();

        Debug.Log(blocks.Count);

        foreach (Block block in blocks)
        {
            Block newBlock = new Block();
            newBlock.pieceLocalPosition = m.MultiplyPoint3x4(block.pieceLocalPosition);
            rotatedBlocks.Add(newBlock);
        }

        ChangeBlocks(rotatedBlocks);
        return rotatedBlocks;
    }

}

[Serializable]
public class Block
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
