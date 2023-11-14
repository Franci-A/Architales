using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListPiece")]
public class ListOfBlocksSO : ScriptableObject
{
    public List<PieceSO> pieceList = new List<PieceSO>();

    public PieceSO GetRandomPiece()
    {
        return pieceList[Random.Range(0, pieceList.Count)];
    }
}
