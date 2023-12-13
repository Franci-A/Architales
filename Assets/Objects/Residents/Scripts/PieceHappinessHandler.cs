using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceHappinessHandler : MonoBehaviour
{
    private int happinnessLevel;
    private int totalResidentLevel;
    List<ResidentHandler> residentHandlers;

    private void Awake()
    {
        happinnessLevel = 0;
        totalResidentLevel = 0;
    }


    public void Init() 
    { 
        residentHandlers = GetComponentsInChildren<ResidentHandler>().ToList();
        for (int i = 0; i < residentHandlers.Count; i++)
        {
            residentHandlers[i].OnNeighborsChanged.AddListener(UpdateHappiness);
        }
    }

    private void UpdateHappiness()
    {
        totalResidentLevel = 0;
        for (int i = 0; i < residentHandlers.Count; i++)
        {
            totalResidentLevel += residentHandlers[i].BlockLikeValue;
        }

        int currentMood = happinnessLevel;

        if (totalResidentLevel > 0)
            happinnessLevel = 1;
        else if (totalResidentLevel < 0)
            happinnessLevel = -1;
        
        currentMood = currentMood * (-1) + happinnessLevel; // remove previous value from gauge and add new value
        ResidentManager.Instance.UpdateResidentsHappiness(currentMood);
    }
    public int GetHappinessLevel()
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
        
        return happinnessLevel;
    }

    public void RemoveResident(ResidentHandler handler)
    {
        residentHandlers.Remove(handler);
        UpdateHappiness();
    }

    private void OnDestroy()
    {
        if(residentHandlers == null)
            return;

        for (int i = 0; i < residentHandlers.Count; i++)
        {
            residentHandlers[i].OnNeighborsChanged.RemoveListener(UpdateHappiness);
        }
    }
}
