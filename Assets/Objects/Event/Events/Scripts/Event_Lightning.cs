using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Lightning")]
public class Event_Lightning : GameplayEvent
{
    [Space]
    public GameObject lightningEffect;
    private GameObject lightningsave;

    public override void Activate()
    {
        EventManager.Instance.CallOnPreviewDeactivated();
        Grid3DManager.Instance.SwitchMouseMode(Grid3DManager.MouseMode.AimPiece);
        lightningsave = Instantiate(lightningEffect);
    }
    public override void Deactivate()
    {
        Grid3DManager.Instance.SwitchMouseMode(Grid3DManager.MouseMode.PlacePiece);
        Destroy(lightningsave);
    }
}
