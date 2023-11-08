using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckResidentsLikes : MonoBehaviour
{
    [SerializeField] private Resident currentResident;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance;
    [SerializeField] private FeedbackPopup feedbackPopup;
    private List<GameObject> feedbackInstances;
    private Vector3 previousPos;
    private List<Vector3> checkDirections;
    [SerializeField] private EventScriptable onPiecePlaced;
    int likeAmount;

    private void Start()
    {
        onPiecePlaced.AddListener(ValidatePosition);
        feedbackInstances = new List<GameObject>();
        previousPos = transform.position;


        checkDirections = new List<Vector3>
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };
        CheckRelations();
    }

    private void Update()
    {
        if (Vector3.Distance(previousPos, transform.position) > .5f)
        {
            CheckRelations();
            previousPos = transform.position;
        }
    }

    public void CheckRelations()
    {
        ClearFeedback();

        RaycastHit[] hit;

        likeAmount = 0;

        for (int i = 0; i < checkDirections.Count; i++)
        {
            hit = Physics.RaycastAll(transform.position, checkDirections[i], distance,mask);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.gameObject != gameObject)
                {
                    ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();
                    likeAmount = currentResident.CheckLikes(collidedResident.GetResidentRace());
                    if (likeAmount == 1)
                    {
                        FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[j].point, Quaternion.identity, transform);
                        obj.InitPopup(true);
                        feedbackInstances.Add(obj.gameObject);
                    }
                    else if (likeAmount == -1)
                    {
                        FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[j].point, Quaternion.identity, transform);
                        obj.InitPopup(false);
                        feedbackInstances.Add(obj.gameObject);
                    }
                }
            }
        }
        //Debug.DrawLine(transform.position + Vector3.back * distance /2, transform.position + Vector3.forward * distance/2, Color.cyan, 5);
    }

    public void ClearFeedback()
    {
        for (int i = feedbackInstances.Count - 1; i >= 0; i--)
        {
            Destroy(feedbackInstances[i].gameObject);
        }
        feedbackInstances.Clear();
    }

    public void ValidatePosition()
    {
        ClearFeedback();
        if(likeAmount > 0)
            ResidentManager.Instance.UpdateHappyResidents(likeAmount);
        else if(likeAmount < 0)
            ResidentManager.Instance.UpdateAngryResidents(likeAmount * -1);
        Debug.Log("like amount : " + likeAmount);
        Destroy(this);
    }

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(ValidatePosition);
    }
}
