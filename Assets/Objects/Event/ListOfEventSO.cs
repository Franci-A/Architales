using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListEvent")]
public class ListOfEventSO : ScriptableObject
{
    public List<EventSO> eventList = new List<EventSO>();

    public EventSO GetRandomEvent()
    {

        return eventList[Random.Range(0, eventList.Count)];
    }
}
