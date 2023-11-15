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

}

[Serializable]
public class Cube
{
    [HideInInspector] public Vector3 gridPosition;
    public Vector3 pieceLocalPosition;
    public GameObject cubeGO;
}
