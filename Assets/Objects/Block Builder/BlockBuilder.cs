using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Block Builder/Builder Asset")]
public class BlockBuilder : ScriptableObject
{
    [SerializeField] private BlockAssetTypeRaceList blockAssetList;
    [SerializeField] private GridData gridData;
    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private Mesh hatMesh;

    public GameObject CreateBlock(Piece piece, Vector3 gridPosition)
    {
        /// Infos Necéssaires pour construire le bloc
        /// 
        /// Piece :
        ///     Race
        ///     Position dans la Piece
        ///         Dessus Vide / Bloqué ?
        ///         Côté Vide / Bloqué ?
        ///         Dessous Vide / Bloqué ?
        /// 
        /// Grid :
        ///     Voisins dans la Grille
        ///         Facade Vide / Bloquée ?
        ///         Toit Vide / Bloqué ?
        ///         Support Vide / Bloqué ?
        ///         

        var instance = Instantiate(blockPrefab, gridData.GridToWorldPosition(gridPosition), piece.transform.rotation, piece.transform);

        BuildAsset(instance, piece, gridPosition);

        return instance;
    }

    public void UpdateSurroundingBlocks(Vector3 gridPosition)
    {
        UpdateBlock(gridPosition + Vector3.up);
        UpdateBlock(gridPosition + Vector3.down);
    }

    private void UpdateBlock(Vector3 gridPosition)
    {
        // Exit if No existing block to Update
        if (gridData.IsPositionFree(gridPosition))
            return;

        var block = gridData.GetBlockAtPosition(gridPosition);
        var socketHandler = block.GetComponent<BlockSocketHandler>();
        var residentHandler = block.GetComponent<ResidentHandler>();

        var roofGridPosition = gridPosition + Vector3.up;

        if (!IsPositionFree(residentHandler.parentPiece, roofGridPosition))
            return;

        socketHandler.SetMesh(null);
    }

    private void BuildAsset(GameObject block, Piece piece, Vector3 gridPosition)
    {
        var socketHandler = block.GetComponent<BlockSocketHandler>();
        Race race = piece.GetResident.race;

        BuildRoof(socketHandler, piece, race, gridPosition);
    }

    private void BuildRoof(BlockSocketHandler socketHandler, Piece piece, Race race, Vector3 gridPosition)
    {
        var roofGridPosition = gridPosition + Vector3.up;

        if (!IsPositionFree(piece, roofGridPosition))
            return;

        socketHandler.SetMesh(blockAssetList.GetMeshByRaceAndType(race, BlockAssetType.ROOF));
    }

    private bool IsPositionFree(Piece piece, Vector3 gridPosition)
    {
        // False if Position is blocked
        if (!gridData.IsPositionFree(gridPosition))
            return false;

        // False if Piece contains Position
        var pieceGridPos = piece.GetGridPosition;
        foreach (var cube in piece.Cubes)
        {
            if (pieceGridPos + cube.gridPosition == gridPosition)
                return false;
        }

        return true;
    }
}
