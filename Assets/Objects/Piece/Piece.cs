using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeField] private BlockBuilder blockBuilder;
    [SerializeField] private PieceHappinessHandler happinessHandler;

    List<Cube> cubes = new List<Cube>();
    public List<Cube> Cubes { get => cubes; }

    private Resident currentResident;
    public Resident GetResident { get => currentResident; }

    private Vector3 baseGridPosition;
    public Vector3 GetGridPosition { get => baseGridPosition; }

    private void SpawnCubes(bool disableCollider)
    {
        foreach (var cube in cubes)
        {
            cube.gridPosition = baseGridPosition + cube.pieceLocalPosition;

            var instance = blockBuilder.CreateBlock(this, cube.gridPosition);

            if(disableCollider)
                instance.GetComponent<Collider>().enabled = false;

            var residentHandler = instance.GetComponent<ResidentHandler>();
            residentHandler.SetResident(currentResident);
            residentHandler.parentPiece = this;
            
            cube.cubeGO = instance.gameObject;
        }

        happinessHandler.Init();
    }

    private void UpdateCubes()
    {
        foreach (var cube in cubes)
        {
            blockBuilder.UpdateSurroundingBlocks(cube.gridPosition);
        }
    }

    public void SpawnPiece(PieceSO piece, Vector3 gridPos, bool disableCollider = false)
    {
        baseGridPosition = gridPos;
        transform.position = gridData.GridToWorldPosition(baseGridPosition);

        ChangePiece(piece);
        SpawnCubes(disableCollider);

        UpdateCubes();
    }

    public void ChangePiece(PieceSO piece)
    {
        if(piece.cubes.Count < 0) return;

        currentResident = piece.resident;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        cubes = piece.cubes;
    }
    
    public void ChangeCubes(List<Cube> _cubes)
    {
        if(_cubes.Count < 0) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        cubes = _cubes;
    }


    public List<Cube> Rotate(bool rotateLeft)
    {
        Quaternion rotation;
        if (rotateLeft)
        {
            rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }

        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        List<Cube> rotatedBlocks = new List<Cube>();

        foreach (Cube cube in cubes)
        {
            Cube newBlock = new Cube();
            newBlock.pieceLocalPosition = m.MultiplyPoint3x4(cube.pieceLocalPosition);
            newBlock.pieceLocalPosition = new Vector3(MathF.Round(newBlock.pieceLocalPosition.x), MathF.Round(newBlock.pieceLocalPosition.y),
                MathF.Round(newBlock.pieceLocalPosition.z));
            rotatedBlocks.Add(newBlock);
        }

        ChangeCubes(rotatedBlocks);
        return rotatedBlocks;
    }

    public void CheckResidentLikesImpact()
    {
        CheckResidentsLikes checkResidents = GetComponent<CheckResidentsLikes>();
        checkResidents.Init(Cubes);
        checkResidents.CheckRelations();
        checkResidents.ValidatePosition();
    }
}

[Serializable]
public class Cube
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
    public GameObject cubeGO;
}
