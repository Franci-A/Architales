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

    //public GameObject CreateBlock(Race race, Piece piece, Vector3 gridPosition)
    public GameObject CreateBlock(Piece piece, Vector3 gridPosition)
    {
        /// Infos Nec�ssaires pour construire le bloc
        /// 
        /// Piece :
        ///     Race
        ///     Position dans la Piece
        ///         Dessus Vide / Bloqu� ?
        ///         C�t� Vide / Bloqu� ?
        ///         Dessous Vide / Bloqu� ?
        /// 
        /// Grid :
        ///     Voisins dans la Grille
        ///         Facade Vide / Bloqu�e ?
        ///         Toit Vide / Bloqu� ?
        ///         Support Vide / Bloqu� ?
        ///         

        var instance = Instantiate(blockPrefab, gridData.GridToWorldPosition(gridPosition), piece.transform.rotation, piece.transform);

        BuildAsset(instance, gridPosition);

        return instance;
    }

    private void BuildAsset(GameObject block, Vector3 gridPosition)
    {
        var sockethandler = block.GetComponent<BlockSocketHandler>();

        sockethandler.SetMesh(hatMesh);
    }
}
