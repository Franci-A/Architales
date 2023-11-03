using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    [Header("GhostObject")]
    [SerializeField] Piece ghostPiecePrefab;
    Piece ghostPiece;
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

    private void Start()
    {
        Grid3DManager.Instance.OnBrickChange += OnBrickChange;
        ghostPiece = Instantiate(ghostPiecePrefab, transform);
        ghostPiece.ChangeBlocks(Grid3DManager.Instance.BrickSO);
        ghostPiece.SpawnBlock();
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

    private void OnBrickChange(List<Block> newBrick)
    {
        ghostPiece.ChangeBlocks(newBrick);
        ghostPiece.SpawnBlock();
    }
}
