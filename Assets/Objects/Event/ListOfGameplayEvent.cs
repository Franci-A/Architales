using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListEvent")]
public class ListOfGameplayEvent : ScriptableObject
{
    public List<GameplayEvent> eventList = new List<GameplayEvent>();

    public GameplayEvent GetRandomEvent()
    {

        return eventList[Random.Range(0, eventList.Count)];
    }
}
