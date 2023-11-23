using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Lightning")]
public class Event_Lightning : EventSO
{
    public override void Activate()
    {
        EventManager.Instance.CallOnPreviewDeactivated();
        Grid3DManager.Instance.SwitchMouseMode(Grid3DManager.MouseMode.AimPiece);
    }
    public override void Deactivate()
    {
        Grid3DManager.Instance.SwitchMouseMode(Grid3DManager.MouseMode.PlacePiece);
    }
}
