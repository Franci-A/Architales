using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] List<Block> blocks = new List<Block>();
    [SerializeField] private GameObject blockGO;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var block in blocks)
        {
            Instantiate(blockGO, transform.position + block.pieceLocalPosition, transform.rotation, transform);
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
    Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
}
