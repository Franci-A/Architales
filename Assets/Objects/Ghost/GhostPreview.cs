using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private LayerMask cubeLayer;

    void Awake()
    {
        ghostMaterial.SetColor("_Color", Color.white);
    }

    private void Start()
    {
        Grid3DManager.Instance.OnCubeChange += OnPieceChange;
        ghostPiece = Instantiate(ghostPiecePrefab, transform);
        ghostPiece.ChangePiece(Grid3DManager.Instance.pieceSo);
        ghostPiece.SpawnCubes();
    }

    void Update()
    {
        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
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
                ghostPiece.transform.position = Grid3DManager.GridToWorldPosition(Grid3DManager.WorldToGridPosition(hit.point));

            }
            else
            {
                ghostMaterial.SetColor("_Color", validColor);
            }
                
        }else ghostPiece.gameObject.SetActive(false);
    }

    private void OnPieceChange(PieceSO newPiece)
    {
        ghostPiece.ChangePiece(newPiece);
        ghostPiece.SpawnCubes();
    }
}
