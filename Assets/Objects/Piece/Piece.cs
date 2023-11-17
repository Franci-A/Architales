using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeField] private ResidentHandler cubePrefab;
    [SerializeField] private PieceHappinessHandler happinessHandler;
    [SerializeField] private Transform visualTrans;

    [SerializeField] private GameObject smokeVFX;

    List<Cube> cubes = new List<Cube>();
    Resident currentResident;
    public List<Cube> Cubes { get => cubes; }

    public void SpawnCubes()
    {
        foreach (var cube in cubes)
        {
            var cubeGO = Instantiate<ResidentHandler>(cubePrefab, visualTrans.position + cube.pieceLocalPosition, visualTrans.rotation, visualTrans);
            cubeGO.SetResident(currentResident);
            cubeGO.parentPiece = this;
            cube.gridPosition = gridData.WorldToGridPosition(transform.position) + cube.pieceLocalPosition;
            cube.cubeGO = cubeGO.gameObject;
        }
        happinessHandler.Init();
    }

    public void PlacePieceInFinalSpot(PieceSO piece)
    {
        ChangePiece(piece);
        SpawnCubes();
        CheckResidentsLikes[] checkResidents = GetComponentsInChildren<CheckResidentsLikes>();
        for (int i = 0; i < checkResidents.Length; i++)
        {
            checkResidents[i].CheckRelations();
            checkResidents[i].ValidatePosition();
        }
        Debug.Log($"{centerLowerPiecePos(piece)} - {transform.position} = {transform.position - centerLowerPiecePos(piece)}");
        var vfx = Instantiate(smokeVFX, transform.position - centerLowerPiecePos(piece), transform.rotation);
        Destroy(vfx, 3);
    }

    public void ChangePiece(PieceSO piece)
    {
        if(piece.cubes.Count < 0) return;

        currentResident = piece.resident;

        for (int i = 0; i < visualTrans.childCount; i++)
        {
            Destroy(visualTrans.GetChild(i).gameObject);
        }

        cubes = piece.cubes;
    }
    
    public void ChangeCubes(List<Cube> _cubes)
    {
        if(_cubes.Count < 0) return;

        for (int i = 0; i < visualTrans.childCount; i++)
        {
            Destroy(visualTrans.GetChild(i).gameObject);
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
