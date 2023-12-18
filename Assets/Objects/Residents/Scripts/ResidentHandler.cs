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
    public int BlockLikeValue => currentBlockLikeValue;

    public Piece parentPiece;

    [SerializeField] private LayerMask cubeMask;

    public UnityEvent OnNeighborsChanged;


    public void SetResident(Resident res)
    {
        currentResident = res;
    }

    public void NewNeighbors(Race neighbors)
    {
        int likes = currentResident.CheckLikes(neighbors);
        //Debug.Log(currentResident.race.ToString() + " added value : " + likes +  " current block value before: " +currentBlockLikeValue);
        currentBlockLikeValue += likes;
        OnNeighborsChanged?.Invoke();
    }

    private void RemoveFromNeighbors(Race neighbors)
    {
        int likes = currentResident.CheckLikes(neighbors);
        currentBlockLikeValue -= likes;
        OnNeighborsChanged?.Invoke();
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

    private void OnDestroy()
    {
        RaycastHit[] hit;
        List<Vector3> checkDirections = new List<Vector3>
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        for (int i = 0; i < checkDirections.Count; i++)
        {
            hit = Physics.RaycastAll(this.gameObject.transform.position, checkDirections[i], 1, cubeMask);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.gameObject == gameObject)
                    continue;

                ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();
                if (collidedResident.parentPiece != parentPiece)
                    collidedResident.RemoveFromNeighbors(currentResident.race);
            }
        }
    }
}
