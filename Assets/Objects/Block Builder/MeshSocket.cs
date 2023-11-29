using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshSocket : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh mesh;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void SetMesh(Mesh mesh)
    {
        this.mesh = mesh;
        meshFilter.mesh = mesh;
    }

    public void ActivateMesh()
    {
        meshFilter.mesh = mesh;
    }

    public void EmptyMesh()
    {
        meshFilter.mesh = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ghost"))
        {
            //EmptyMesh();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {   
           // ActivateMesh();
        }

    }
}
