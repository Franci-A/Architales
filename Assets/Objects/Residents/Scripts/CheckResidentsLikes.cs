using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckResidentsLikes : MonoBehaviour
{
    private ResidentHandler currentResident;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance;
    [SerializeField] private FeedbackPopup feedbackPopup;
    private List<FeedbackPopup> feedbackInstances;
    private Vector3 previousPos;
    private List<Vector3> checkDirections;
    private List<ResidentHandler> neighbors;

    private void Awake()
    {
        feedbackInstances = new List<FeedbackPopup>();
        previousPos = transform.position;
        neighbors = new List<ResidentHandler>();    
        currentResident = GetComponent<ResidentHandler>();

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
        
        neighbors.Clear();
        int likeAmount = 0;

        for (int i = 0; i < checkDirections.Count; i++)
        {
            hit = Physics.RaycastAll(transform.position, checkDirections[i], distance,mask);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.gameObject != gameObject)
                {
                    ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();

                    if (collidedResident.parentPiece == currentResident.parentPiece)
                        continue;

                    neighbors.Add(collidedResident);
                    likeAmount = currentResident.GetResident.CheckLikes(collidedResident.GetResidentRace);
                    if (likeAmount == 1)
                    {
                        FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[j].point, Quaternion.identity);
                        obj.InitPopup(true);
                        feedbackInstances.Add(obj);
                    }
                    else if (likeAmount == -1)
                    {
                        FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[j].point, Quaternion.identity);
                        obj.InitPopup(false);
                        feedbackInstances.Add(obj);
                    }
                }
            }
        }
        //Debug.DrawLine(transform.position + Vector3.back * distance /2, transform.position + Vector3.forward * distance/2, Color.cyan, 5);
    }

    public void ClearFeedback()
    {
        if (feedbackInstances == null)
            return;

        for (int i = feedbackInstances.Count - 1; i >= 0; i--)
        {
            feedbackInstances[i].DestroyPopup();
        }
        feedbackInstances.Clear();
    }

    public void ValidatePosition()
    {
        ClearFeedback();
        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbors[i].NewNeighbors(currentResident.GetResidentRace);
            currentResident.NewNeighbors(neighbors[i].GetResidentRace);
        }
        Destroy(this);
    }
    private void OnDestroy()
    {
        ClearFeedback();
    }
}
