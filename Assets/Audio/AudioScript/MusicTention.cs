using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTention : MonoBehaviour
{
    private int currentHigh;
    private HighState state;

    [Header("Low Tention")]
    [SerializeField] AudioClip TransitionToMedMusic;
    [SerializeField] int levelToMedium;

    [Header("Medium Tention")]
    [SerializeField] AudioClip MedTensionMusic;
    [SerializeField] AudioClip TransitionToHighMusic;
    [SerializeField] int levelToHigh;

    [Header("High Tention")]
    [SerializeField] AudioClip HighTensionMusic;
    
    enum HighState
    {
        Low,
        Medium,
        High
    }


    private void Start()
    {
        Grid3DManager.Instance.OnLayerCubeChange += UpdateMaximumHigh;
    }

    void UpdateMaximumHigh(int higherValue)
    {
        currentHigh = higherValue;
        CheckMusicChange();
    }

    void CheckMusicChange()
    {
        if (currentHigh > levelToHigh && state == HighState.Medium)
        {
            StartCoroutine(SwitchSound(TransitionToHighMusic, HighTensionMusic));
            state = HighState.High;
        }
        else if (currentHigh > levelToMedium && state == HighState.Low)
        {
            StartCoroutine(SwitchSound(TransitionToMedMusic, MedTensionMusic));
            state = HighState.Medium;
        }

    }

    IEnumerator SwitchSound(AudioClip transition, AudioClip music)
    {
        AudioManager.Instance.PlayMusic(transition);

        yield return new WaitForSeconds(transition.length);

        AudioManager.Instance.PlayMusic(music);

    }
}
