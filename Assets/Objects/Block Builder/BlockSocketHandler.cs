using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSocketHandler : MonoBehaviour
{
    [SerializeField] List<MeshSocket> sockets = new List<MeshSocket>();

    public void SetMesh(Mesh mesh)
    {
        sockets[0].SetMesh(mesh);
    }
}
