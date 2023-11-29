using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListPiece")]
public class ListOfBlocksSO : ScriptableObject, InitializeOnAwake, UninitializeOnDisable
{
    [SerializeField] private List<Resident> inGameResidents;
    [SerializeField] private List<PieceSO> pieceList;
    [SerializeField] private int initialPiecesNumber;

    /// <summary>
    /// Library of every single piece per resident type
    /// </summary>
    private Dictionary<Resident, List<PieceSO>> residentSubList;
    
    /// <summary>
    /// Gameplay purpose lists, populated and incremented/decremented during game
    /// </summary>
    private Dictionary<Resident, List<PieceSO>> gameplayPiecesSubList;

    public void Initialize()
    {
        GenerateResidentSubLists();
        GeneratePiecesList();
    }

    public void Uninitialize()
    {
        residentSubList?.Clear();
        gameplayPiecesSubList?.Clear();
    }

    /// <summary>
    /// Called from Piece Editor
    /// </summary>
    /// <param name="piece"></param>
    public void AddPiece(PieceSO piece)
    {
        pieceList.Add(piece);
    }

    public PieceSO GetRandomPiece()
    {
        return GetRandomPiece(inGameResidents[Random.Range(0, inGameResidents.Count)]);
    }

    /// <summary>
    /// Take Piece from <paramref name="resident"/>'s Sub List
    /// </summary>
    /// <param name="resident">Piece's Type of Resident</param>
    /// <returns></returns>
    public PieceSO GetRandomPiece(Resident resident)
    {
        int count = gameplayPiecesSubList[resident].Count;
        int index = Random.Range(0, count);

        var piece = gameplayPiecesSubList[resident][index];
        gameplayPiecesSubList[resident].RemoveAt(index);

        Debug.Log($"Took 1 piece from the {piece.resident.race}s, {--count} left..");

        return piece;
    }

    private PieceSO GetRandomPieceFromLibrary(Resident resident)
    {
        int count = residentSubList[resident].Count;
        return residentSubList[resident][Random.Range(0, count)];
    }

    private void GenerateResidentSubLists()
    {
        // Generate Dictionnary
        residentSubList = new Dictionary<Resident, List<PieceSO>>();
        foreach (var resident in inGameResidents)
            residentSubList.Add(resident, new List<PieceSO>());

        // Populate each sub list from piece's resident
        foreach (var item in pieceList)
            residentSubList[item.resident].Add(item);
    }

    private void GeneratePiecesList()
    {
        // Generate Dictionnary
        gameplayPiecesSubList = new Dictionary<Resident, List<PieceSO>>();
        foreach (var resident in inGameResidents)
        {
            gameplayPiecesSubList.Add(resident, new List<PieceSO>());
            // Populate each sub list from the "Library"'s choices
            for (int i = 0; i < initialPiecesNumber; ++i)
                gameplayPiecesSubList[resident].Add(GetRandomPieceFromLibrary(resident));
        }
    }

    [Button("Debug Sub List")]
    private void DebugSubListContent()
    {
        if(gameplayPiecesSubList == null || gameplayPiecesSubList.Count <= 0)
        {
            Debug.Log("Empty Piece Sub Lists");
            return;
        }

        string str = $"{gameplayPiecesSubList.Count} subLists:\n";
        foreach (var item in gameplayPiecesSubList)
            str += $" - {item.Key.race}  | {item.Value.Count} pieces\n";
        Debug.Log(str);
    }
}
