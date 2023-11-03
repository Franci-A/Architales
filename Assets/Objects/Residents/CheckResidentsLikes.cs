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

    private void Start()
    {
        feedbackInstances = new List<GameObject>();
        previousPos = transform.position;
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
        for (int i = feedbackInstances.Count -1; i >= 0; i--)
        {
            Destroy(feedbackInstances[i].gameObject);
        }
        feedbackInstances.Clear();

        RaycastHit[] hit;
        hit = Physics.RaycastAll(transform.position + Vector3.back * distance / 2, Vector3.forward, distance,mask);
        Debug.DrawLine(transform.position + Vector3.back * distance /2, transform.position + Vector3.forward * distance, Color.cyan, 5);
        Debug.Log(hit.Length);
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
                    feedbackInstances.Add(obj.gameObject);
                }
                else if (like == -1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(false);
                    feedbackInstances.Add(obj.gameObject);
                }
            }
        }

        hit = Physics.RaycastAll(transform.position + Vector3.right *distance / 2, Vector3.left, distance, mask);
        Debug.DrawLine(transform.position + Vector3.right * distance, transform.position + Vector3.left * distance, Color.red, 5);

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
                    feedbackInstances.Add(obj.gameObject);
                }
                else if (like == -1)
                {
                    FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[i].point, Quaternion.identity);
                    obj.InitPopup(false);
                    feedbackInstances.Add(obj.gameObject);
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
