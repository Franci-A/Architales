using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceHappinessHandler : MonoBehaviour
{
    private int happinnessLevel;
    private int totalResidentLevel;
    List<ResidentHandler> residentHandlers;

    public int GetHappinessLevel => happinnessLevel;
    public Resident GetResident => residentHandlers[0].GetResident;

    private void Awake()
    {
        happinnessLevel = 0;
        totalResidentLevel = 0;
    }

    public void Init() 
    { 
        residentHandlers = GetComponentsInChildren<ResidentHandler>().ToList();
    }

    public int SetHappinessLevel()
    {
        totalResidentLevel = 0;
        happinnessLevel = 0;
        for (int i = 0; i < residentHandlers.Count; i++)
        {
            totalResidentLevel += residentHandlers[i].BlockLikeValue;
        }

        if (totalResidentLevel > 0)
            happinnessLevel = 1;
        else if (totalResidentLevel < 0)
            happinnessLevel = -1;

        ResidentManager.Instance.UpdateHappiness(happinnessLevel);

        return happinnessLevel;
    }

    public void RemoveResident(ResidentHandler handler)
    {
        residentHandlers.Remove(handler);
    }
}
