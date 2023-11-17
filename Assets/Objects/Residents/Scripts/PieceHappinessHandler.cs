using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHappinessHandler : MonoBehaviour
{
    private int happinnessLevel;
    private int totalResidentLevel;
    ResidentHandler[] residentHandlers;

    private void Awake()
    {
        happinnessLevel = 0;
        totalResidentLevel = 0;
    }


    public void Init() 
    { 
        residentHandlers = GetComponentsInChildren<ResidentHandler>();
        for (int i = 0; i < residentHandlers.Length; i++)
        {
            residentHandlers[i].OnNeighborsChanged.AddListener(UpdateHappiness);
        }
    }

    private void UpdateHappiness()
    {
        totalResidentLevel = 0;
        for (int i = 0; i < residentHandlers.Length; i++)
        {
            totalResidentLevel += residentHandlers[i].BlockLikeValue;
        }

        int currentMood = happinnessLevel;

        if (totalResidentLevel > 0)
            happinnessLevel = 1;
        else if (totalResidentLevel < 0)
            happinnessLevel = -1;
        
        Debug.Log(residentHandlers[0].GetResidentRace.ToString() + " piece happiness : " + happinnessLevel + " total happiness : " +  totalResidentLevel);
        currentMood = currentMood * (-1) + happinnessLevel; // remove previous value from gauge and add new value
        ResidentManager.Instance.UpdateResidentsHappiness(currentMood);
    }

    private void OnDestroy()
    {
        if(residentHandlers == null)
            return;

        for (int i = 0; i < residentHandlers.Length; i++)
        {
            residentHandlers[i].OnNeighborsChanged.RemoveListener(UpdateHappiness);
        }
    }
}
