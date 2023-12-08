using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioRFXContainer
{
    [HideInInspector]
    public AudioClip m_selectedClip;

    public string RFX_Name;
    public GameObject RFX_Prefab;
    public BoxCollider RFX_Zone;
    [Space]

    [Range(0, 60)]
    public float RFX_rndWaitTimeMin;
    [Range(0, 60)]
    public float RFX_rndWaitTimeMax;

}
public class AudioRFXManager : MonoBehaviour
{
    [Space]

    public bool AudioDebug = false;

    public List<AudioRFXContainer> rfxList;



    // Start is called before the first frame update
    void Start()
    {
        if (rfxList.Count == 0)
            return;
        int i = 0;
        foreach(AudioRFXContainer rfx in rfxList)
        {
            if (rfx.RFX_Prefab != null && rfx.RFX_Zone != null)
            {
                StartCoroutine(LaunchRFX(i));
            }
            i++;
        }
    }

   
    // RFX 01 

    float rndWaitTimeRFX(int rfxPrefabIndex)
    {
        return Random.Range(rfxList[rfxPrefabIndex].RFX_rndWaitTimeMin, rfxList[rfxPrefabIndex].RFX_rndWaitTimeMax);
    }

    IEnumerator LaunchRFX(int rfxPrefabIndex)
    {
        
        float WaitTimeRFX = rndWaitTimeRFX(rfxPrefabIndex);
        Vector3 rndPosRFX = RandomPointInBounds(rfxList[rfxPrefabIndex].RFX_Zone.bounds);
        yield return new WaitForSeconds(WaitTimeRFX);
        AudioSFXOneShot rfxGO = Instantiate(rfxList[rfxPrefabIndex].RFX_Prefab, rndPosRFX , Quaternion.identity).GetComponent<AudioSFXOneShot>();
        rfxGO.SetPreviousClip(rfxList[rfxPrefabIndex].m_selectedClip);
        rfxList[rfxPrefabIndex].m_selectedClip = rfxGO.GetClip();
        rfxGO.PlaySound();

        if (AudioDebug)
            Debug.Log($"RFX {rfxList[rfxPrefabIndex].m_selectedClip.name} from Container '{rfxList[rfxPrefabIndex].RFX_Name}' launched at {rndPosRFX}");
        StartCoroutine(LaunchRFX(rfxPrefabIndex));
    }
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
    }
}
