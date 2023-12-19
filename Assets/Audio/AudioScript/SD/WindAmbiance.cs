using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindAmbiance : AudioAmbiance
{
    [Header("Wind")]

    [SerializeField, Tooltip("Set the index of the first item of the \"second\" list")] private int divideIndex;
    [SerializeField, Tooltip("If the tower balance is greater than this %, the strong wind will play")
        ,Range(0,1)] private float switchPourcentage;
    private bool useBeginOfTheList = true;

    [Header("Reference")]
    [SerializeField] private GameplayDataSO gameplayData;
    private Grid3DManager grid;

    protected void Start()
    {
        grid = Grid3DManager.Instance;
        base.Start();
    }

    protected override IEnumerator LaunchRFX()
    {

        yield return new WaitForSeconds(1);

        float WaitTimeRFX = Random.Range(rndWaitTimeMin, rndWaitTimeMax);
        Vector3 rndPosRFX = RandomPointInBounds(Zone.bounds);
        yield return new WaitForSeconds(WaitTimeRFX);
        useBeginOfTheList = IsTowerStable();
        AudioSFXOneShot rfxGO = Instantiate(emptyPrefab, rndPosRFX, Quaternion.identity).GetComponent<AudioSFXOneShot>();
        m_selectedClip = GetClip(true);

        rndWaitTimeMax = m_selectedClip.length - 1;
        rndWaitTimeMin = m_selectedClip.length - 1;

        rfxGO.AddClip(m_selectedClip);
        rfxGO.PlayAmbiance(AudioManager.AmbianceType.Wind);

        if (AudioDebug)
            Debug.Log($"RFX {m_selectedClip.name} from Container '{this.gameObject}' launched at {rndPosRFX}");
        StartCoroutine(LaunchRFX());
    }

    bool IsTowerStable()
    {
        float value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(Mathf.Abs(grid.BalanceValue.x), 0));
        if (value > switchPourcentage) return false;
        return true;
    }

    public override AudioClip GetClip(bool canHaveSameSoundTwice)
    {
        int startIndex = 0;
        int endIndex = divideIndex;
        if(!useBeginOfTheList)
        {
            startIndex = divideIndex;
            endIndex = _audioClipList.Count;
        }


        if (_audioClipList.Count == 0) return null;

        AudioClip selectedClip = _audioClipList[Random.Range(startIndex, endIndex)];

        if (endIndex - startIndex == 1 || canHaveSameSoundTwice) return selectedClip;


        while (selectedClip == m_previousClip)
        {
            selectedClip = _audioClipList[Random.Range(startIndex, endIndex)];
        }

        m_previousClip = selectedClip;
        if (selectedClip == null)
        {
            return null;
        }

        return selectedClip;
    }
}
