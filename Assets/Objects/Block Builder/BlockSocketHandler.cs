using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSocketHandler : MonoBehaviour
{
    [SerializeField] private Decoration baseBlockSocket;
    [SerializeField] private Decoration roofSocket;
    [SerializeField] private Decoration supportSocket;
    [SerializeField] private GridData data;
    [SerializeField] private BlockAssetTypeRaceList assetList;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 debugPos;
    [SerializeField] private float roofStartingHeight = 1;
    [SerializeField] private float supportStartingHeight = 1;

    private List<Vector3> checkDirections;

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

    public void Init(Race residentRace)
    {
        var assets = assetList.GetMeshByRace(residentRace);
        if (assets != null)
        {
            baseBlockSocket.socket.SetMesh(assets.wallMesh);
            baseBlockSocket.socket.SetMaterial(assets.blockMaterial);
        }

        if (roofSocket.socket.transform.position.y > roofStartingHeight && CheckSocket(roofSocket, Vector3.up, Vector3.zero))
        {
            roofSocket.socket.SetMesh(assets.roofMesh);
            roofSocket.socket.SetMaterial(assets.roofMaterial);
            Vector2 gridPos = new Vector2(data.WorldToGridPositionRounded(roofSocket.socket.transform.position).x, data.WorldToGridPositionRounded(roofSocket.socket.transform.position).z);
            RoofManager.Instance.PiecePlaced(new RoofObject(roofSocket.socket.transform.position.y, this), gridPos);
        }
        if(supportSocket.socket.transform.position.y > supportStartingHeight && CheckSocket(supportSocket, Vector3.down, new Vector3(0, .2f,0))) 
        {
            supportSocket.socket.SetMesh(assets.supportMesh);
            supportSocket.socket.SetMaterial(assets.supportMaterial);
            GetSupportDirection();
        }
    }

    private bool CheckSocket(Decoration socket, Vector3 direction, Vector3 offset)
    {
        if (!socket.canBeFilled)
            return false;

        RaycastHit[] hit;

        hit = Physics.SphereCastAll(socket.socket.transform.position + offset, .1f, direction, .1f, layerMask);
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
                var renderer = supportSocket.socket.gameObject.GetComponent<MeshRenderer>();
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    renderer.materials[j].SetFloat("_ObjectRotation", i * 90);
                }
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
}

[Serializable]
struct Decoration
{
    public MeshSocket socket;
    public bool canBeFilled;
    public Vector3 blockOffset;
    public List<GameObject> neighbors;
}