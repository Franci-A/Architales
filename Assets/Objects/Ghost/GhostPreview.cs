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
    bool isBalanceBroken;

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
        Grid3DManager.Instance.onBalanceBroken.AddListener(BalanceBroken);
        ghostPiece = Instantiate(ghostPiecePrefab, transform);
        ghostPiece.ChangePiece(Grid3DManager.Instance.pieceSo);
        ghostPiece.SpawnCubes();

    }

    void Update()
    {
        if (isBalanceBroken) return;

        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            ghostPiece.gameObject.SetActive(true);

            Vector3 gridPos = gridData.WorldToGridPosition(hit.point);
            
            ghostMaterial.SetColor("_ValidColor", gridData.IsPiecePlaceable(ghostPiece, gridPos) ? validColor : invalidColor);

            Vector3 pos = gridData.GridToWorldPosition(gridPos);
            ghostPiece.transform.position = pos;

            /*if (hit.normal != Vector3.up)
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
            }*/


        }
        else ghostPiece.gameObject.SetActive(false);
    }

    private void OnPieceChange(PieceSO newPiece)
    {
        ghostPiece.ChangePiece(newPiece);
        ghostPiece.SpawnCubes();
        for (int i = 0; i < ghostPiece.Cubes.Count; i++)
        {
            Renderer rend = ghostPiece.Cubes[i].cubeGO.GetComponentInChildren<Renderer>();
            rend.SetMaterials(new List<Material>() { ghostMaterial});
        }
        ghostMaterial.SetColor("_BaseColor", newPiece.resident.blockColor);
        float alpha = validColor.a;
        validColor = newPiece.resident.blockColor;
        validColor.a = alpha;
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
    }

    private void BalanceBroken()
    {
        isBalanceBroken = true;
        ghostPiece.gameObject.SetActive(false);
    }
}
