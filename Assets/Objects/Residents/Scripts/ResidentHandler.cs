using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ResidentHandler : MonoBehaviour
{
    [SerializeField] private Resident currentResident;
    public Resident GetResident => currentResident;
    public Race GetResidentRace => currentResident.race;
    
    [SerializeField] private Renderer cube;
    [SerializeField] private BlockSocketHandler socketHandler;

    private int currentBlockLikeValue = 0;
    public int BlockLikeValue {get { return currentBlockLikeValue; } set { currentBlockLikeValue = value; } }

    public Piece parentPiece;

    [SerializeField] private LayerMask cubeMask;

    public void SetResident(Resident res)
    {
        currentResident = res;
    }

    public void ShowRelationsMaterial(Vector3 position, Color color)
    {
        cube.materials[0].SetFloat("_UseOutline", 1);
        cube.materials[0].SetVector("_Position", position);
        cube.materials[0].SetColor("_OutlineColor", color);
    }
    
    public void RemoveRelationsMaterial()
    {
        if(cube == null)
            return;
        cube.materials[0].SetFloat("_UseOutline", 0);
        if(socketHandler != null)
            socketHandler.ResetBlockMaterial();
    }
}
