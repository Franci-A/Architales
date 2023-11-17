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


    private Vector3 debugPosition;
    private float debugDistance = .5f;
    public void SetResident(Resident res)
    {
        currentResident = res;

        cube.SetMaterials(new List<Material>() { currentResident.blockMaterial });
    }

    public void NewNeighbors(Race neighbors)
    {
        int likes = currentResident.CheckLikes(neighbors);
        //Debug.Log(currentResident.race.ToString() + " added value : " + likes +  " current block value before: " +currentBlockLikeValue);
        currentBlockLikeValue += likes;
        OnNeighborsChanged?.Invoke();
    }

    //add change neighbor if jenga mechanic is added

    public void ShowRelationsMaterial(Vector3 position, Color color)
    {
        cube.materials[0].SetFloat("_UseOutline", 1);
        cube.materials[0].SetVector("_Position", position);
        cube.materials[0].SetColor("_OutlineColor", color);
        debugPosition = position;
        debugDistance= cube.materials[0].GetFloat("_OutlineDistance");
    }
    
    public void RemoveRelationsMaterial()
    {
        cube?.materials[0].SetFloat("_UseOutline", 0);
    }
}
