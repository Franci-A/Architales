using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Asset Reference")]
    [SerializeField] private GridData gridData;
    [SerializeField] private BlockBuilder blockBuilder;

    [Header("Components")]
    [SerializeField] private PieceHappinessHandler happinessHandler;
    [SerializeField] private PieceDecorationsHandler DecorationsHandler;
    [SerializeField] private Transform blocksParentTransform;

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

            var instance = blockBuilder.CreateBlock(this, cube.gridPosition, blocksParentTransform, disableCollider);
/*
            if(disableCollider)
                instance.GetComponent<Collider>().enabled = false;*/

            var residentHandler = instance.GetComponent<ResidentHandler>();
            residentHandler.SetResident(currentResident);
            residentHandler.parentPiece = this;
            
            cube.cubeGO = instance.gameObject;
        }

        happinessHandler.Init();
    }

    private void UpdateSurroundingBlocks()
    {
        foreach (var cube in cubes)
        {
            blockBuilder.UpdateSurroundingBlocks(cube.gridPosition);
        }
    }

    public void SpawnPiece(PieceSO piece, Vector3 gridPos)
    {
        baseGridPosition = gridPos;
        transform.position = gridData.GridToWorldPosition(baseGridPosition);

        ChangePiece(piece);
        SpawnCubes(false);

        UpdateSurroundingBlocks();
        DecorationsHandler?.Init();


        if (piece.resident.vfxSmoke == null) return;

        var vfx = Instantiate(piece.resident.vfxSmoke, transform.position - centerLowerPiecePos(piece), transform.rotation);
        Destroy(vfx, 3);
    }

    public void PreviewSpawnPiece(PieceSO piece, Vector3 gridPos)
    {
        baseGridPosition = gridPos;
        transform.position = gridData.GridToWorldPosition(baseGridPosition);

        ChangePiece(piece);
        SpawnCubes(true);
    }

    public void ChangePiece(PieceSO piece)
    {
        if(piece.cubes.Count < 0) return;

        currentResident = piece.resident;

        for (int i = 0; i < blocksParentTransform.childCount; i++)
        {
            Destroy(blocksParentTransform.GetChild(i).gameObject);
        }

        cubes = piece.cubes;
    }
    
    public void ChangeCubes(List<Cube> _cubes)
    {
        if(_cubes.Count < 0) return;

        for (int i = 0; i < blocksParentTransform.childCount; i++)
        {
            Destroy(blocksParentTransform.GetChild(i).gameObject);
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
        checkResidents.CheckRelationsWithoutFeedback();
    }

    public Vector3 centerPiecePos(PieceSO _piece)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        float minZ = 0;
        float maxZ = 0;

        foreach (var cube in _piece.cubes)
        {
            if (cube.pieceLocalPosition.x < minX) minX = cube.pieceLocalPosition.x;
            else if (cube.pieceLocalPosition.x > maxX) maxX = cube.pieceLocalPosition.x;

            if (cube.pieceLocalPosition.y < minY) minY = cube.pieceLocalPosition.y;
            else if (cube.pieceLocalPosition.y > maxY) maxY = cube.pieceLocalPosition.y;

            if (cube.pieceLocalPosition.z < minZ) minZ = cube.pieceLocalPosition.z;
            else if (cube.pieceLocalPosition.z > maxZ) maxZ = cube.pieceLocalPosition.z;
        }

        return new Vector3((maxX + minX) / -2, (maxY + minY) / -2, (maxZ + minZ) / -2);
    }

    public Vector3 centerLowerPiecePos(PieceSO _piece)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float minZ = 0;
        float maxZ = 0;

        foreach (var cube in _piece.cubes)
        {
            if (cube.pieceLocalPosition.x < minX) minX = cube.pieceLocalPosition.x;
            else if (cube.pieceLocalPosition.x > maxX) maxX = cube.pieceLocalPosition.x;

            if (cube.pieceLocalPosition.y < minY) minY = cube.pieceLocalPosition.y;

            if (cube.pieceLocalPosition.z < minZ) minZ = cube.pieceLocalPosition.z;
            else if (cube.pieceLocalPosition.z > maxZ) maxZ = cube.pieceLocalPosition.z;
        }

        return new Vector3((maxX + minX) / -2, (minY) / -2, (maxZ + minZ) / -2);
    }

}

[Serializable]
public class Cube
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
    public GameObject cubeGO;
}
