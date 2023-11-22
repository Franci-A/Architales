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

    [Header("Weight")]
    [SerializeField] private float shaderAnimTime;
    [SerializeField] private AnimationCurve shaderAnimCurve;
    [SerializeField] private EventScriptable onPiecePlaced;

    [Header("GameOver")]
    [SerializeField] int cubeDestroyProba;
    [SerializeField] float delayBtwBlast;
    [SerializeField] float explosionForce;
    [SerializeField] float radius;
    [SerializeField] float verticalExplosionForce;
    [SerializeField] GameObject explosionVFX;

    [Header("Debug")]
    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();
    [SerializeField] private Gradient debugWeightColors;

    private void Awake()
    {
        onPiecePlaced.AddListener(UpdateDisplacement);
        onPiecePlaced.AddListener(UpdateWeightDebug);
        Shader.SetGlobalFloat("_LeaningPower", 2);
        grid = GetComponent<Grid3DManager>();
    }

    public void DestroyTower()
    {
        StartCoroutine(DestroyTowerCoroutine());
    }

    public IEnumerator DestroyTowerCoroutine()
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
        Shader.SetGlobalVector("_LeaningDirection", grid.BalanceValue.normalized);
        Shader.SetGlobalFloat("_MaxHeight", grid.GetHigherBlock);
        StartCoroutine(BalanceDisplacementRoutine());
    }

    private void ResetDisplacement()
    {
        Shader.SetGlobalVector("_LeaningDirection", Vector2.zero);
        Shader.SetGlobalFloat("_MaxHeight", 1f);

        SetDisplacementValue(0f);
    }
    private IEnumerator BalanceDisplacementRoutine()
    {
        float timer = shaderAnimTime;
        float maxValue = Mathf.Clamp01(Mathf.Max(Mathf.Abs(grid.BalanceValue.x), Mathf.Abs(grid.BalanceValue.y)) / gameplayData.MaxBalance);
        float t;
        do
        {
            // 0-1 of time elapsed
            t = 1 - Mathf.InverseLerp(0f, shaderAnimTime, timer);
            SetDisplacementValue(shaderAnimCurve.Evaluate(t) * maxValue);

            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        } while (timer > 0);

        SetDisplacementValue(0f);
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

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(UpdateWeightDebug);
        onPiecePlaced.RemoveListener(UpdateDisplacement);
        ResetDisplacement();
    }

}
