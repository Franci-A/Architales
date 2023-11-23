using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostPreview : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GridData gridData;
    [SerializeField] private EventScriptable onPiecePlaced;

    [Header("GhostObject")]
    [SerializeField] Piece ghostPiecePrefab;
    [SerializeField] Material ghostMaterial;
    [SerializeField] Color validColor;
    [SerializeField] Color invalidColor;
    bool isBalanceBroken;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask cubeLayer;

    private Piece ghostPiece;
    private CheckResidentsLikes likes;
    bool isGhostActive = false;

    void Awake()
    {
        ghostMaterial.SetColor("_Color", Color.white);
    }

    private void Start()
    {
        Grid3DManager.Instance.OnCubeChange += OnPieceChange;
        Grid3DManager.Instance.onBalanceBroken.AddListener(BalanceBroken);
        onPiecePlaced.AddListener(EmptyGhost);

        ghostPiece = Instantiate(ghostPiecePrefab, transform);
        ghostPiece.PreviewSpawnPiece(Grid3DManager.Instance.pieceSo, ghostPiece.GetGridPosition);
        likes = GetComponentInChildren<CheckResidentsLikes>();
    }

    void Update()
    {
        if (!isGhostActive) return;
        if (isBalanceBroken) return;

        RaycastHit hit;
     
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            ghostPiece.gameObject.SetActive(true);

            Vector3 gridPos = gridData.WorldToGridPosition(hit.point + hit.normal / 4f);

            bool isPlaceable = gridData.IsPiecePlaceValid(ghostPiece, gridPos, out Vector3 validPos);
            if (isPlaceable)
                likes.CheckRelations();
            else
               likes.ClearFeedback();

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
        ghostPiece.PreviewSpawnPiece(newPiece, ghostPiece.GetGridPosition);

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
        isGhostActive = true;
    }

    private void EmptyGhost()
    {
        isGhostActive = false;
        if (ghostPiece == null)
            return; 
        ghostPiece.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(BalanceBroken);
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
        onPiecePlaced.RemoveListener(EmptyGhost);
    }

    private void BalanceBroken()
    {
        isBalanceBroken = true;
        ghostPiece.gameObject.SetActive(false);
    }
}
