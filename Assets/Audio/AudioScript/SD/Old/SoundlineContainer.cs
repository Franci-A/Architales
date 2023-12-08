using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SoundlineContainer : MonoBehaviour
{
    // Kudos to Cujo Sound for this amazing script : https://www.youtube.com/watch?v=IGmkDpNSpB8
    // I just readapted this script to replace child actors by spline knobs data.
    
    private Vector3[] soundlinePoints;
    private int soundlineCount;
    private SplineContainer spline;
    private GameObject _soundPointInstance;

    public GameObject _soundPointPrefab;
    public ControlSwitch _followGameObject;
    public bool debug_DrawSoundLine = true;

    // Start is called before the first frame update
    void Start()
    {
        

        spline = GetComponent<SplineContainer>();

        soundlineCount = spline.Spline.Count;
        soundlinePoints = new Vector3[soundlineCount];
        
        int i = 0;
        foreach(BezierKnot knot in spline.Spline.Knots)
        {
            //Construct the array of soundline points based on spline knobs
            soundlinePoints[i] = transform.position + new Vector3(knot.Position.x, knot.Position.y, knot.Position.z);
            i++;
        }
        _soundPointInstance = (_soundPointPrefab != null) ? Instantiate(_soundPointPrefab, transform) : null;
    }

    // Update is called once per frame
    void Update()
    {
        if(soundlineCount > 1)
        {
            if(_soundPointInstance != null)
            {
                _soundPointInstance.transform.position = WhereOnSpline(_followGameObject.GetCurrentController().transform.position);
            }
            //Display the soundline on Debug mode
            if (debug_DrawSoundLine)
            {
                for (int i = 0; i < soundlineCount; i++)
                {
                    if (i + 1 < soundlineCount)
                        Debug.DrawLine(soundlinePoints[i], soundlinePoints[i + 1], Color.green);
                }
            }
        }
    }

    public Vector3 WhereOnSpline(Vector3 pos)
    {
        int closestSoundlinePoint = GetClosestPoints(pos);
        if(closestSoundlinePoint == 0)
        {
            return SoundlineSegment(soundlinePoints[0], soundlinePoints[1], pos);
        }
        else if (closestSoundlinePoint == soundlineCount -1)
        {
            return SoundlineSegment(soundlinePoints[soundlineCount - 1], soundlinePoints[soundlineCount -2],pos);
        }
        else
        {
            Vector3 leftSeg = SoundlineSegment(soundlinePoints[closestSoundlinePoint - 1], soundlinePoints[closestSoundlinePoint], pos);
            Vector3 rightSeg = SoundlineSegment(soundlinePoints[closestSoundlinePoint + 1], soundlinePoints[closestSoundlinePoint], pos);
            if ((pos - leftSeg).sqrMagnitude <= (pos - rightSeg).sqrMagnitude)
            {
                return leftSeg;
            }
            else
                return rightSeg;
        }
    }
    private int GetClosestPoints(Vector3 pos)
    {
        int closestPoint = -1;
        float shortestDistance = 0.0f;

        for (int i=0; i<soundlineCount; i++)
        {
            float sqrDistance = (soundlinePoints[i] - pos).sqrMagnitude;
            if(shortestDistance == 0.0f || sqrDistance < shortestDistance)
            {
                shortestDistance = sqrDistance;
                closestPoint = i;
            }
        }
        return closestPoint;
    }

    public Vector3 SoundlineSegment (Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 v1ToPos = pos - v1;
        Vector3 seqDirection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(seqDirection, v1ToPos);

        if(distanceFromV1 < 0.0f)
        {
            return v1;
        }
        else if(distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude)
        {
            return v2;
        }
        else
        {
            Vector3 fromV1 = seqDirection * distanceFromV1;
            return v1 + fromV1;
        }
    }
}
