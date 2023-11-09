using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentHandler : MonoBehaviour
{
    [SerializeField] private Resident currentResident;
    [SerializeField] private Renderer cube;
    public Race GetResidentRace()
    {
        return currentResident.race;
    }

    public void SetResident(Resident res)
    {
        currentResident = res;

        cube.SetMaterials(new List<Material>() { currentResident.blockMaterial });
        //GetComponentInChildren<Renderer>().materials[0].SetColor("_Color", currentResident.blockColor);
    }
}
