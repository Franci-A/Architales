using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Solid Block")]
public class Event_SolidBlock : GameplayEvent
{
    [Header("Personal datas")]
    public float addBalance;
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
        base.EndEvent();
        Grid3DManager.Instance.AddBalance(addBalance);
    }
}
