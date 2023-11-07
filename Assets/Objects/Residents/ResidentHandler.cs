using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentHandler : MonoBehaviour
{
    [SerializeField] private Resident currentResident;

    public Race GetResidentRace()
    {
        return currentResident.race;
    }
}
