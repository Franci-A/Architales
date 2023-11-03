using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckResidentsLikes : MonoBehaviour
{
    [SerializeField] private Resident currentResident;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance;
    [SerializeField] private FeedbackPopup feedbackPopup;
    private List<FeedbackPopup> feedbackInstances;

    private void Start()
    {
        feedbackInstances = new List<FeedbackPopup>();
    }

    private void Update()
    {
        CheckRelations();
    }

    public void CheckRelations()
    {
        for (int i = feedbackInstances.Count; i > 0; i--)
        {
            Destroy(feedbackInstances[i].gameObject);
        }

        RaycastHit[] hit;
        hit = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, distance,mask);

        for (int i = 0; i < hit.Length; i++)
        {
            if(hit[i].collider.gameObject != gameObject)
            {
                ResidentHandler collidedResident = hit[i].collider.gameObject.GetComponent<ResidentHandler>();
                int like = currentResident.CheckLikes(collidedResident.GetResidentRace());
                if (like == 1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(true);
                    feedbackInstances.Add(obj);
                }
                else if (like == -1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(false);
                    feedbackInstances.Add(obj);
                }
            }
        }

        hit = Physics.RaycastAll(transform.position + Vector3.right, Vector3.left, distance, mask);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != gameObject)
            {
                ResidentHandler collidedResident = hit[i].collider.gameObject.GetComponent<ResidentHandler>();
                int like = currentResident.CheckLikes(collidedResident.GetResidentRace());
                if (like == 1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(true);
                    feedbackInstances.Add(obj);
                }
                else if (like == -1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(false);
                    feedbackInstances.Add(obj);
                }
            }
        }
    }

    public void ValidatePosition()
    {
        //update happy / angry gauge
        Destroy(this);
    }
}
