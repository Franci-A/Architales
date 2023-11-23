using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshSocket : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
    }
}
