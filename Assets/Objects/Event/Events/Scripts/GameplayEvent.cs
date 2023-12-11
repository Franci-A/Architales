using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayEvent : ScriptableObject
{
    [Header("Event")]
    public string eventName;
    public Sprite eventSprite;

    [Header("Datas")]
    public GameObject soundPrefab;
    public PieceSO piece;
    public int cooldown;

    public virtual void Activate() { }

    public virtual void Deactivate() { }

    public virtual void EndEvent() 
    {
        if(soundPrefab != null) Instantiate(soundPrefab);
    }

    public void PlaceNewBlock()
    {
        EventManager.Instance.GetPieceToSave();
        EventManager.Instance.SetSavedPiece(piece, EventManager.Instance.CurrentPieceSO);
    }
}
