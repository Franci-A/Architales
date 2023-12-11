using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [Header("GameOver")]
    [SerializeField] int cubeDestroyProba = 40;
    [SerializeField] float delayBtwBlast = 0.001f;
    [SerializeField] float explosionForce = 500;
    [SerializeField] float radius= 50;
    [SerializeField] float verticalExplosionForce = 1;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] EventScriptable onBalanceBroken;

    [Header("Debug")]
    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();
    [SerializeField] private Gradient debugWeightColors;
    [SerializeField] private BoolVariable autoDestroyTower;

    private bool isBalanceBroken = false;

    private void Awake()
    {
        onPiecePlaced.AddListener(UpdateWeightDebug);
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
        StartCoroutine(DestroyTowerCoroutine());
    }

    private IEnumerator DestroyTowerCoroutine()
    {
        List<GameObject> cubes = data.GetCubes();
        List<int> intcubes = new List<int>();

        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].AddComponent<Rigidbody>();
            if (Random.Range(0, 100) < cubeDestroyProba)
            {
                intcubes.Add(i);
            }
        }

        for (int i = 0; i < intcubes.Count; i++)
        {
            cubes[intcubes[i]].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, cubes[intcubes[i]].transform.position, radius, verticalExplosionForce);
            var vfx = Instantiate(explosionVFX, cubes[intcubes[i]].transform.position, transform.rotation);
            Destroy(vfx, 3);
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
        if (value >= beginDisplacementValue)
        {
            Shader.SetGlobalFloat("_LeaningPower", Mathf.Lerp(0, displacementPower, value));
            float maxTimer = Mathf.Lerp(0, shaderAnimTime, value);
            float timer = maxTimer;
            float t;
            do
            {
                // 0-1 of time elapsed
                t = Mathf.InverseLerp(maxTimer, 0, timer);
                SetDisplacementValue(shaderAnimCurve.Evaluate(t) * value);

                timer -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            } while (!isBalanceBroken && timer > 0 || (isBalanceBroken && autoDestroyTower.value) && timer > maxTimer * .2f);

            SetDisplacementValue(0f);
        }

        if (isBalanceBroken && autoDestroyTower.value)
        {
            DestroyTower();
        }
    }


    private void UpdateWeightDebug()
    {
        for (int i = 0; i < DebugInfo.Count; i++)
        {
            DebugInfo[i].transform.rotation = Quaternion.LookRotation(DebugInfo[i].transform.position - Camera.main.transform.position);
            float value = 0;
            switch (i)
            {
                case 0:
                    DebugInfo[i].text = Mathf.Max(-grid.BalanceValue.x, 0).ToString();
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(-grid.BalanceValue.x, 0));
                    break;

                case 1:
                    DebugInfo[i].text = Mathf.Max(grid.BalanceValue.x, 0).ToString();
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(grid.BalanceValue.x, 0));

                    break;

                case 2:
                    DebugInfo[i].text = Mathf.Max(-grid.BalanceValue.y, 0).ToString();
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(-grid.BalanceValue.y, 0));
                    break;

                default:
                    DebugInfo[i].text = Mathf.Max(grid.BalanceValue.y, 0).ToString();
                    value = Mathf.InverseLerp(0, gameplayData.MaxBalance, Mathf.Max(grid.BalanceValue.y, 0));
                    break;
            }
            DebugInfo[i].color = debugWeightColors.Evaluate(value);
        }
        
    }

    private void OnDisable()
    {
        onPiecePlaced.RemoveListener(UpdateWeightDebug);
        onBalanceBroken.RemoveListener(OnBalanceBroken);

        ResetDisplacement();
    }
}
