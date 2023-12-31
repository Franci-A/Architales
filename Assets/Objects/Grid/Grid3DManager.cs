using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Grid3DManager : MonoBehaviour
{
    // Singleton
    private static Grid3DManager instance;
    public static Grid3DManager Instance { get => instance; }
    [SerializeField] private GameplayDataSO gameplayData;
    [SerializeField] private BoolVariable canPlaceBlock;
    [SerializeField] private BoolVariable isPlayerActive;

    [Header("Grid")]
    [SerializeField] GridData data;
    public int GetHigherBlock { get => higherBlock; }
    int higherBlock = 1;

    bool isBalanceBroken;
    private Vector2 balance;
    private float additionalBalance;

    [Header("Residents")]
    [SerializeField] private IntVariable totalNumResidents;

    [Header("Mouse Check")]
    [SerializeField] private float maxDistance = 15;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private CameraManager cameraManager;
    private MouseMode mouseMode;

    [Header("Piece")]
    [SerializeField] PieceSO lobbyPiece;
    [SerializeField] private Piece piece;
    [SerializeField] ListOfBlocksSO pieceListRandom; // liste des trucs random

    [Header("Event")]
    [SerializeField] private EventScriptable onEventEnd;
    [SerializeField] private EventScriptable onPiecePlaced;
    [SerializeField] private EventObjectScriptable onPiecePlacedObject;
    [SerializeField] private EventObjectScriptable lastPiecePlaced;
    [SerializeField] private EventObjectScriptable previewPieceChanged;
    [SerializeField] public EventScriptable onBalanceBroken;
    public delegate void OnCubeChangeDelegate(PieceSO newPiece);
    public event OnCubeChangeDelegate OnCubeChange;
    public delegate void OnLayerCubeChangeDelegate(int higherCubeValue);
    public event OnLayerCubeChangeDelegate OnLayerCubeChange;

    private List<Cube> cubeList; // current list
    public PieceSO CurrentPiece { get => currentPiece; }
    private PieceSO currentPiece; // current piece

    public PieceSO pieceSo { get => currentPiece; } // get
    public List<Cube> CubeList { get => cubeList; } // get

    private TowerLeaningFeedback feedback;

    public Vector2 BalanceValue => balance * gameplayData.balanceMultiplierVariable.value;

    public float MaxDistance { get => maxDistance;}
    public LayerMask CubeLayer { get => cubeLayer;}

    [Header("AudioEvent")]
    [SerializeField] private GameObject placeSFX;
    [SerializeField] private GameObject rotateSFX;

    public enum MouseMode
    {
        PlacePiece,
        AimPiece,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        feedback = GetComponent<TowerLeaningFeedback>();
        SpawnBase();
    }


    public void PlacePiece(Vector3 gridPos)
    {
        canPlaceBlock.SetValue(false);

        var piece = Instantiate(this.piece, transform);

        PieceSO pieceSO = ScriptableObject.CreateInstance<PieceSO>();
        pieceSO.cubes = new List<Cube>(CubeList);
        pieceSO.resident = currentPiece.resident;

        piece.SpawnPiece(pieceSO, gridPos);
        piece.CheckResidentLikesImpact();

        foreach (var block in piece.Cubes)
        {
            data.AddToGrid(block.gridPosition, block.cubeGO);
            UpdateWeight(block.gridPosition);

            if (block.gridPosition.y > higherBlock)
                higherBlock = (int)block.gridPosition.y;
        }

        totalNumResidents.Add(piece.Cubes.Count);

        lastPiecePlaced.Call(pieceSO);

        if (!EventManager.Instance.IsEventActive) ChangePieceSORandom();
        else onEventEnd.Call();

        onPiecePlaced.Call();
        onPiecePlacedObject.Call(piece.GetHappinessHandler);
        OnLayerCubeChange?.Invoke(higherBlock);
        StartCoroutine(WaitForFeedback());
    }

    IEnumerator WaitForFeedback()
    {
        yield return StartCoroutine(feedback.BalanceDisplacementRoutine());
        ChangedBlock();
    }

    private void SpawnBase()
    {
        cubeList = lobbyPiece.cubes;
        currentPiece = lobbyPiece;
        PlacePiece(Vector3.zero);
        canPlaceBlock.SetValue(true);
    }

    private void ChangedBlock()
    {
        PieceSO pieceSO = ScriptableObject.CreateInstance<PieceSO>();
        pieceSO.cubes = CubeList;
        pieceSO.resident = currentPiece.resident;
        OnCubeChange?.Invoke(pieceSO);
        piece.ChangePiece(pieceSO);
        canPlaceBlock.SetValue(true);
    }

    private void RotatePiece(bool rotateLeft)
    {
        cubeList = piece.Rotate(rotateLeft);
        Instantiate(rotateSFX);
        ChangedBlock();

        cameraManager.resetTimer();
    }

    private void TryPlacePiece()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer)) return;

        Vector3 gridPos = data.WorldToGridPosition(hit.point + hit.normal / 4f);

        if (data.IsPiecePlaceValid(piece, gridPos, out Vector3 validPos))
        {
            PlacePiece(validPos);
            Instantiate(placeSFX);
            cameraManager.resetTimer();
        }
    }

    private void TryAimPiece()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer)) return;

        Vector3 gridPos = data.WorldToGridPositionRounded(hit.collider.gameObject.transform.position);

        if (data.IsPieceDeletable(gridPos))
            DeletePiece(hit.collider.gameObject, gridPos);
    }

    private void DeletePiece(GameObject cube, Vector3 gridPos)
    {
        data.RemoveToGrid(gridPos);
        cube.GetComponent<ResidentHandler>().parentPiece.DestroyCube(cube);
        onEventEnd.Call();
    }

    private void ChangePieceSORandom()
    {
        currentPiece = pieceListRandom.GetRandomPiece();
        cubeList = currentPiece.cubes;

        previewPieceChanged.Call(currentPiece);
    }

    public void ChangePieceSO(PieceSO _current)
    {
        currentPiece = _current;
        cubeList = currentPiece.cubes;

        previewPieceChanged.Call(_current);
        ChangedBlock();
    }

    private void UpdateWeight(Vector3 gridPosistion)
    {
        balance.x += gridPosistion.x;
        balance.y += gridPosistion.z;

        if (!isBalanceBroken)
        {
            if (Mathf.Abs(BalanceValue.x) > gameplayData.MaxBalance + additionalBalance)
            {
                isBalanceBroken = true;
                onBalanceBroken.Call();
            }

            if (Mathf.Abs(BalanceValue.y) > gameplayData.MaxBalance + additionalBalance)
            {
                isBalanceBroken = true;
                onBalanceBroken.Call();
            }
        }
    }

    public void SwitchMouseMode(MouseMode newMode)
    {
        mouseMode = newMode;
    }

    public void AddBalance(float addBalance)
    {
        additionalBalance += addBalance;
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
    //INPUTS
    public void LeftClickInput(InputAction.CallbackContext context)
    {
        if(!isPlayerActive) return;
        if (!context.performed || isBalanceBroken || !canPlaceBlock.value) return;

        if (mouseMode == MouseMode.PlacePiece) TryPlacePiece();
        else TryAimPiece();
    }

    public void RotatePieceInput(InputAction.CallbackContext context)
    {
        if (!isPlayerActive) return;

        if (!context.performed || isBalanceBroken) return;
        RotatePiece(context.ReadValue<float>() < 0);
    }

    public void ZoomInput(InputAction.CallbackContext context)
    {
        if (!isPlayerActive) return;

        if (!context.performed) return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer)) 
            RotatePiece(context.ReadValue<float>() < 0);
    }
}
