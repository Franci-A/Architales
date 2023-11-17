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
    [SerializeField] private LayerMask cubeLayer;

    Piece ghostPiece;
    private CheckResidentsLikes likes;

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
        likes = GetComponentInChildren<CheckResidentsLikes>();
    }

    void Update()
    {
        if (isBalanceBroken) return;

        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            ghostPiece.gameObject.SetActive(true);

            Vector3 gridPos = gridData.WorldToGridPosition(hit.point + hit.normal / 4f);

            bool isPlaceable = gridData.IsPiecePlaceValid(ghostPiece, gridPos, out Vector3 validPos);

            ghostMaterial.SetColor("_ValidColor", isPlaceable ? validColor : invalidColor);
            Vector3 pos = gridData.GridToWorldPosition(isPlaceable ? validPos : gridPos);

            ghostPiece.transform.position = pos;
        }
        else ghostPiece.gameObject.SetActive(false);
    }

    private void OnPieceChange(PieceSO newPiece)
    {
        likes.isAcive = false;
        likes.ClearFeedback();
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
        likes.Init(ghostPiece.Cubes);
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(BalanceBroken);
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
    }

    private void BalanceBroken()
    {
        isBalanceBroken = true;
        ghostPiece.gameObject.SetActive(false);
    }
}
