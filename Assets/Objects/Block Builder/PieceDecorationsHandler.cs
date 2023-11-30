using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDecorationsHandler : MonoBehaviour
{
    private BlockSocketHandler[] socketHandlers;

    public void Init()
    {
        socketHandlers = GetComponentsInChildren<BlockSocketHandler>();
        for (int i = 0; i < socketHandlers.Length; i++)
        {
            socketHandlers[i].Init();
        }
    }
}
