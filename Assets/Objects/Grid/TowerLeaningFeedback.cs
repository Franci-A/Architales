using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLeaningFeedback : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] GridData data;
    [SerializeField] private GameplayDataSO gameplayData;
    private Grid3DManager grid;
    [SerializeField] private EventScriptable onPiecePlaced;

    [Header("Weight")]
    [SerializeField] private float shaderAnimTime = 2;
    [SerializeField] private AnimationCurve shaderAnimCurve;
    [SerializeField, Range(0, 1)] private float beginDisplacementValue = .35f;
    [SerializeField] private float displacementPower = 1.8f;
    [SerializeField] private float heightMax = 10;

    [Header("GameOver")]
    [SerializeField] int cubeDestroyProba = 40;
    [SerializeField] float delayBtwBlast = 0.001f;
    [SerializeField] float explosionForce = 500;
    [SerializeField] float radius= 50;
    [SerializeField] float verticalExplosionForce = 1;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] EventScriptable onBalanceBroken;
    [SerializeField] AudioPlayCollision audioPlayCollision;
    [SerializeField] GameObject destructionSfx;

    [Header("Tower Fall")]
    [SerializeField] private float pushPower;
    [SerializeField] private float heightMultiplier;

    [Header("Weight")]
    [SerializeField] List<Image> DebugInfo = new List<Image>();
    [SerializeField] private Gradient debugWeightColors;
    [SerializeField] private BoolVariable autoDestroyTower;

    [Header("Audio")]
    [Range(0,1), SerializeField] private float ReleaseTime;
    //[SerializeField] private AudioSFXOneShot oneShotSound;
    [SerializeField] private GameObject Tension;
    [SerializeField] private GameObject Release;

    private bool isBalanceBroken = false;

    private void Awake()
    {
        onPiecePlaced.AddListener(UpdateWeight);
        Shader.SetGlobalFloat("_LeaningPower", displacementPower);
        grid = GetComponent<Grid3DManager>();
    }

    private void Start()
    {
        onBalanceBroken.AddListener(OnBalanceBroken);
    }

    private void OnBalanceBroken()
    {
        isBalanceBroken = true;
    }

    private void DestroyTower()
    {
        StartCoroutine(TowerFallCoroutine());
        // StartCoroutine(ExplodeTowerCoroutine());
    }

    private IEnumerator TowerFallCoroutine()
    {
        List<CubeHandler> cubes = new List<CubeHandler>(data.GetCubeHandlers);
        Vector3 direction = new Vector3(grid.BalanceValue.normalized.x, 0 ,grid.BalanceValue.normalized.y);
        Debug.Log(direction);
        float maxHieght = Grid3DManager.Instance.GetHigherBlock;
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] == null)
                continue;
            if (cubes[i].GetResident.GetResidentRace == Race.Lobby)
                continue;

            cubes[i].GetRigidBody.isKinematic = false;
            cubes[i].GetAudioCollision.SetData(audioPlayCollision);
            float cubeHeightMultiplier = Mathf.Lerp(0, heightMultiplier, cubes[i].transform.position.y / maxHieght);
            cubes[i].GetRigidBody.AddForce(direction * pushPower * cubeHeightMultiplier, ForceMode.Impulse); //change direction
        }
        yield return new WaitForSeconds(.2f);
        float t = .75f;
        while (t > 0)
        {
            yield return new WaitForFixedUpdate();
            t -= Time.fixedDeltaTime * .8f;
            SetDisplacementValue(t);
        }

    }


    private IEnumerator ExplodeTowerCoroutine()
    {
        List<CubeHandler> cubes = data.GetCubeHandlers;
        List<int> intcubes = new List<int>();

        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].GetRigidBody.isKinematic = false;
            if (UnityEngine.Random.Range(0, 100) < cubeDestroyProba)
            {
                intcubes.Add(i);
            }
            cubes[i].GetAudioCollision.SetData(audioPlayCollision);
        }

        for (int i = 0; i < intcubes.Count; i++)
        {
            cubes[intcubes[i]].GetRigidBody.AddExplosionForce(explosionForce, cubes[intcubes[i]].transform.position, radius, verticalExplosionForce); //change direction
            var vfx = Instantiate(explosionVFX, cubes[intcubes[i]].transform.position, transform.rotation);
            Destroy(vfx, 3);

            Instantiate(destructionSfx);
            yield return new WaitForSeconds(delayBtwBlast);
        }

    }

    private void SetDisplacementValue(float value)
    {
        Shader.SetGlobalFloat("_Value", value);
    }

    private void UpdateDisplacement()
    {
        StartCoroutine(BalanceDisplacementRoutine());
    }

    private void ResetDisplacement()
    {
        Shader.SetGlobalVector("_LeaningDirection", Vector2.zero);
        Shader.SetGlobalFloat("_MaxHeight", 1f);

        SetDisplacementValue(0f);
    }
    public IEnumerator BalanceDisplacementRoutine()
    {
        Shader.SetGlobalVector("_LeaningDirection", grid.BalanceValue.normalized);
        Shader.SetGlobalFloat("_MaxHeight", grid.GetHigherBlock);
        float maxValue = Mathf.Max(Mathf.Abs(grid.BalanceValue.x), Mathf.Abs(grid.BalanceValue.y));
        float value = Mathf.InverseLerp(0, gameplayData.MaxBalance, maxValue);
        bool once = false;
        
        
        if (value >= beginDisplacementValue)
        {
            Instantiate(Tension);
            float height = Mathf.Clamp01(Grid3DManager.Instance.GetHigherBlock / heightMax);
            float disPowerWithHeight = Mathf.Lerp(0, displacementPower, height);
            float displacment = Mathf.Lerp(0, disPowerWithHeight, value);
            Shader.SetGlobalFloat("_LeaningPower",displacment);
            float maxTimer = Mathf.Lerp(0, shaderAnimTime, value);
            float timer = maxTimer;
            float t;
            do
            {
                // 0-1 of time elapsed
                t = Mathf.InverseLerp(maxTimer, 0, timer);
                SetDisplacementValue(shaderAnimCurve.Evaluate(t) * value);

                if(t > ReleaseTime && !once)
                {
                    once = true;
                    Instantiate(Release);
                }

                timer -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            } while (!isBalanceBroken && timer > 0 || (isBalanceBroken && autoDestroyTower.value) && timer > maxTimer * .25f);

            if(!(isBalanceBroken && autoDestroyTower.value))
                SetDisplacementValue(0f);
        }


        if (isBalanceBroken && autoDestroyTower.value)
        {
            DestroyTower();
        }

    }


    private void UpdateWeight()
    {
        for (int i = 0; i < DebugInfo.Count; i++)
        {
            //DebugInfo[i].transform.rotation = Quaternion.LookRotation(DebugInfo[i].transform.position - Camera.main.transform.position);
            float value = 0;
            switch (i)
            {
                case 0:
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(-grid.BalanceValue.x, 0));
                    break;

                case 1:
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(grid.BalanceValue.x, 0));

                    break;

                case 2:
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(-grid.BalanceValue.y, 0));
                    break;

                case 3:
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(grid.BalanceValue.y, 0));
                    break;
            }
            DebugInfo[i].color = debugWeightColors.Evaluate(value);
        }
        
    }

    private void OnDisable()
    {
        onPiecePlaced.RemoveListener(UpdateWeight);
        onBalanceBroken.RemoveListener(OnBalanceBroken);

        ResetDisplacement();
    }
}
