using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    [Header("GhostObject")]
    [SerializeField] Piece ghostPiece;
    [SerializeField] Material ghostMaterial;
    [SerializeField] Color validColor;
    [SerializeField] Color invalidColor;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask blockLayer;

    void Awake()
    {
        ghostMaterial.SetColor("_Color", Color.white);
    }

    void Update()
    {
        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, blockLayer))
        {
            ghostPiece.gameObject.SetActive(true);
            ghostPiece.transform.position = Grid3DManager.GridToWorldPosition(Grid3DManager.WorldToGridPosition(hit.point));

            if (hit.normal != Vector3.up)
            {
                ghostMaterial.SetColor("_Color", invalidColor);
                ghostPiece.transform.position = Grid3DManager.GridToWorldPosition(Grid3DManager.WorldToGridPosition(hit.point + hit.normal / 2));
            }
            else if(!Grid3DManager.IsPiecePlaceable(ghostPiece, Grid3DManager.WorldToGridPosition(hit.point)))
            {
                ghostMaterial.SetColor("_Color", invalidColor);
                ghostPiece.transform.position = Grid3DManager.GridToWorldPosition(Grid3DManager.WorldToGridPosition(hit.point + hit.normal / 2));

            }
            else
            {
                ghostMaterial.SetColor("_Color", validColor);
            }
                
        }else ghostPiece.gameObject.SetActive(false);
    }
}
