using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckResidentsLikes : MonoBehaviour
{
    private List<ResidentHandler> currentResident;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance;
    [SerializeField] private FeedbackPopup feedbackPopup;
    private List<Vector3> checkDirections;
    private List<FeedbackElement> feedbackElements;
    [SerializeField] private Color likeColor = Color.green;
    [SerializeField] private Color dislikeColor = Color.red;
    private FeedbackPopup popup;

    Vector3 previousPos;

    public bool isAcive =false;

    public void Init(List<Cube> children)
    {
        isAcive = true;
        currentResident = new List<ResidentHandler>();
        for (int i = 0; i < children.Count; i++)
        {
            currentResident.Add(children[i].cubeGO.GetComponent<ResidentHandler>());
        }
        feedbackElements = new List<FeedbackElement>();

        checkDirections = new List<Vector3>
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };
    }

    private void OnDisable()
    {
        ClearFeedback();
    }

    public void CheckRelations()
    {
        if (Vector3.Distance(currentResident[0].gameObject.transform.position, previousPos) < .5f)
            return;

        previousPos = currentResident[0].gameObject.transform.position;
        RaycastHit[] hit;

        List<ResidentHandler> residents = new List<ResidentHandler>();
        int totalLike = 0;
        for (int index = 0; index < currentResident.Count; index++)
        {
            for (int i = 0; i < checkDirections.Count; i++)
            {
                hit = Physics.RaycastAll(currentResident[index].gameObject.transform.position, checkDirections[i], distance, mask);
                for (int j = 0; j < hit.Length; j++)
                {
                    if (hit[j].collider.gameObject == gameObject)
                        continue;

                    ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();
                    if (currentResident.Contains(collidedResident))
                        continue;

                    int likeAmount = currentResident[index].GetResident.CheckLikes(collidedResident.GetResidentRace);
                    if (likeAmount == 0)
                        continue;

                    residents.Add(collidedResident);

                    if (likeAmount > 0)
                    {
                        collidedResident.ShowRelationsMaterial(hit[j].point, likeColor);
                    }
                    else
                    {
                        collidedResident.ShowRelationsMaterial(hit[j].point, dislikeColor);
                    }

                    bool alreadyHere = false;
                    for (int k = 0; k < feedbackElements.Count; k++)
                    {
                        if (collidedResident == feedbackElements[k].neighbor)
                        {
                            alreadyHere = true;
                            break;
                        }
                    }
                    totalLike += likeAmount;
                    if (alreadyHere)
                        continue;

                    FeedbackElement element = new FeedbackElement();
                    element.neighbor = collidedResident;
                    element.currentResident = currentResident[index];
                    feedbackElements.Add(element);
                }
            }
        }

        popup?.DestroyPopup();
        popup = Instantiate<FeedbackPopup>(feedbackPopup, transform.position, Quaternion.identity);
        popup.InitPopup(totalLike);
        for (int k = feedbackElements.Count -1; k >=0; k--)
        {
            if (residents.Contains(feedbackElements[k].neighbor))
                continue;

            feedbackElements[k].neighbor.RemoveRelationsMaterial();
            feedbackElements.RemoveAt(k);
        }
    }

    public void CheckRelationsWithoutFeedback()
    {

        RaycastHit[] hit;

        List<ResidentHandler> residents = new List<ResidentHandler>();
        for (int index = 0; index < currentResident.Count; index++)
        {
            for (int i = 0; i < checkDirections.Count; i++)
            {
                hit = Physics.RaycastAll(currentResident[index].gameObject.transform.position, checkDirections[i], distance, mask);
                for (int j = 0; j < hit.Length; j++)
                {
                    if (hit[j].collider.gameObject == gameObject)
                        continue;

                    ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();
                    if (currentResident.Contains(collidedResident))
                        continue;

                    int likeAmount = currentResident[index].GetResident.CheckLikes(collidedResident.GetResidentRace);
                    if (likeAmount == 0)
                        continue;

                    residents.Add(collidedResident);

                    bool alreadyHere = false;
                    for (int k = 0; k < feedbackElements.Count; k++)
                    {
                        if (collidedResident == feedbackElements[k].neighbor)
                        {
                            alreadyHere = true;
                            break;
                        }
                    }

                    if (alreadyHere)
                        continue;

                    FeedbackElement element = new FeedbackElement();
                    element.neighbor = collidedResident;
                    element.currentResident = currentResident[index];
                    feedbackElements.Add(element);
                }
            }
        }

        for (int k = feedbackElements.Count - 1; k >= 0; k--)
        {
            feedbackElements[k].neighbor.NewNeighbors(feedbackElements[k].currentResident.GetResidentRace);
            feedbackElements[k].currentResident.NewNeighbors(feedbackElements[k].neighbor.GetResidentRace);
        }
        feedbackElements.Clear();
        Destroy(this);
    }

    public void ClearFeedback()
    {
        if (feedbackElements == null)
            return;

        popup?.DestroyPopup();
        for (int i = 0; i < feedbackElements.Count; i++)
        {
            feedbackElements[i].neighbor.RemoveRelationsMaterial();
        }
        feedbackElements.Clear();
    }

    public void ValidatePosition()
    {
        for (int i = 0; i < feedbackElements.Count; i++)
        {
            feedbackElements[i].neighbor.NewNeighbors(feedbackElements[i].currentResident.GetResidentRace);
            feedbackElements[i].currentResident.NewNeighbors(feedbackElements[i].neighbor.GetResidentRace);
        }
        Destroy(this);
    }
}

struct FeedbackElement
{
    public ResidentHandler currentResident;
    public ResidentHandler neighbor;
}
 