using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class EventSO : ScriptableObject
{
    public string eventName;
    public Sprite eventSprite;

    public PieceSO piece;

    public virtual void Activate() { }

    public virtual void Deactivate() { }

    public virtual void EndEvent() { }

    public void PlaceNewBlock()
    {
        Instance.GetPieceToSave();
        Instance.SetSavedPiece(piece, Instance.CurrentPieceSO);
    }
}
