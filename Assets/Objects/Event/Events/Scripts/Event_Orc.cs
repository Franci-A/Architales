using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Orc")]
public class Event_Orc : EventSO
{
    public override void Activate() 
    {
        PlaceNewBlock();
    }

    public override void Deactivate() 
    {
        EventManager.Instance.SetSavedPiece();
    }

    public override void EndEvent()
    {
        Debug.Log("Orc piece placed >:D");    
    }
}
