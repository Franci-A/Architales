using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PiecesCountUIHandler : MonoBehaviour
{
    [Serializable]
    private struct ResidentCountUIText
    {
        public Race ResidentRace;
        public TextMeshProUGUI TextUI;
    }

    [SerializeField] private ListOfBlocksSO blocksList;
    [SerializeField] private EventScriptable updatePieceCountUI;
    [SerializeField] private List<ResidentCountUIText> residentCountTexts;

    private void Start()
    {
        updatePieceCountUI.AddListener(UpdateCountUI);
        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        foreach (var ui in residentCountTexts)
        {
            int count = blocksList.GetResidentPiecesCount(ui.ResidentRace);
            ui.TextUI.text = count.ToString();
            ui.TextUI.color = count <= 0 ? Color.red : Color.white;
        }
    }

    private void OnDestroy()
    {
        updatePieceCountUI.RemoveListener(UpdateCountUI);
    }
}
