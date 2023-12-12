using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class AudioRFXContainer
{
    public AudioClip m_selectedClip;

    public string RFX_Name;
    public GameObject RFX_Prefab;
    public BoxCollider RFX_Zone;

    [Space]

    [Range(0, 60)] public float RFX_rndWaitTimeMin;
    [Range(0, 60)] public float RFX_rndWaitTimeMax;

}

public class AudioRFXManager : AudioScript
{
    [Header("Container")]
    [SerializeField] private bool AudioDebug = false;

    [SerializeField] private List<AudioRFXContainer> rfxList;


    protected override void Awake()
    {
        base.Awake();

        foreach (AudioRFXContainer rfx in rfxList)
        {
            if (rfx.RFX_Prefab != null && rfx.RFX_Zone != null)
            {
                StartCoroutine(LaunchRFX(rfx));
            }
        }
    }

    
    float rndWaitTimeRFX(AudioRFXContainer rfxPrefab)
    {
        return Random.Range(rfxPrefab.RFX_rndWaitTimeMin, rfxPrefab.RFX_rndWaitTimeMax);
    }

    IEnumerator LaunchRFX(AudioRFXContainer rfxPrefab)
    {
        yield return new WaitForSeconds(1);
        
        float WaitTimeRFX = rndWaitTimeRFX(rfxPrefab);
        Vector3 rndPosRFX = RandomPointInBounds(rfxPrefab.RFX_Zone.bounds);
        yield return new WaitForSeconds(WaitTimeRFX);       
        AudioSFXOneShot rfxGO = Instantiate(rfxPrefab.RFX_Prefab, rndPosRFX , Quaternion.identity).GetComponent<AudioSFXOneShot>();
        rfxGO.SetPreviousClip(rfxPrefab.m_selectedClip);
        rfxPrefab.m_selectedClip = rfxGO.GetClip(false);
        rfxGO.PlaySound();

        if (AudioDebug)
            Debug.Log($"RFX {rfxPrefab.m_selectedClip.name} from Container '{rfxPrefab.RFX_Name}' launched at {rndPosRFX}");
        StartCoroutine(LaunchRFX(rfxPrefab));
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
    }
}
