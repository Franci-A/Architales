using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostPreview : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GridData gridData;
    Grid3DManager.MouseMode mouseMode;

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

    [Header("Event")]
    [SerializeField] private EventScriptable onPrevivewDeactivated;
    [SerializeField] private EventScriptable onBalanceBroken;
    [SerializeField] private EventScriptable onEventCancel;
    [SerializeField] private EventScriptable onEventEnd;

    void Awake()
    {
        ghostMaterial.SetColor("_Color", Color.white);
    }

    private void Start()
    {
        Grid3DManager.Instance.OnCubeChange += OnPieceChange;
        onBalanceBroken.AddListener(IsBalanceBroken);
        onPrevivewDeactivated.AddListener(SwitchToAim);
        onEventCancel.AddListener(SwitchToPlace);
        onEventEnd.AddListener(SwitchToPlace);

        ghostPiece = Instantiate(ghostPiecePrefab, transform);
        ghostPiece.PreviewSpawnPiece(Grid3DManager.Instance.pieceSo, ghostPiece.GetGridPosition);
        likes = GetComponentInChildren<CheckResidentsLikes>();
    }

    void Update()
    {
        if (isBalanceBroken) return;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            if (mouseMode == Grid3DManager.MouseMode.PlacePiece)
            {
                ghostPiece.gameObject.SetActive(true);

                Vector3 gridPos = gridData.WorldToGridPosition(hit.point + hit.normal / 4f);

                bool isPlaceable = gridData.IsPiecePlaceValid(ghostPiece, gridPos, out Vector3 validPos);

                ghostMaterial.SetColor("_ValidColor", isPlaceable ? validColor : invalidColor);
                Vector3 pos = gridData.GridToWorldPosition(isPlaceable ? validPos : gridPos);

                ghostPiece.transform.position = pos;
            }
            else
            {

            }
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
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(IsBalanceBroken);
        Grid3DManager.Instance.OnCubeChange -= OnPieceChange;
    }
    void SwitchToAim()
    {
        SwitchMode(Grid3DManager.MouseMode.AimPiece);
    }
    void SwitchToPlace()
    {
        SwitchMode(Grid3DManager.MouseMode.PlacePiece);
    }

    private void SwitchMode(Grid3DManager.MouseMode newMode)
    {
         mouseMode = newMode;
    }

    void IsBalanceBroken()
    {
        isBalanceBroken = true;
        ghostPiece.gameObject.SetActive(false);
    }
}
