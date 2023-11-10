using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostPreview : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GridData gridData;

    [Header("GhostObject")]
    [SerializeField] Piece ghostPiecePrefab;
    [SerializeField] Material ghostMaterial;
    [SerializeField] Color validColor;
    [SerializeField] Color invalidColor;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask cubeLayer;

    Piece ghostPiece;

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

            Vector3 gridPos = gridData.WorldToGridPosition(hit.point + hit.normal / 4f);
            
            ghostMaterial.SetColor("_Color", gridData.IsPiecePlaceable(ghostPiece, gridPos) ? validColor : invalidColor);

            Vector3 pos = gridData.GridToWorldPosition(gridPos);
            ghostPiece.transform.position = pos;
        }
        else ghostPiece.gameObject.SetActive(false);
    }

    private void OnPieceChange(PieceSO newPiece)
    {
        ghostPiece.ChangePiece(newPiece);
        ghostPiece.SpawnCubes();
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
    }
}
