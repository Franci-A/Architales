using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListPiece")]
public class ListOfBlocksSO : ScriptableObject, InitializeOnAwake, UninitializeOnDisable
{
    [SerializeField] private List<Resident> inGameResidents;
    [SerializeField] private List<PieceSO> pieceList;

    private Dictionary<Resident, List<PieceSO>> residentSubList;

    public void Initialize()
    {
        GenerateResidentSubLists();
    }

    public void Uninitialize()
    {
        residentSubList?.Clear();
    }

    public void AddPiece(PieceSO piece)
    {
        pieceList.Add(piece);
    }

    public PieceSO GetRandomPiece()
    {
        return GetRandomPiece(inGameResidents[Random.Range(0, inGameResidents.Count)]);
    }

    public PieceSO GetRandomPiece(Resident resident)
    {
        var subList = residentSubList[resident];
        int index = Random.Range(0, subList.Count);

        var piece = subList[index];
        subList.RemoveAt(index);

        return piece;
    }

    private void GenerateResidentSubLists()
    {
        residentSubList = new Dictionary<Resident, List<PieceSO>>();
        foreach (var resident in inGameResidents)
            residentSubList.Add(resident, new List<PieceSO>());

        foreach (var item in pieceList)
            residentSubList[item.resident].Add(item);
    }

    [Button("Debug Sub List")]
    private void DebugSubListContent()
    {
        string str = $"{residentSubList.Count} subLists:\n";
        foreach (var item in residentSubList)
            str += $" - {item.Key.race}  | {item.Value.Count} pieces\n";
        Debug.Log(str);
    }
}
