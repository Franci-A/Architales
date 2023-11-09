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
        ghostPiece.ChangeCubes(Grid3DManager.Instance.CubeList);
        ghostPiece.SpawnCubes();
    }

    void Update()
    {
        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            ghostPiece.gameObject.SetActive(true);
            ghostPiece.transform.position = gridData.GridToWorldPosition(gridData.WorldToGridPosition(hit.point));

            if (hit.normal != Vector3.up)
            {
                ghostMaterial.SetColor("_Color", invalidColor);
                ghostPiece.transform.position = gridData.GridToWorldPosition(gridData.WorldToGridPosition(hit.point + hit.normal / 2));
            }
            else if (!gridData.IsPiecePlaceable(ghostPiece, gridData.WorldToGridPosition(hit.point)))
            {
                ghostMaterial.SetColor("_Color", invalidColor);
                ghostPiece.transform.position = gridData.GridToWorldPosition(gridData.WorldToGridPosition(hit.point));
            }
            else
            {
                ghostMaterial.SetColor("_Color", validColor);
            }
                
        }else ghostPiece.gameObject.SetActive(false);
    }

    private void OnPieceChange(List<Cube> newBrick)
    {
        ghostPiece.ChangeCubes(newBrick);
        ghostPiece.SpawnCubes();
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
    }
}
