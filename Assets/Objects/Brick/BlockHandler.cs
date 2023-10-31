using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour, IGridElement
{
    private List<GameObject> blocks = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            blocks.Add(transform.GetChild(i).gameObject);
        }
    }

    public void PlaceBlocks(RaycastHit hit)
    {
        GridManager.Instance.PlaceBlock(hit.point);
    }
}
