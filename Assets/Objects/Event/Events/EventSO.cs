using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(menuName = "ScriptableObjects/Event")]
public class EventSO : ScriptableObject
{
    public string eventName;
    public Sprite eventSprite;
    public TypeEvent eventType;
}
