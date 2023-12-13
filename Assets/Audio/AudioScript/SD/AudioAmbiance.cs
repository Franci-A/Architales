using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbiance : AudioScript
{
    [SerializeField] protected BoxCollider Zone;
    [SerializeField] protected AudioSFXOneShot emptyPrefab;

    [Space]

    [Range(0, 120), SerializeField] protected float rndWaitTimeMin;
    [Range(0, 120), SerializeField] protected float rndWaitTimeMax;

    [Space]

    [SerializeField] protected bool AudioDebug = false;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Awake();
        StartCoroutine(LaunchRFX());
    }

    protected virtual IEnumerator LaunchRFX()
    {
        yield return new WaitForSeconds(1);

        float WaitTimeRFX = Random.Range(rndWaitTimeMin, rndWaitTimeMax);
        Vector3 rndPosRFX = RandomPointInBounds(Zone.bounds);
        yield return new WaitForSeconds(WaitTimeRFX);
        AudioSFXOneShot rfxGO = Instantiate(emptyPrefab, rndPosRFX, Quaternion.identity).GetComponent<AudioSFXOneShot>();
        m_selectedClip = GetClip(true);
        rfxGO.AddClip(m_selectedClip);
        rfxGO.PlaySound();

        if (AudioDebug)
            Debug.Log($"RFX {m_selectedClip.name} from Container '{this.gameObject}' launched at {rndPosRFX}");
        StartCoroutine(LaunchRFX());
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
    }
}
