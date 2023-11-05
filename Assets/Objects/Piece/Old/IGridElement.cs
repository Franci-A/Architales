using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridElement
{
    public void PlaceBlocks(RaycastHit hit);
}
