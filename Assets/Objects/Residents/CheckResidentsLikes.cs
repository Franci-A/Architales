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

    private void Start()
    {
        onPiecePlaced.AddListener(ClearFeedback);
        feedbackInstances = new List<GameObject>();
        previousPos = transform.position;

        checkDirections = new List<Vector3>();
        checkDirections.Add(Vector3.forward);
        checkDirections.Add(Vector3.back);
        checkDirections.Add(Vector3.left);
        checkDirections.Add(Vector3.right);
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
        for (int i = 0; i < checkDirections.Count; i++)
        {
            hit = Physics.RaycastAll(transform.position, checkDirections[i], distance,mask);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.gameObject != gameObject)
                {
                    ResidentHandler collidedResident = hit[j].collider.gameObject.GetComponent<ResidentHandler>();
                    int like = currentResident.CheckLikes(collidedResident.GetResidentRace());
                    if (like == 1)
                    {
                        FeedbackPopup obj = Instantiate<FeedbackPopup>(feedbackPopup, hit[j].point, Quaternion.identity, transform);
                        obj.InitPopup(true);
                        feedbackInstances.Add(obj.gameObject);
                    }
                    else if (like == -1)
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
        //update happy / angry gauge
        Destroy(this);
    }
}
