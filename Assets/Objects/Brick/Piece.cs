using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] List<Block> blocks = new List<Block>();
    [SerializeField] private GameObject blockGO;

    public List<Block> Blocks { get => blocks;}

    // Start is called before the first frame update
    void Start()
    {
        foreach (var block in blocks)
        {
            Instantiate(blockGO, transform.position + block.pieceLocalPosition, transform.rotation, transform);
            block.gridPosition = Grid3DManager.WorldToGridPosition(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class Block
{
    public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
