using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResidentHandler : MonoBehaviour
{
    [SerializeField] private Resident currentResident;
    public Resident GetResident => currentResident;
    public Race GetResidentRace => currentResident.race;
    
    [SerializeField] private Renderer cube;

    private int currentBlockLikeValue = 0;
    public int BlockLikeValue => currentBlockLikeValue;

    public Piece parentPiece;


    public UnityEvent OnNeighborsChanged;
    
    public void SetResident(Resident res)
    {
        currentResident = res;

        cube.SetMaterials(new List<Material>() { currentResident.blockMaterial });
    }

    public void NewNeighbors(Race neighbors)
    {
        int likes = currentResident.CheckLikes(neighbors);
        Debug.Log(currentResident.race.ToString() + " added value : " + likes +  " current block value before: " +currentBlockLikeValue);
        currentBlockLikeValue += likes;
        OnNeighborsChanged?.Invoke();
    }

    //add change neighbor if jenga mechanic is added
}
