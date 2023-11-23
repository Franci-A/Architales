using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "ScriptableObjects/Block Builder/Asset List")]
public class BlockAssetTypeRaceList : ScriptableObject, InitializeOnAwake
{
    [Serializable]
    private class RaceAssetTypeMesh
    {
        [SerializeField] private RaceAssetTypePair pair;
        public RaceAssetTypePair Pair { get => pair; }

        [SerializeField] private Mesh mesh;
        public Mesh AssetMesh { get => mesh; }

        public RaceAssetTypeMesh(RaceAssetTypePair pair)
        {
            this.pair = pair;
            mesh = null;
        }
    }

    [Serializable]
    private struct RaceAssetTypePair
    {
        public Race race;
        public BlockAssetType assetType;

        public RaceAssetTypePair(Race race, BlockAssetType assetType)
        {
            this.race = race;
            this.assetType = assetType;
        }
    }

    [SerializeField] private List<RaceAssetTypeMesh> assetDatabase;

    private Dictionary<RaceAssetTypePair, Mesh> assetDictionnary;

    public void Initialize()
    {
        // Dictionnaries can't be Serialized, Need to be Initialized
        GenerateDictionnary();
    }

    public Mesh GetMeshByRaceAndType(Race race, BlockAssetType type)
    {
        RaceAssetTypePair pair = new RaceAssetTypePair(race, type);
        return assetDictionnary[pair];
    }

    [Button("Regenerate Empty Editor Database")]
    private void GenerateEmptyDatabase()
    {
        assetDatabase.Clear();

        foreach (BlockAssetType type in Enum.GetValues(typeof(BlockAssetType)))
        {
            foreach (Race race in Enum.GetValues(typeof(Race)))
            {
                RaceAssetTypePair pair = new RaceAssetTypePair(race, type);
                assetDatabase.Add(new RaceAssetTypeMesh(pair));
            }
        }
    }

    [Button("Regenerate Internal Database")]
    private void GenerateDictionnary()
    {
        assetDictionnary = new();
        foreach (var data in assetDatabase)
        {
            assetDictionnary.Add(data.Pair, data.AssetMesh);
        }
    }
}
