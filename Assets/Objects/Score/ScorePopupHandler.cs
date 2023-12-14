using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopupHandler : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float upMoveAmount = 10;
    [SerializeField] private float basePunchAmount;
    public void Init(string value, int punchAmount)
    {
        text.text = value;
        Vector3 punchValue = new Vector3(basePunchAmount *punchAmount, basePunchAmount * punchAmount, basePunchAmount * punchAmount);
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + upMoveAmount, 1).SetEase(Ease.OutQuad));
        seq.Insert(.25f, transform.DOPunchScale(punchValue, .5f));
        seq.Insert(.25f, transform.DOPunchRotation(new Vector3(0,0,45) * punchAmount, .5f));
        seq.Insert(.75f ,text.DOColor(Color.clear, .5f));
        seq.onComplete += DestroyPopup;
        seq.Play();

    }

    private void DestroyPopup()
    {
        Destroy(gameObject);
    }
}
