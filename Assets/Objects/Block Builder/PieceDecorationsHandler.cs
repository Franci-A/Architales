using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceDecorationsHandler : MonoBehaviour
{
    private List<BlockSocketHandler> socketHandlers;

    public void Init(Race residentRace)
    {
        socketHandlers = GetComponentsInChildren<BlockSocketHandler>().ToList() ;
        for (int i = 0; i < socketHandlers.Count; i++)
        {
            socketHandlers[i].Init(residentRace);
        }
    }

    public void RemoveSocket(BlockSocketHandler handler)
    {
        socketHandlers.Remove(handler);
    }

}
