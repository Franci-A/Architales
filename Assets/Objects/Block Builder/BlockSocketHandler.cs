using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlockSocketHandler : MonoBehaviour
{
    [SerializeField] Decoration roofSocket;
    [SerializeField] Decoration supportSocket;
    [SerializeField] GridData data;
    private List<Vector3> checkDirections;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 debugPos;
    [SerializeField] private float roofStartingHeight = 1;
    [SerializeField] private float supportStartingHeight = 1;

    private void Awake()
    {
        checkDirections = new List<Vector3>
        {
            Vector3.right,
            Vector3.back,
            Vector3.left,
            Vector3.forward
        };
        roofSocket.canBeFilled = true;
        supportSocket.canBeFilled = true;
    }

    public void Init()
    {
        if (roofSocket.socket.transform.position.y > roofStartingHeight && CheckSocket(roofSocket, Vector3.up, Vector3.zero))
        {
                Vector2 gridPos = new Vector2(data.WorldToGridPositionRounded(roofSocket.socket.transform.position).x, data.WorldToGridPositionRounded(roofSocket.socket.transform.position).z);
                RoofManager.Instance.PiecePlaced(new RoofObject(roofSocket.socket.transform.position.y, this), gridPos);
            
        }
        if(supportSocket.socket.transform.position.y > supportStartingHeight && CheckSocket(supportSocket, Vector3.down, new Vector3(0, .2f,0))) 
        {
            GetSupportDirection();
        }
    }

    private bool CheckSocket(Decoration socket, Vector3 direction, Vector3 offset)
    {
        if (!socket.canBeFilled)
            return false;

        RaycastHit[] hit;

        hit = Physics.SphereCastAll(socket.socket.transform.position + offset, .1f, direction, .1f, layerMask);
        Debug.DrawRay(socket.socket.transform.position, direction, Color.blue, 2);
        debugPos = socket.socket.transform.position + offset;
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
                socket.canBeFilled = false;
                socket.socket.EmptyMesh();
                socket.socket.gameObject.SetActive(false);
                return false;
            }
        }
        //check if empty
        if (!data.IsPositionFree(data.WorldToGridPositionRounded(socket.socket.transform.position)))
        {
            socket.canBeFilled = false;
            socket.socket.EmptyMesh();
            socket.socket.gameObject.SetActive(false);
            return false;
        }

        return true;

    }

    private void GetSupportDirection()
    {
        RaycastHit[] hit;

        for (int i = 0; i < checkDirections.Count; i++)
        {
            hit = Physics.RaycastAll(supportSocket.socket.transform.position + Vector3.up * .2f, checkDirections[i], 1, layerMask);
            if(hit.Length > 0)
            {
                supportSocket.socket.ActivateMesh();
                supportSocket.socket.transform.localEulerAngles = new Vector3(0, 90 * i, 0);
                supportSocket.socket.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_ObjectRotation", i * 90);
                break;
            }
        }

    }

    public void ActivateRoof()
    {
        roofSocket.socket.ActivateMesh();
    }
    public void RemoveRoof()
    {
        roofSocket.socket.EmptyMesh();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(debugPos, .5f);
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