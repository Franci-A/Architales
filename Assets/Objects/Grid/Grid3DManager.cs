using HelperScripts.EventSystem;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grid3DManager : MonoBehaviour
{
    // Singleton
    private static Grid3DManager instance;
    public static Grid3DManager Instance { get => instance; }
    [SerializeField] private GameplayDataSO gameplayData;

    [Header("Grid")]
    [SerializeField] GridData data;
    public int GetHigherBlock { get => higherBlock; }
    int higherBlock = 1;

    [Header("Weight")]
    [SerializeField] private float shaderAnimTime;
    [SerializeField] private AnimationCurve shaderAnimCurve;
    bool isBalanceBroken;
    private Vector2 balance;

    [Header("Residents")]
    [SerializeField] private IntVariable totalNumResidents;

    [Header("Mouse Check")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask cubeLayer;

    [Header("Piece")]
    [SerializeField] PieceSO lobbyPiece;
    [SerializeField] private Piece piece;
    //[SerializeField] List<PieceSO> pieceListRandom = new List<PieceSO>(); // liste des trucs random
    [SerializeField] ListOfBlocksSO pieceListRandom; // liste des trucs random

    [Header("Event")]
    [SerializeField] private EventScriptable onPiecePlaced;
    [SerializeField] private EventObjectScriptable onPiecePlacedPiece;
    [SerializeField] public EventScriptable onBalanceBroken;
    public delegate void OnCubeChangeDelegate(PieceSO newPiece);
    public event OnCubeChangeDelegate OnCubeChange;
    public delegate void OnLayerCubeChangeDelegate(int higherCubeValue);
    public event OnLayerCubeChangeDelegate OnLayerCubeChange;

    [Header("Debug")]
    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();

    private List<Cube> cubeList; // current list
    public PieceSO CurrentPiece { get => currentPiece; }
    private PieceSO currentPiece; // current piece
    public PieceSO NextPiece { get => nextPiece; }
    private PieceSO nextPiece;

    public PieceSO pieceSo { get => currentPiece; } // get
    public List<Cube> CubeList { get => cubeList; } // get
    private List<Cube> _cubeList; // check si ca a changer

    [Header("GameOver")]
    [SerializeField] int cubeDestroyProba;
    [SerializeField] float delayBtwBlast;
    [SerializeField] float explosionForce;
    [SerializeField] float radius;
    [SerializeField] float verticalExplosionForce;
    [SerializeField] GameObject explosionVFX;

    public Vector2 BalanceValue => balance * gameplayData.balanceMultiplierVariable.value;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        data.Initialize();

        onPiecePlaced.AddListener(UpdateDisplacement);
    }

    private void Start()
    {
        SpawnBase();
        Shader.SetGlobalFloat("_LeaningPower",  2);
    }

    private void Update()
    {
        IsBlockChanged();
        UpdateWeightDebug();
    }

    //INPUTS
    public void LeftClickInput(InputAction.CallbackContext context)
    {
        if (!context.performed || isBalanceBroken) return;
        TryPlacePiece();
    }

    public void RotatePieceInput(InputAction.CallbackContext context)
    {
        if (!context.performed || isBalanceBroken) return;
        RotatePiece(context.ReadValue<float>() < 0);
    }



    public void PlacePiece(Vector3 gridPos)
    {
        var piece = Instantiate(this.piece, data.GridToWorldPosition(gridPos), Quaternion.identity, transform);
        PieceSO pieceSO = ScriptableObject.CreateInstance<PieceSO>();
        pieceSO.cubes = CubeList;
        pieceSO.resident = currentPiece.resident;
        piece.PlacePieceInFinalSpot(pieceSO);

        foreach (var block in piece.Cubes)
        {
            data.AddToGrid(block.pieceLocalPosition + gridPos, block.cubeGO);
            UpdateWeight(block.pieceLocalPosition + gridPos);

            if (block.gridPosition.y > higherBlock)
                higherBlock = (int)block.gridPosition.y;
        }

        totalNumResidents.Add(piece.Cubes.Count);

        if(!EventManager.Instance.IsEventActive)
        ChangePieceSORandom();
        
        onPiecePlaced.Call();
        OnLayerCubeChange?.Invoke(higherBlock);

    }

    private void SpawnBase()
    {
        cubeList = lobbyPiece.cubes;
        currentPiece = lobbyPiece;
        PlacePiece(Vector3.zero);
    }

    private void IsBlockChanged()
    {
        if (_cubeList != CubeList)
        {
            _cubeList = CubeList;
            PieceSO pieceSO = ScriptableObject.CreateInstance<PieceSO>();
            pieceSO.cubes = CubeList;
            pieceSO.resident = currentPiece.resident;
            OnCubeChange?.Invoke(pieceSO);
            piece.ChangePiece(pieceSO);
        }
    }

    private void RotatePiece(bool rotateLeft)
    {
        cubeList = piece.Rotate(rotateLeft);
    }

    private void TryPlacePiece()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer)) return;

        Vector3 gridPos = data.WorldToGridPosition(hit.point + hit.normal / 4f);

        if (data.IsPiecePlaceValid(piece, gridPos, out Vector3 validPos))
            PlacePiece(validPos);
    }

    private void ChangePieceSORandom()
    {
        if(nextPiece == null)
        {
            nextPiece = pieceListRandom.GetRandomPiece();
        }

        currentPiece = nextPiece;
        cubeList = currentPiece.cubes;
        nextPiece = pieceListRandom.GetRandomPiece();

        onPiecePlacedPiece.Call(nextPiece);

    }

    public void ChangePieceSO(PieceSO _current, PieceSO _next)
    {
        currentPiece = _current;
        cubeList = currentPiece.cubes;
        nextPiece = _next;

        onPiecePlacedPiece.Call(nextPiece);
    }

    private void UpdateWeight(Vector3 gridPosistion)
    {
        balance.x += gridPosistion.x;
        balance.y += gridPosistion.z;
    }

    private void UpdateDisplacement()
    {
        Shader.SetGlobalVector("_LeaningDirection", balance.normalized);
        Shader.SetGlobalFloat("_MaxHeight", higherBlock);
        StartCoroutine(BalanceDisplacementRoutine());
    }

    private void SetDisplacementValue(float value)
    {
        Shader.SetGlobalFloat("_Value", value);
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
        float maxValue = Mathf.Clamp01(Mathf.Max(Mathf.Abs(BalanceValue.x), Mathf.Abs(BalanceValue.y)) / gameplayData.MaxBalance);
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

            switch (i)
            {
                case 0:
                    DebugInfo[i].text = Mathf.Max(-balance.x, 0).ToString();
                    break;

                case 1:
                    DebugInfo[i].text = Mathf.Max(balance.x, 0).ToString();
                    break;

                case 2:
                    DebugInfo[i].text = Mathf.Max(-balance.y, 0).ToString();
                    break;

                default:
                    DebugInfo[i].text = Mathf.Max(balance.y, 0).ToString();
                    break;
            }
        }

        if (!isBalanceBroken)
        {
            if (Mathf.Abs(BalanceValue.x) > gameplayData.MaxBalance)
            {
                DebugInfo[0].color = Color.red;
                DebugInfo[1].color = Color.red;
                isBalanceBroken = true;
                onBalanceBroken.Call();
            }

            if (Mathf.Abs(BalanceValue.y) > gameplayData.MaxBalance)
            {
                DebugInfo[2].color = Color.red;
                DebugInfo[3].color = Color.red;
                isBalanceBroken = true;
                onBalanceBroken.Call();
            }
        }
    }


    public void SetSavedPiece(PieceSO _current, PieceSO _next)
    {
        currentPiece = _current;
        nextPiece = _next;
    }


    public IEnumerator DestroyTower()
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

        for (int i = 0;i < intcubes.Count; i++)
        {
            cubes[intcubes[i]].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, cubes[intcubes[i]].transform.position, radius, verticalExplosionForce);
            var vfx = Instantiate(explosionVFX, cubes[intcubes[i]].transform.position, transform.rotation);
            Destroy(vfx, 3);
            yield return new WaitForSeconds(delayBtwBlast);
        }
    }

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(UpdateDisplacement);
        ResetDisplacement();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).origin, Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).origin + Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).direction * maxDistance);

        if (!data) return;

        for (int z = -1; z < 2; ++z)
            for (int x = -1; x < 2; ++x)
                Gizmos.DrawWireCube(data.GridToWorldPosition(new Vector3(x, 0, z)) + Vector3.down * data.CellSize * .5f, new Vector3(data.CellSize, 0, data.CellSize));

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            Vector3 gridPos = data.GridToWorldPosition(data.WorldToGridPosition(hit.point));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gridPos, Vector3.one * .9f * data.CellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridPos + Vector3.down * data.CellSize * .5f, new Vector3(data.CellSize, 0, data.CellSize));
        }
    }
}
