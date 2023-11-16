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
        piece.ChangePiece(newPiece);
        piece.SpawnCubes();

        piece.transform.localPosition = centerPiecePos(newPiece);

    }

    private void Update()
    {
        parent.transform.Rotate(rotationVector, rotationSpeed, Space.World);
    }

    public Vector3 centerPiecePos(PieceSO _piece)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        float minZ = 0;
        float maxZ = 0;

        foreach (var cube in _piece.cubes)
        {
            if (cube.pieceLocalPosition.x < minX) minX = cube.pieceLocalPosition.x;
            else if (cube.pieceLocalPosition.x > maxX) maxX = cube.pieceLocalPosition.x;

            if (cube.pieceLocalPosition.y < minY) minY = cube.pieceLocalPosition.y;
            else if (cube.pieceLocalPosition.y > maxY) maxY = cube.pieceLocalPosition.y;

            if (cube.pieceLocalPosition.z < minZ) minZ = cube.pieceLocalPosition.z;
            else if (cube.pieceLocalPosition.z > maxZ) maxZ = cube.pieceLocalPosition.z;
        }

        return new Vector3((maxX - minX)/-2, (maxY - minY)/-2, (maxZ - minZ)/-2);
    }

}
