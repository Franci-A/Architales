using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPiecePreview : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private EventObjectScriptable onPiecePlacedPiece;

    [Header("Piece")]
    [SerializeField] Piece piecePrefab;
    Piece piece;

    [Header("Rotation")]
    [SerializeField] Vector3 rotationVector;
    [SerializeField] float rotationSpeed;
    Transform parent;

    private void Awake()
    {
        Debug.Log("Add listener");
        onPiecePlacedPiece.AddListener(OnPieceChange);
        parent = transform.GetChild(0);
    }

    private void OnPieceChange(object _newPiece)
    {
        if (piece == null)
        {
            piece = Instantiate(piecePrefab, transform);
            piece.transform.SetParent(parent);
        }

        //piece.transform.SetParent(null);

        parent.rotation = new Quaternion(0,0,0,0);

        PieceSO newPiece = (PieceSO)_newPiece;
        piece.PreviewSpawnPiece(newPiece, piece.GetGridPosition);

        piece.transform.localPosition = piece.centerPiecePos(newPiece);

    }

    private void Update()
    {
        parent.transform.Rotate(rotationVector, rotationSpeed, Space.World);
    }

    private void OnDestroy()
    {
        onPiecePlacedPiece.RemoveListener(OnPieceChange);
    }
}
