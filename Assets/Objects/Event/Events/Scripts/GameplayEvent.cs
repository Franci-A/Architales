using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayEvent : ScriptableObject
{
    public string eventName;
    public Sprite eventSprite;

    public PieceSO piece;

    public virtual void Activate() { }

    public virtual void Deactivate() { }

    public virtual void EndEvent() { }

    public void PlaceNewBlock()
    {
        EventManager.Instance.GetPieceToSave();
        EventManager.Instance.SetSavedPiece(piece, EventManager.Instance.CurrentPieceSO);
    }
}
