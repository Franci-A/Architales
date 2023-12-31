using HelperScripts.EventSystem;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListPiece")]
public class ListOfBlocksSO : ScriptableObject, InitializeOnAwake, UninitializeOnDisable
{
    [Header("Resident List")]
    [SerializeField] private GameplayDataSO gameplayData;

    [Header("Events")]
    [SerializeField] private IntVariable happinessResidentGain;
    [SerializeField] private EventObjectScriptable lastPiecePlaced;
    [SerializeField] private EventScriptable updatePieceCountUI;
    [SerializeField] private EventScriptable onBalanceBroken;

    [Header("Resident List")]
    [SerializeField] private List<Resident> inGameResidents;
    [SerializeField] private List<PieceSO> pieceList;

    /// <summary>
    /// Library of every single piece per resident type
    /// </summary>
    private Dictionary<Resident, List<PieceSO>> residentSubList;

    private Dictionary<Resident, int> residentPiecesCount;

    public void Initialize()
    {
        GenerateResidentSubLists();
        GenerateResidentCount();

        lastPiecePlaced.AddListener(OnPiecePlaced);
    }

    public void Uninitialize()
    {
        residentSubList.Clear();
        residentPiecesCount.Clear();

        lastPiecePlaced.RemoveListener(OnPiecePlaced);
    }

    public int GetResidentPiecesCount(Race race)
    {
        foreach (var item in residentPiecesCount)
        {
            if(item.Key.race == race)
                return item.Value;
        }
        return 0;
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
        int index = Random.Range(0, residentPiecesCount.Keys.Count);
        var piece = GetRandomPieceFromLibrary(residentPiecesCount.ElementAt(index).Key);
        return piece;
    }

    private PieceSO GetRandomPieceFromLibrary(Resident resident)
    {
        int count = residentSubList[resident].Count;
        var piece = residentSubList[resident][Random.Range(0, count)];

        // Lose placed Piece
        UpdateResidentCount(resident, -1);

        return piece;
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

    private void GenerateResidentCount()
    {
        residentPiecesCount = new Dictionary<Resident, int>();
        foreach (var resident in inGameResidents)
            residentPiecesCount.Add(resident, gameplayData.InitialPiecesNumber);
    }

    private void UpdateResidentCount(Resident resident, int value)
    {
        int count = residentPiecesCount[resident] + value;

        residentPiecesCount[resident] = gameplayData.MaxPiecesNumber >= 0 ?
            Mathf.Min(count, gameplayData.MaxPiecesNumber) :
            count;
    }

    private void CheckForResidentsStillAvailable()
    {
        List<Resident> toRemove = new List<Resident>();

        foreach (var resident in residentPiecesCount)
        {
            if (resident.Value > 0) continue;

            toRemove.Add(resident.Key);
        }

        if (toRemove.Count <= 0) return;

        for (int i = 0; i < toRemove.Count; ++i)
            residentPiecesCount.Remove(toRemove[i]);

        CheckForResidentsLost();
    }

    private void CheckForResidentsLost()
    {
        if(residentPiecesCount.Count <= inGameResidents.Count - gameplayData.ResidentsToLoseGame)
        {
            onBalanceBroken?.Call();
        }
    }

    /// <summary>
    /// Called when a piece is placed
    /// </summary>
    /// <param name="lastPieceObject">Piece that was placed</param>
    private void OnPiecePlaced(object lastPieceObject)
    {
        if (lastPieceObject == null)
            return;

        PieceSO lastPiece = lastPieceObject as PieceSO;
        if (lastPiece == null || !inGameResidents.Contains(lastPiece.resident))
            return;

        ProcessHappinessGain(lastPiece.resident);
        CheckForResidentsStillAvailable();
        updatePieceCountUI.Call();
    }

    private void ProcessHappinessGain(Resident resident)
    {
        // Gain additional pieces if positive
        if (happinessResidentGain.value >= 1)
            UpdateResidentCount(resident, gameplayData.PositiveHappinessPieceGain);

        // Gain back a single piece if neutral
        else if (happinessResidentGain.value >= 0)
            UpdateResidentCount(resident, gameplayData.NeutralHappinessPieceGain);

        else
            UpdateResidentCount(resident, gameplayData.NegativeHappinessPieceGain);

        happinessResidentGain.SetValue(0);
    }

    [Button("Debug Sub List")]
    private void DebugSubListContent()
    {
        if(residentPiecesCount == null || residentPiecesCount.Count <= 0)
        {
            Debug.Log("No Pieces Available");
            return;
        }

        string str = $"{residentPiecesCount.Count} subLists:\n";
        foreach (var item in residentPiecesCount)
            str += $" - {item.Key.race}  | {item.Value} pieces\n";
        Debug.Log(str);
    }
}
