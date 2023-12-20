using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static ResidentManager Instance;
    [SerializeField] private IntVariable happinessGain;
    [SerializeField] private GameplayDataSO gameplayData;
    [SerializeField] private EventObjectScriptable onPiecePlaced;
    [SerializeField] private EventObjectScriptable onHappinessWithResident;

    private int value = -1;
    private Resident resident = null;

    private void Awake()
    {
        Instance = this;
        happinessGain.SetValue(0);
        onPiecePlaced.AddListener(UpdateResident);
    }

    public void UpdateHappiness(int value)
    {
        happinessGain.SetValue(value);
        this.value = value;
        SendInfo();
    }
    
    public void UpdateResident(object obj)
    {
        PieceHappinessHandler piece = obj as PieceHappinessHandler;
        resident = piece.GetResident;
        SendInfo();
    }


    private void SendInfo()
    {
        if (resident == null || value == -1)
            return;

        onHappinessWithResident.Call(new ResidentInfo(value, resident));
        value = -1;
        resident = null;
    }

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(UpdateResident);

    }
}
class ResidentInfo
{
    public int value;
    public Resident resident;

    public ResidentInfo(int value, Resident resident)
    {
        this.value = value; 
        this.resident = resident;
    }
}
