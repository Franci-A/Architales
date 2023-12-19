using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "ScriptableObjects/Block Builder/Asset List")]
public class BlockAssetTypeRaceList : ScriptableObject, InitializeOnAwake
{
    [Serializable]
    public class RaceAssetTypePair
    {
        public Race race;
        public Material[] blockMaterial;
        public Material[] roofMaterial;
        public Material[] supportMaterial;
        public Material[] windowMaterial;
        public Mesh wallMesh;
        public Mesh roofMesh;
        public Mesh supportMesh;
        public Mesh windowMesh;
        public Vector3 windowOffset;

        public RaceAssetTypePair(Race race)
        {
            this.race = race;
        }
    }

    [SerializeField] private List<RaceAssetTypePair> assetDatabase;

    private Dictionary<Race, RaceAssetTypePair> assetDictionnary;

    public void Initialize()
    {
        // Dictionnaries can't be Serialized, Need to be Initialized
        GenerateDictionnary();
    }

    public RaceAssetTypePair GetMeshByRace(Race race)
    {
        if(!assetDictionnary.ContainsKey(race))
        {
            Debug.LogWarning($"AssetDictionnary doesn't contain Pair<{race}>, OR a corresponding asset");
            return null;
        }

        return assetDictionnary[race];
    }

    [Button("Regenerate Empty Editor Database")]
    private void GenerateEmptyDatabase()
    {
        assetDatabase.Clear();

        foreach (BlockAssetType type in Enum.GetValues(typeof(BlockAssetType)))
        {
            foreach (Race race in Enum.GetValues(typeof(Race)))
            {
                RaceAssetTypePair pair = new RaceAssetTypePair(race);
                assetDatabase.Add(pair);
            }
        }
    }

    [Button("Regenerate Internal Database")]
    private void GenerateDictionnary()
    {
        assetDictionnary = new();
        foreach (var data in assetDatabase)
        {
            assetDictionnary.Add(data.race, data);
        }
    }
}
