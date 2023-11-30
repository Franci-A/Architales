using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlockSocketHandler : MonoBehaviour
{
    [SerializeField] List<MeshSocket> sideSockets = new List<MeshSocket>();
    [SerializeField] Decoration roofSocket;
    [SerializeField] GridData data;
    private List<Vector3> checkDirections;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        checkDirections = new List<Vector3>
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };
        roofSocket.canBeFilled = true;
    }

    public void Init()
    {
        CheckRoof();
    }
    
    private void CheckRoof()
    {
        if (!roofSocket.canBeFilled)
            return;

        RaycastHit[] hit;

        hit = Physics.RaycastAll(roofSocket.socket.transform.position, Vector3.up, 1, layerMask);
        Debug.DrawRay(roofSocket.socket.transform.position, Vector3.up, Color.blue, 2);

        if (hit.Length > 0)
        {
            bool hasHit = false;
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject != this.gameObject)
                    hasHit = true;
            }
            if (hasHit)
            {

                Debug.Log("hit something");
                roofSocket.canBeFilled = false;
                roofSocket.socket.EmptyMesh();
                roofSocket.socket.gameObject.SetActive(false);
                return;
            }
        }
        //check if empty
        if (!data.IsPositionFree(data.WorldToGridPositionRounded(roofSocket.socket.transform.position)))
        {
            roofSocket.canBeFilled = false;
            roofSocket.socket.EmptyMesh();
            roofSocket.socket.gameObject.SetActive(false);
            return;
        }
        Vector2 gridPos = new Vector2(data.WorldToGridPositionRounded(roofSocket.socket.transform.position).x, data.WorldToGridPositionRounded(roofSocket.socket.transform.position).z);
        RoofManager.Instance.PiecePlaced(new RoofObject(roofSocket.socket.transform.position.y, this), gridPos);

        /*
        //check if has neighbors
        roofSocket.neighbors = new List<GameObject>();
        for (int i = 0; i < checkDirections.Count; i++)
        {
            Debug.DrawRay(roofSocket.socket.transform.position, checkDirections[i], Color.blue, 2);
            Vector3 gridPosition = data.WorldToGridPosition(roofSocket.socket.transform.position + checkDirections[i]);
            if (data.IsPositionFree(gridPosition))
                continue;

            roofSocket.neighbors.Add(data.GetBlockAtPosition(gridPosition));
        }*/

    }

    public void ActivateRoof()
    {
        roofSocket.socket.ActivateMesh();
    }
    public void RemoveRoof()
    {
        roofSocket.socket.EmptyMesh();
    }
}

[Serializable]
struct Decoration
{
    public MeshSocket socket;
    public bool canBeFilled;
    public Vector3 blockOffset;
    public List<GameObject> neighbors;
}